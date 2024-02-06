using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using DuckTunes.Utility;
using DuckTunes.Events;
using DuckTunes.Systems.Interaction;

namespace DuckTunes.Targets
{
    public class SplineFunctionality : MonoBehaviour
    {
        SplineTarget _target;

        [SerializeField] private float _failThreshold = 0.5f;
        [SerializeField] private float _successThreshodld = 0.4f;
        [SerializeField] private float _failMaxDamage = 5f;
        [SerializeField] private float _successHealAmount = 5f;
        [SerializeField] private float _pointSearchThreshold = 0.5f;

        [SerializeField] private UnityFloatEvent _onSuccess;
        [SerializeField] private UnityFloatEvent _onFail;
        [SerializeField] private UnityVoidEvent _onSplineCleared;

        [Header("Sounds")] 
        [SerializeField] private AudioClip _failSound;
        [SerializeField] private AudioClip[] _successSounds;
        [SerializeField] private AudioClip _slideSound;
        private float _startPitch;

        [HideInInspector] public bool IsFinished = false;

        private Vector3 _startCirclePos;
        private GameObject _startCircle;
        private Vector3 _closestPoint = Vector3.zero;
        private Vector3 _lastPoint = Vector3.zero;
        private int _currentIndex = 0;
        private Vector2 _touchPosInWorld;
        private float Startimer;
        
        private void Awake()
        {
            _target = GetComponent<SplineTarget>();
            _startCircle = gameObject.GetComponentInChildren<Interactable>().gameObject;
            _startCirclePos = _startCircle.transform.position;
        }

        private void Start()
        {
            _lastPoint = new Vector3(_target.Data.Points[^1].x, _target.Data.Points[^1].y, _target._startCircleOffset);
            _currentIndex = 0;
        }

        public void Tick()
        {
            if (!_target.IsActive || IsFinished) { return; }

            _startCirclePos = _startCircle.transform.position;
            var deltaStart = _startCirclePos;
            _touchPosInWorld = _target.CurrentTouchPos;
            SplineUtil.GetNearestPoint(_target.Data.Points, _touchPosInWorld, out _closestPoint, 124);

            if (!_target._source.isPlaying)
            {
                _target._source.clip = _slideSound;
                _target._source.Play();
            }

            MoveCircleToClosestPoint();

            var delta = (_startCirclePos - deltaStart).magnitude;
            delta *= 30f;
            delta = Mathf.Clamp(delta, -1, 2);
            _target._source.pitch = delta;

            Debug.DrawLine(_startCirclePos, _startCirclePos + Vector3.up, Color.blue);
            Debug.DrawLine(_lastPoint, _lastPoint + Vector3.up, Color.red);

            CheckDistances(_touchPosInWorld);
        }

        private void Success()
        {
            if (!IsFinished)
            {
                _target._source.Stop();
                _target._source.pitch = 1f;
                _target._source.PlayOneShot(_successSounds[Random.Range(0, _successSounds.Length)]);
                _onSuccess?.Invoke(_successHealAmount);
                _onSplineCleared?.Invoke(new Void());
                IsFinished = true;
                _target.DisableTarget();
            }
        }

        private void Fail()
        {
            if (!IsFinished)
            {
                _target._source.Stop();
                _target._source.pitch = 1f;
                _target._source.PlayOneShot(_failSound);
                float dmg = CalculateDamage(CheckCheckPointProgress(), _target._checkPoints.Length);
                _onFail?.Invoke(dmg);
                IsFinished = true;
                _target.DisableTarget();
            }
        }

        public void Disable()
        {
            if (!IsFinished)
            {
                Fail();
            }

            SetUpForNextSpawn();
        }
        
        private void MoveCircleToClosestPoint()
        {
            for (int i = _currentIndex; i < _currentIndex + 40; i++)
            {
                if (i >= _target.Data.Points.Length)
                {
                    break;
                }
                else if (Vector3.Distance(_target.Data.Points[i], _closestPoint) < _pointSearchThreshold)
                {
                    _startCirclePos = new Vector3(_closestPoint.x, _closestPoint.y, _target._startCircleOffset);
                    _startCircle.transform.position = _startCirclePos;
                    _currentIndex = i;
                    break;
                }
            }
        }

        private void CheckDistances(Vector3 worldPos)
        {
            if (Vector3.Distance(_startCirclePos, _lastPoint) < _successThreshodld)
            {
                Success();
            }
            else if (Vector3.Distance(_closestPoint, worldPos) > _failThreshold)
            {
                Fail();
            }
        }

        private int CheckCheckPointProgress()
        {
            int successCount = 0;
            foreach (GameObject checkPoint in _target._checkPoints)
            {
                if (checkPoint.GetComponent<CheckPoint>().IsCleared == true)
                {
                    successCount++;
                }
            }

            return successCount;
        }

        private void SetUpForNextSpawn()
        {
            _closestPoint = Vector3.zero;
            _startCirclePos = _startCircle.transform.position;
            _lastPoint = new Vector3(_target.Data.Points[^1].x, _target.Data.Points[^1].y, _target._startCircleOffset);
            _currentIndex = 0;
        }

        private float CalculateDamage(int checkPointsCleared, int totalCheckPoints)
        {
            return (float)(totalCheckPoints - checkPointsCleared) / totalCheckPoints * _failMaxDamage;
        }
    }
}