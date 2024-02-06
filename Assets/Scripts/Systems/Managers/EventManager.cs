using System;
using DuckTunes.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DuckTunes.ScriptableObjects;

namespace DuckTunes.Events
{
    public class EventManager : MonoBehaviour
    {
        [SerializeField] private FloatReference _playerCurrentHP;

        public UnityVoidEvent OnStart;
        public UnityVoidEvent OnVictory;
        public UnityVoidEvent OnDefeat;
        public bool Paused;

        private AudioSource _source;
        [SerializeField] private AudioClip _victoryAudio;
        [SerializeField] private AudioClip _deathAudio;
        bool tempPause;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
        }

        private void Start()
        {
            OnStart?.Invoke(new Void());
        }

        private void Update()
        {
            if (Paused != tempPause)
            {
                if (Paused)
                {
                    GameManager.Instance.PauseGame();
                }
                else
                {
                    GameManager.Instance.UnPauseGame();
                }

                tempPause = Paused;
            }
        }


#region GameEvents
        public void HandleOnGameEnd()
        {
            OnDefeat?.Invoke(new Void());
            _source.PlayOneShot(_deathAudio);
            GameManager.TargetManager.RemoveAllActiveTargets();
            GameManager.Conductor.StopSpawning();
            GameManager.Instance.PauseGame();
        }

        public void HandleOnSongEnd()
        {
            if (_playerCurrentHP.Value > 79f)
            {
                OnVictory?.Invoke(new Void());
                _source.PlayOneShot(_victoryAudio);
            }
            else
            {
                HandleOnGameEnd();
            }
        }

#endregion
    }
}

