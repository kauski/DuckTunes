using System;
using System.Collections;
using System.Collections.Generic;
using DuckTunes.ScriptableObjects;
using UnityEngine;

namespace DuckTunes.CharacterFunctionality
{
    [Serializable]
    public class PlayerStats
    {
        [field: SerializeField]
        private int Tomatoes
        {
            get;
            set;
        }
        private IntReference TomatoCount;
        [SerializeField] private int Hits;
        [SerializeField] private int Misses;
        [SerializeField] private int Combo;
        [SerializeField] private int HigestCombo;

        [field: SerializeField]
        private int Score
        {
            get;
            set;
        }
        private IntReference _score;
        private int _tomatoScore = 15;
        private int _splineScore = 65;
        private int _baseDamage = 50;

        public PlayerStats(IntReference tomatoCount, IntReference score)
        {
            TomatoCount = tomatoCount;
            Tomatoes = TomatoCount.Value;
            Hits = 0;
            Misses = 0;
            _score = score;
            _score.Value = 0;
        }

        public void Hit()
        {
            Hits++;
            Combo++;
        }

        public void Miss()
        {
            Misses++;
            if (Combo > HigestCombo)
            {
                HigestCombo = Combo;
            }

            var amount = _baseDamage + Misses * Misses - Combo;
            CalculateScore(-amount);
            Combo = 0;
        }

        public void GainTomato()
        {
            TomatoCount.Value++;
            Tomatoes = TomatoCount.Value;
        }

        public void Save()
        {
            PlayerPrefs.SetInt("tomatoes", TomatoCount.Value);
            PlayerPrefs.Save();
        }
        
        public void AddTomatoScore()
        {
            var amount = (int)(_tomatoScore + Hits + Combo * Combo);
            CalculateScore(amount);
        }

        public void AddSplineScore()
        {
            var amount = (int)(_splineScore + Hits + Combo * Combo); 
            CalculateScore(amount);
        }
        
        private void CalculateScore(int amount)
        {
            _score.Value += amount;
            if (_score.Value < 0) { _score.Value = 0; }
            Score = _score.Value;
        }
    }
}
