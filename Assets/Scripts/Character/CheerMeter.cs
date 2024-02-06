using System;
using UnityEngine;
using DuckTunes.ScriptableObjects;

namespace DuckTunes.CharacterFunctionality
{
    [Serializable]
    public class CheerMeter
    {
        private Player _player;
        private FloatReference _maxHP;
        private FloatReference _currentHP;
        private float _clampCap;

        public CheerMeter(Player player, FloatReference maxHP, FloatReference currentHp, float hpClamp)
        {
            _player = player;
            _maxHP = maxHP;
            _currentHP = currentHp;
            _clampCap = hpClamp;

            ResetHP();
        }

        public void ResetHP()
        {
            _currentHP.Value = _maxHP.Value / 2;
        }

        public void TakeDamage(float amount)
        {
            _currentHP.Value -= amount;

            if (_currentHP.Value <= 0)
            {
                _player.Die();
            }
        }

        public void HealDamage(float amount)
        {
            _currentHP.Value += amount;
            _currentHP.Value = Mathf.Clamp(_currentHP.Value, 0, _clampCap);
        }

       
    }
}