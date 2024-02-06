using System;
using System.Collections.Generic;
using DuckTunes.Events;
using DuckTunes.ScriptableObjects;
using UnityEngine;
using Void = DuckTunes.Events.Void;

namespace DuckTunes.Systems.Music
{
    [RequireComponent(typeof(AudioSource))]
    public class Conductor : MonoBehaviour
    {
        [HideInInspector] public bool SpawnTargets = true;
        
        public float SongPositionInSec; //in seconds
        public float SongPositionInBeats;
        public int SongPositionInFullBeats => (int)SongPositionInBeats;
        private float _songDurationInSec => _currentClip.length;

        public TrackInfo TrackInfo;

        private AudioSource _source;
        private AudioClip _currentClip;
        private float[] noteTimes;
        
        private float _crotchet; //time duration of the beat in seconds
        private float _bpm;
        private float _offset; //Offset to the first beat in seconds
        private float _startTime; //Song start time

        private BeatMap _currentBeatMap;

        [SerializeField] private UnityVoidEvent _onSongEnd;
        private bool _invoked = false;
        [SerializeField] private UnityVoidEvent _onBeat;
        private int _lastBeat;

        private bool _paused;
        private bool _songStarted = false;
        private float _stopTime;

    
#region Public Functions

        public void Play(TrackInfo trackInfo)
        {
            _invoked = false;
            
            SaveTrackInfo(trackInfo);

            //Create beatmap from the midi file
            _currentBeatMap = new BeatMap(trackInfo, this);
            
            //Play audio once
            _source.clip = _currentClip;
            _source.Play();
            _songStarted = true;
        }

        public void Restart()
        {
            if (TrackInfo != null && !_songStarted)
            {
                Play(TrackInfo);
            }
        }
        
        public void Stop()
        {
            _source.Stop();
            _songStarted = false;
        }
        #endregion


#region Event Functions
        public void Pause()
        {
            _stopTime = _source.time;
            _source.Pause();
            //_source.enabled = false;
            _paused = true;
        }

        public void UnPause()
        {
            //_source.enabled = true;
            //_source.clip = _currentClip;
            _source.time = _stopTime;
            _source.UnPause();
            _paused = false;
        }

#endregion


#region Queries

        public float GetTime()
        {
            return SongPositionInSec;
        }

        public float GetBeatDuration()
        {
            return _bpm / 60f;
        }

        public void SetNoteTimes(float[] times)
        {
            noteTimes = times;
        }

        public void StartSpawning()
        {
            SpawnTargets = true;
        }

        public void StopSpawning()
        {
            SpawnTargets = false;
        }
#endregion


#region Private functions
        private void Awake()
        {
            _source = GetComponent<AudioSource>();
            _invoked = false;
        }
        
        private void Update()
        {
            if (!_songStarted || _paused) { return; }

            if (SongPositionInSec < _songDurationInSec)
            {
                CalculateSongPosition();
                _currentBeatMap.Update(SongPositionInSec, SpawnTargets);
            }
            else
            {
                if (!_invoked)
                {
                    _onSongEnd?.Invoke(new Void());
                    _invoked = true;
                    _songStarted = false;
                }
            }
        }

 

        private void CalculateSongPosition()
        {
            SongPositionInSec = Time.time - _startTime - _offset;
            SongPositionInBeats = SongPositionInSec / _crotchet;

            if ((int)SongPositionInBeats > _lastBeat)
            {
                _onBeat?.Invoke(new Void() );
                _lastBeat = (int)SongPositionInBeats;
            }
        }
     
        private void SaveTrackInfo(TrackInfo trackInfo)
        {
            if (trackInfo != null)
            {
                TrackInfo = trackInfo;
            }
            
            //Set Track data for calculating song position
            if (trackInfo.BPM == 0)
            {
                Debug.LogWarning("Track BPM 0");
            }

            _bpm = trackInfo.BPM;
            _crotchet = 60 / _bpm;

            if (trackInfo.Track == null)
            {
                Debug.LogWarning("Track AudioClip null.");
            }

            _currentClip = trackInfo.Track;

            if (trackInfo.FirstBeatOffsetInSeconds == 0)
            {
                Debug.LogWarning("Offset is 0. make sure to set it to time when the first beat happens to prevent off sync");
            }

            _offset = trackInfo.FirstBeatOffsetInSeconds;
            _startTime = Time.time; //AudioSettings.dspTime is the current time of audio system.
        } 
        #endregion
    }
}

