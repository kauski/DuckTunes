using System;
using System.Collections;
using System.Runtime.CompilerServices;
using DuckTunes.Systems;
using DuckTunes.Utility;
using Unity.Mathematics;
using UnityEngine;

namespace DuckTunes.Targets
{
    [RequireComponent(typeof(SplineMovement), typeof(SplineFunctionality))]
    [RequireComponent(typeof(TargetLifeTime), typeof(SplineVisuals))]
     public class SplineTarget : Target
    {
        public TargetData Data;
        
        [SerializeField] private SplineMovement _movement;
        [SerializeField] private SplineFunctionality _functionality;
        [SerializeField] private SplineVisuals _visuals;
        [SerializeField] private TargetLifeTime _lifeTime;

        [HideInInspector] public GameObject[] _checkPoints;
        [HideInInspector] public GameObject[] _circleGOs;
        public float _startCircleOffset = -1f;
        [HideInInspector] public GameObject _checkPointPrefab;

        [SerializeField] private bool _debugEndPos = false;
        [SerializeField] private float _endCircleRotationOffset = 0f;
        [SerializeField] private float _endCirclePositionOffset = 0f;

        [HideInInspector] public AudioSource _source;

        private void Awake()
        {
            _movement = GetComponent<SplineMovement>();
            _functionality = GetComponent<SplineFunctionality>();
            _lifeTime = GetComponent<TargetLifeTime>();
            _visuals = GetComponent<SplineVisuals>();
            _source = GetComponent<AudioSource>();
        }

        //Run once when creating the pool
        public override void Spawn()
        {
            if (_hasBeenInitialized == false) { Initialize(); }
            _hasBeenDisabled = false;
            SetVisuals();
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (_debugEndPos)
            {
                _visuals.SetEndCirclePosAndRot(_endCirclePositionOffset, _endCircleRotationOffset);
            }
        }
#endif

        public override void Tap()
        {
            
        }

        public override void Tick()
        {
            _movement.Move();
            _functionality.Tick();
        }

        public override void OnObjectReuse()
        {
            if (!_hasBeenInitialized) { Initialize(); }
            SetCircles();
            ResetCheckPoints();
            _lifeTime.SetDuration(Duration);
            _lifeTime.OnSpawn();
            _visuals.ResetVisuals();
            _functionality.IsFinished = false;
            _hasBeenDisabled = false;
        }

        public override void DisableTarget()
        {
            if (IsActive && !_hasBeenDisabled)
            {
                _hasBeenDisabled = true;
                _functionality.Disable();
                var wait = _visuals.GetLongestParticleSystemDuration();
                StartCoroutine(WaitForParticlesAndDisable(wait));
            }
        }

        private IEnumerator WaitForParticlesAndDisable(float waitDur)
        {
            _visuals.DisableVisuals();
            yield return new WaitForSeconds(waitDur);

            while (_source.isPlaying)
            {
                yield return null;
            }
            GameManager.TargetManager.RemoveTargetFromActiveList(SpawnObject.Spline, this.gameObject);
            Destroy();
        }

        public override void SetVisuals()
        {
            _visuals.SetData(Data, _circleGOs);
            _visuals.SetLineRenderer(Data.Points);
            _visuals.SetEndCirclePosAndRot(_endCirclePositionOffset, _endCircleRotationOffset);
        }

        public void SetSortOrder(int sortOrder)
        {
            _visuals.SetSortOrder(sortOrder);
        }

        private void Initialize()
        {
            _checkPointPrefab = Resources.Load("CheckPoint") as GameObject;
            CreateCheckPoints();
            SetCircles();
            _hasBeenInitialized = true;
        }

        private void SetCircles()
        {
            _circleGOs = Utilities.FindChildWithTag(this.gameObject, "Circle");
            _circleGOs[0].transform.position = new Vector3(Data.Points[0].x, Data.Points[0].y, _startCircleOffset);
        }

        private void CreateCheckPoints()
        {
            _checkPoints = new GameObject[Data.Anchors.Length];
            for (int i = 0; i < Data.Anchors.Length; i++)
            {
                _checkPoints[i] = Instantiate(_checkPointPrefab, gameObject.transform, false);
                _checkPoints[i].transform.position = Data.Anchors[i].With(z: 0);
            }
        }

        private void ResetCheckPoints()
        {
            for (int i = 0; i < _checkPoints.Length; i++)
            {
                _checkPoints[i].GetComponent<CheckPoint>().ResetCheckPoint();
            }
        }
     }
}
