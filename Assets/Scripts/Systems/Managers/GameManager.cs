using System;
using UnityEngine;
using DuckTunes.Events;
using DuckTunes.Systems.Interaction;
using DuckTunes.Systems.InputHandling;
using DuckTunes.Targets;
using DuckTunes.Systems.Music;
using DuckTunes.Systems.ObjectPooling;
using DuckTunes.UI;
using Void = DuckTunes.Events.Void;

namespace DuckTunes.Systems
{
    [DefaultExecutionOrder(-101)]
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance => _instance;
        private static GameManager _instance;

        public bool Paused => _paused;
        private bool _paused;

        [SerializeField] private UnityVoidEvent OnPause;
        [SerializeField] private UnityVoidEvent OnUnPause;


        public static EventManager EventManager;
        public static SelectionManager SelectionManager;
        public static InputManager InputManager;
        public static TargetManager TargetManager;
        public static ObjectPoolManager PoolManager;
        public static Conductor Conductor;
        public static VolumeControl VolumeControl;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(this);
            }

            EventManager = FindObjectOfType<EventManager>();
            SelectionManager = FindObjectOfType<SelectionManager>();
            InputManager = FindObjectOfType<InputManager>();
            TargetManager = FindObjectOfType<TargetManager>();
            PoolManager = FindObjectOfType<ObjectPoolManager>();
            Conductor = FindObjectOfType<Conductor>();
            VolumeControl = FindObjectOfType<VolumeControl>();

            SetVolume();
            UnPauseGame();
        }

        private void SetVolume()
        {
            var master = PlayerPrefs.GetFloat("mastervolume", 1f);
            var music = PlayerPrefs.GetFloat("musicvolume", 1f);
            var effects = PlayerPrefs.GetFloat("effectsvolume", 1f);

            if (float.IsNaN(master) || master == Single.NegativeInfinity)
            {
                master = 1f;
            }
            if (float.IsNaN(music) || music == Single.NegativeInfinity)
            {
                music = 1f;
            }
            if (float.IsNaN(effects) || effects == Single.NegativeInfinity)
            {
                effects = 1f;
            }

            VolumeControl.SetMasterVolume(master);
            VolumeControl.SetMusicVolume(music);
            VolumeControl.SetEffectsVolume(effects);
        }

        public void PauseGame()
        {
            Time.timeScale = 0;
            _paused = true;
            OnPause?.Invoke(new Void());
        }

        public void UnPauseGame()
        {
            Time.timeScale = 1f;
            _paused = false;
            OnUnPause?.Invoke(new Void());
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                PauseGame();
            }
            else
            {
                UnPauseGame();
            }
        }
    }
}

