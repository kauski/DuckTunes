using DuckTunes.Systems;
using DuckTunes.Utility;
using System.Collections;
using DuckTunes.Events;
using UnityEngine;
using DG.Tweening;

namespace DuckTunes.Targets
{
    public class TomatoTarget : Target
    {
        UpdateUI UIUpdate;
        public GameObject Target;
        private bool _GainTomato;

        [SerializeField] private float _maxSize;
        [SerializeField] private float _moveSpeed = 1f;
        
        private TrailRenderer _trail;
        private AudioSource _source;
        private float _trailTime;
        private Camera _cam;
        private bool hasBeenDisabled = false;

        [SerializeField] private Sprite _tomatoNormalSprite;
        [SerializeField] private Sprite _tomatoSplashedSprite;
        [SerializeField] private const string TomatoMovePointName = "TomatoMovePoint";

        private SpriteRenderer _renderer;

        [SerializeField] private float _successHealAmount = 1f;
        [SerializeField] private float _failDamageAmount = 1f;
        
        [SerializeField] private UnityFloatEvent _onSuccess;
        [SerializeField] private UnityFloatEvent _onFail;
        [SerializeField] private UnityVoidEvent _onTomatoGained;
        
        [Header("Audio")]
        [SerializeField] private AudioClip _splashAudio;
        [SerializeField] private AudioClip _counterAudio;
        [SerializeField] private AudioClip _failAudio;

        private void Awake()
        {
            _cam = Camera.main;
            Target = GameObject.Find(TomatoMovePointName);
            _renderer = GetComponent<SpriteRenderer>();
            _source = GetComponent<AudioSource>();
            _trail = GetComponent<TrailRenderer>();
            _trailTime = _trail.time;
        }

        //run once when creating the pool
        public override void Spawn()
        {
            _GainTomato = false;
        }

        public override void Tap()
        {
            _GainTomato = true;
            SetVisuals();
            _source.PlayOneShot(_splashAudio);
            DisableTarget();
        }

        public override void Tick()
        {
            
        }
        
        public override void OnObjectReuse()
        {
            StartCoroutine(ResetTrail());
            SetPositionAndScale();

            _GainTomato = false;
            hasBeenDisabled = false;

            SetVisuals();
            StartCoroutine(LifeTime(Duration));
        }

        private IEnumerator ResetTrail()
        {
            _trail.time = -1;
            yield return new WaitForSeconds(0.1f);
            _trail.time = _trailTime;
        }

        public override void DisableTarget()
        {
            if (!hasBeenDisabled)
            {
                hasBeenDisabled = true;
                StartCoroutine(AfterDisable());
            }
        }

        public override void SetVisuals()
        {
            if (!_GainTomato)
            {
                if (_renderer.sprite != _tomatoNormalSprite)
                {
                    _renderer.sprite = _tomatoNormalSprite;
                }
            }
            else
            {
                if (_renderer.sprite != _tomatoSplashedSprite)
                {
                    _renderer.sprite = _tomatoSplashedSprite;
                }
            }
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
        }

        private IEnumerator LifeTime(float duration)
        {
            if (GameManager.Instance.Paused)
            {
                DOTween.PauseAll();

                yield return new WaitWhile(() =>
                {
                    if (GameManager.Instance.Paused)
                    {
                        return true;
                    }

                    DOTween.PlayAll();
                    return false;
                });
            }

            //setup
            gameObject.transform.DOScale(_maxSize, 1f);
            float timer = duration;
            
            // loop
            while (timer > 0)
            {
                if (GameManager.Instance.Paused)
                {
                    DOTween.PauseAll();

                    yield return new WaitWhile(() =>
                    {
                        if (GameManager.Instance.Paused)
                        {
                            return true;
                        }

                        DOTween.PlayAll();
                        return false;
                    });
                }

                if (_GainTomato) {break;}
                timer -= Time.deltaTime;

                yield return null;
            }

            var dur = 1f;
            var timer2 = dur;
            gameObject.transform.DOScale(0f, dur);
            while (timer2 > 0)
            {
                if (GameManager.Instance.Paused)
                {
                    DOTween.PauseAll();

                    yield return new WaitWhile(() =>
                    {
                        if (GameManager.Instance.Paused)
                        {
                            return true;
                        }

                        DOTween.PlayAll();
                        return false;
                    });
                }

                timer2 -= Time.deltaTime;

                yield return null;
            }
            
            DisableTarget();
        }

        private IEnumerator AfterDisable()
        {
            bool inScore = false;
            float t = 0;

            if (_GainTomato)
            { 
                while (!inScore)
                {
                    t += Time.deltaTime * _moveSpeed;
                    transform.position = Vector3.Lerp(transform.position, Target.transform.position, t);
                
                    inScore = Vector3.Distance(transform.position, Target.transform.position) < 0.1f;
                
                    yield return null;
                }

                if (inScore)
                {
                    _source.PlayOneShot(_counterAudio);
                    _onSuccess?.Invoke(_successHealAmount);
                    _onTomatoGained?.Invoke(new Void());

                    while (_source.isPlaying)
                    {
                        yield return null;
                    }
                    
                    GameManager.TargetManager.RemoveTargetFromActiveList(SpawnObject.Tomato, this.gameObject);
                    Destroy();
                } 
            }
            else
            {
                _source.PlayOneShot(_failAudio);
                while (_source.isPlaying)
                {
                    yield return null;
                }

                _onFail?.Invoke(_failDamageAmount);
                GameManager.TargetManager.RemoveTargetFromActiveList(SpawnObject.Tomato, this.gameObject);
                Destroy(); 
            }

        }
        
        private void SetPositionAndScale()
        {
            transform.localScale = new Vector3(0, 0, 0);
            transform.position = Utilities.GetRandomPointInScreen(_cam, 2, 1, 1);
        }
    }
}
