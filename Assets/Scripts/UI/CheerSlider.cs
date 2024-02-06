using DuckTunes.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace DuckTunes.UI
{
    public class CheerSlider : MonoBehaviour
    {
        private Slider _slider;
        [SerializeField] private FloatReference _currentHP;
        [SerializeField] private FloatReference _maxHP;

        [SerializeField] private SpriteRenderer _faceSprite;
        [SerializeField] private ParticleSystem _faceParticles;
        [SerializeField] private Sprite[] _faceSprites;

        private RectTransform _rectTransform;
        [SerializeField] private RectTransform _faceRectTransform;
        private float _xCoord;
        private float _range;

        private void Awake()
        {
            _slider = GetComponent<Slider>();
            _slider.value = (int)_currentHP.Value / _maxHP.Value;
            _rectTransform = gameObject.GetComponent<RectTransform>();
            _faceSprite.sprite = _faceSprites[0];

            _faceParticles.Stop();
            _faceParticles.gameObject.SetActive(false);
            
            var range = _rectTransform.rect.width;
            var _imgSize = _faceSprite.GetComponent<RectTransform>().rect.width * 0.5f;
            range -= _imgSize * 2f;
            _xCoord = 0.5f * range;
            _range = 2f * _xCoord;
            
        }

        public void UpdateSlider()
        {
            var val = (int)_currentHP.Value / _maxHP.Value;
            Mathf.Clamp(val, 0, _maxHP.Value);

            _slider.value = val;

            SelectCorrectFace(val);
            PositionFace(val);
        }

        private void PositionFace(float val)
        {
            float pos;
            var rangePercent = _range * val;

            var halfDist = _range * 0.5f;
            var diff = halfDist - rangePercent;
            pos = -diff;
            pos = Mathf.Clamp(pos, -_xCoord, _xCoord);

            _faceRectTransform.anchoredPosition = new Vector3(pos, 0f, -100f);
        }

        private void SelectCorrectFace(float val)
        {
            if (val > 0.9f)
            {
                _faceSprite.sprite = _faceSprites[4];
                if (_faceParticles.emission.GetBurst(0).maxCount != 4)
                {
                    var particle = _faceParticles.emission.GetBurst(0);
                    particle.count = 4;
                    _faceParticles.emission.SetBurst(0, particle); 
                }
                _faceParticles.Play();
            }
            else if (val > 0.8f)
            {
                _faceSprite.sprite = _faceSprites[3];
                _faceParticles.gameObject.SetActive(true);
                if (_faceParticles.emission.GetBurst(0).maxCount != 2)
                {
                    var particle = _faceParticles.emission.GetBurst(0);
                    particle.count = 2;
                    _faceParticles.emission.SetBurst(0, particle); 
                }
                _faceParticles.Play();
            }
            else if (val > 0.6f)
            {
                _faceSprite.sprite = _faceSprites[2];
                _faceParticles.Stop();
            }
            else if (val > 0.4f)
            {
                _faceSprite.sprite = _faceSprites[1];
                _faceParticles.Stop();
            }
            else if (val < 0.2f)
            {
                _faceSprite.sprite = _faceSprites[0];
                _faceParticles.Stop();
            }
        }
    }
}