using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace DuckTunes.UI
{
    public class VolumeControl : MonoBehaviour
    {
        [SerializeField] private AudioMixer _mixer;

        private void Awake()
        {
            if (_mixer == null)
            {
                _mixer = Resources.Load<AudioMixer>("MainMixer");
            }
        }

        public void SetMasterVolume(float sliderValue)
        {
            SetMixer();
            var logVal = Mathf.Log10(sliderValue) * 20f;
            _mixer.SetFloat("MasterVolume", logVal);
            PlayerPrefs.SetFloat("mastervolume", logVal);
            PlayerPrefs.Save();
        }

        public void SetMusicVolume(float sliderValue)
        {
            SetMixer();
            var logVal = Mathf.Log10(sliderValue) * 20f;
            _mixer.SetFloat("MusicVolume", logVal);
            PlayerPrefs.SetFloat("musicvolume", logVal);
            PlayerPrefs.Save();
        }

        public void SetEffectsVolume(float sliderValue)
        {
            SetMixer();
            var logVal = Mathf.Log10(sliderValue) * 20f;
            _mixer.SetFloat("EffectsVolume", logVal);
            PlayerPrefs.SetFloat("effectsvolume", logVal);
            PlayerPrefs.Save();
        }

        private void SetMixer()
        {
            if (_mixer == null)
            {
                _mixer = Resources.Load<AudioMixer>("MainMixer");
            }
        }

    }
}

