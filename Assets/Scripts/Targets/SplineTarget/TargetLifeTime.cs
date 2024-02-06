using DuckTunes.Systems;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuckTunes.Targets
{
    public class TargetLifeTime : MonoBehaviour
    {
        private Target _target;

        private float _duration;
        private float _timer = 0;

        public void OnSpawn()
        {
            _timer = _duration;
            StartCoroutine(LifeTime());
        }

        public void SetDuration(float duration)
        {
            _duration = duration;
        }

        private void Awake()
        {
            _target = GetComponent<Target>();
        }

        private IEnumerator LifeTime()
        {
            while (_timer > 0)
            {
                if (GameManager.Instance.Paused) { yield return new WaitWhile(() =>
                {
                    if (GameManager.Instance.Paused)
                    {
                        return true;
                    }

                    return false;
                }); }

                _timer -= Time.deltaTime;

                yield return null;
            }
            _target.DisableTarget();
        }
    }
}

