using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace DuckTunes.Tutorial
{
    [RequireComponent(typeof(VideoPlayer))]
    public class PlayVideo : MonoBehaviour
    {
        [SerializeField] private VideoClip _videoClip;
        [SerializeField] private float _trackVolume = 0.1f;
        [SerializeField] private string _menuSceneName;
        [SerializeField] private TextMeshProUGUI _skipTxt;
        [SerializeField] private float _textFadeSpeed;
        
        private Camera _cam;
        private VideoPlayer _videoPlayer;
        private string _savePath;

        private void Start()
        {
            if (PlayerPrefs.GetInt("show_intro", 1) == 0) {LoadScene();}
            
             _cam = Camera.main;
             _videoPlayer = GetComponent<VideoPlayer>();

             _videoPlayer.Stop();
             _videoPlayer.playOnAwake = false;
             _videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
             _videoPlayer.targetCamera = _cam;
             _videoPlayer.isLooping = false;

             _videoPlayer.source = VideoSource.VideoClip;
             _videoPlayer.clip = _videoClip;
             _videoPlayer.SetDirectAudioVolume(0, _trackVolume);
             
             _videoPlayer.errorReceived += (source, message) =>
             {
                 Debug.LogError("[VideoPlayer]: " + message);
             };
             _videoPlayer.loopPointReached += (source =>
             {
                 PlayerPrefs.SetInt("show_intro", 0);
                 source.Stop();
                 LoadScene();
             });
             _videoPlayer.started += source =>
             {
                 StartCoroutine(ShowSkipText());
             };

            _videoPlayer.Play();
        }

        private void LoadScene()
        {
            SceneManager.LoadScene(_menuSceneName);
        }

        private void Update()
        {
            if (Touch.activeTouches.Count > 0)
            {
                PlayerPrefs.SetInt("show_intro", 0);
                SceneManager.LoadScene(_menuSceneName); 
            }
        }

        private IEnumerator ShowSkipText()
        {
            yield return new WaitForSeconds(1f);

            while (_skipTxt.color.a < 1f)
            {
                var c = _skipTxt.color;
                c.a += _textFadeSpeed * 0.1f * Time.unscaledDeltaTime;
                _skipTxt.color = c;
                yield return null;
            }

            yield return new WaitForSeconds(1f);

            while (_skipTxt.color.a > 0)
            {
                var c = _skipTxt.color;
                c.a -= _textFadeSpeed * 0.1f * Time.unscaledDeltaTime;
                _skipTxt.color = c;
                yield return null; 
            }
        }

        private void OnEnable()
        {
            EnhancedTouchSupport.Enable();
        }

        private void OnDisable()
        {
            EnhancedTouchSupport.Disable();
        }
    }   
}

