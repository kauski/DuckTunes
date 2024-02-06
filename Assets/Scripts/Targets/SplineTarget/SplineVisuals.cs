using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuckTunes.Utility;
using System;

namespace DuckTunes.Targets
{
    public class SplineVisuals : MonoBehaviour
    {
        [SerializeField] private GameObject _checkPointPrefab;
        [SerializeField] private ParticleSystem[] _disappearingParticles;
        [SerializeField] private ParticleSystem[] _stayingParticles;
        [SerializeField] private ParticleSystemRenderer _glowParticle;

        private LineRenderer _renderer;

        private GameObject[] _circles;
        private TargetData _data;
        private float _longestParticleDuration;

        private void Awake()
        {
            _renderer = Utilities.TryGetComponentTFromGameObject<LineRenderer>(this.gameObject.transform.transform);
        }

        private void Start()
        {
            _longestParticleDuration = GetLongestParticleTime();
        }

        public void SetLineRenderer(Vector3[] points)
        {
            _renderer.positionCount = points.Length;
            _renderer.SetPositions(points);
        }

        public void SetSortOrder(int sortOrder)
        {
            var sprites = GetComponentsInChildren<SpriteRenderer>();
            var lines = GetComponentsInChildren<LineRenderer>();
            var trails = GetComponentsInChildren<TrailRenderer>();

            foreach (var s in sprites)
            {
                s.sortingOrder = sortOrder;
            }

            foreach (var l in lines)
            {
                l.sortingOrder = sortOrder;
            }

            foreach (var t in trails)
            {
                t.sortingOrder = sortOrder;
            }

            _glowParticle.sortingOrder = sortOrder;
        }

        public void SetEndCirclePosAndRot(float posOffset, float angleOffset, bool debug = false)
        {
            Vector3 dir = GetDirectionFromSplineEnd(posOffset);
            GetAngleFromDirAndApplyOffset(angleOffset, dir);
        }

        public void ResetVisuals()
        {
            ChangeCircleSprite(true);
            ChangeCircleTrail(true);
            ChangeDisappearingParticles(true);
            _renderer.enabled = true;
        }

        public void DisableVisuals()
        {
            ChangeCircleSprite(false);
            ChangeCircleTrail(false);
            ChangeDisappearingParticles(false);
            _renderer.enabled = false;
        }
        
        private void ChangeDisappearingParticles(bool isActive)
        {
            foreach (var particle in _disappearingParticles)
            {
                if (isActive)
                {
                    particle.gameObject.SetActive(true); 
                }
                else
                {
                    particle.gameObject.SetActive(false);
                }
            }
        }

        private void ChangeCircleTrail(bool isActive)
        {
            foreach (GameObject circle in _circles)
            {
                var trails = circle.GetComponentsInChildren<TrailRenderer>();
                foreach (TrailRenderer trail in trails)
                {
                    trail.enabled = isActive;
                }
            }
        }

        private void ChangeCircleSprite(bool isActive)
        {
            foreach (GameObject circle in _circles)
            {
                var sprites = GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer sprite in sprites)
                {
                    sprite.enabled = isActive;
                }
            }
        }

        public void SetData(TargetData data, GameObject[] circles)
        {
            _data = data;
            _circles = circles;
        }

        public float GetLongestParticleSystemDuration()
        {
            return _longestParticleDuration;
        }

        private float GetLongestParticleTime()
        {
            float dur = -1f;

            foreach (ParticleSystem particle in _stayingParticles)
            {
                dur = particle.main.duration > dur ? particle.main.duration : dur;
            }

            return dur;
        }

        private void GetAngleFromDirAndApplyOffset(float angleOffset, Vector3 dir)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle + angleOffset, Vector3.forward);
            _circles[1].transform.rotation = Quaternion.RotateTowards(_circles[1].transform.rotation, q, 360f);
        }

        private Vector3 GetDirectionFromSplineEnd(float posOffset)
        {
            Vector3 dir = _data.Points[^1] - _data.Points[^2];
            _circles[1].transform.position = (_data.Points[^1] + dir * posOffset);
            return dir;
        }

    }
}

