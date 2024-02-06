using System.Collections;
using System.Threading;
using UnityEngine;

namespace DuckTunes.Systems
{
    public class FpsHandler : MonoBehaviour
    {
        [Header("FPS settings")]
        [SerializeField] private int _maxFps = 240;
        [SerializeField] private float _targetFrameRate = 120f;

        private float _currentFrameTime;

        private void Awake()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = _maxFps;
            _currentFrameTime = Time.realtimeSinceStartup;
            StartCoroutine(WaitForEndOfFrame());
        }

        private IEnumerator WaitForEndOfFrame()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();
                _currentFrameTime += 1f / _targetFrameRate;
                var t = Time.realtimeSinceStartup;
                var sleepTime = _currentFrameTime - t - 0.01f;
                if (sleepTime > 0)
                {
                    Thread.Sleep((int)(sleepTime * 1000));
                }

                while (t < _currentFrameTime)
                {
                    t = Time.realtimeSinceStartup;
                }
            }
        }
    }

}