using DuckTunes.Systems;
using DuckTunes.Systems.Music;
using UnityEngine;

namespace DuckTunes.ScriptableObjects
{
    public class BeatMap : BeatMapFileOperations
    {
        private int _currentNote = 0;

        #region Public functions

        public BeatMap(TrackInfo track,  Conductor conductor) : base(track, conductor)
        {
            GetMidiFilePath();
        }
         
        public void Update(float songPositionInSec, bool play)
        {
            if (!DataReceived) {return;}

            LookForNotesToPlay(songPositionInSec, play);
        }

        #endregion


        #region Private functions

        private void LookForNotesToPlay(float songPositionInSec, bool play)
        {
            for (int i = _currentNote; i < _currentNote + 3; i++)
            {
                if (i >= _notes.Length) { break; }

                float startTime = _notes[i].StartTime * MsToSec;
                if (songPositionInSec > startTime && songPositionInSec < startTime + 0.5f)
                {
                    if (play)
                    {
                        PlayNote(_notes[i]);
                    }
                    _currentNote = i + 1;
                    break;
                }
            }
        } 

        private void PlayNote(SimpleNote note)
        {
            switch (note.NoteNumber)
            {
                case 60: 
                    GameManager.TargetManager.SpawnTarget(SpawnObject.Spline);
                    break;
                case 62:
                    GameManager.TargetManager.SpawnTarget(SpawnObject.Tomato);
                    break;
                default:
                    Debug.LogWarning("Note: " + note + " Not recognized");
                    break;
            }
        } 

        #endregion
    }

#region SimpleNote_Class

    public class SimpleNote
        {
            public int StartTime => _startTime;
            public int NoteNumber => _noteNumber;

            private int _startTime;
            private int _noteNumber;
            
            public SimpleNote(int startTime, int noteNumber)
            {
                _startTime = startTime;
                _noteNumber = noteNumber;
            }
        }

#endregion
}

