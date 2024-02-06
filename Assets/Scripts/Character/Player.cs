using System;
using DuckTunes.Events;
using DuckTunes.ScriptableObjects;
using UnityEngine;
using Void = DuckTunes.Events.Void;

namespace DuckTunes.CharacterFunctionality
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerStats _stats;
        [SerializeField] private CheerMeter _cheerMeter;
        [SerializeField, Range(100, 150)] private float _maxHpClamp;

        [SerializeField] private FloatReference _maxHP;
        [SerializeField] private FloatReference _currentHP;
        [SerializeField] private IntReference _tomatoCount;
        [SerializeField] private IntReference _score;

        [SerializeField] private bool _clearStats;

        [SerializeField] private UnityVoidEvent OnGameEnd;
        [SerializeField] private UnityVoidEvent _onHpChanged;
        private bool _hasInvoked;

        private void Awake()
        {
            _tomatoCount.Value = PlayerPrefs.GetInt("tomatoes", 0);
            _cheerMeter = new CheerMeter(this, _maxHP, _currentHP, _maxHpClamp);
            _stats = new PlayerStats(_tomatoCount, _score);
        }

        private void Start()
        {
            if (_clearStats)
            {
                PlayerPrefs.DeleteAll();
            }
        }

        public void Heal(float healAmount)
        {
            _cheerMeter.HealDamage(healAmount);
            _stats.Hit();
            _onHpChanged?.Invoke(new Void());
        }

        public void Damage(float damageAmount)
        {
            _cheerMeter.TakeDamage(damageAmount);
            _stats.Miss();
            _onHpChanged?.Invoke(new Void());
        }

        public void ResetHP()
        {
            _cheerMeter.ResetHP();
            _onHpChanged?.Invoke(new Void());
        }
        
        public void Die()
        {
            if (!_hasInvoked)
            {
                OnGameEnd?.Invoke(new Void());
                _hasInvoked = true;
            } 
        }

        public void TomatoGained()
        {
            _stats.GainTomato();
            _stats.AddTomatoScore();
        }

        public void SplineGained()
        {
            _stats.AddSplineScore();
        }

        public void Save()
        {
            _stats.Save();
        }
    }
}
