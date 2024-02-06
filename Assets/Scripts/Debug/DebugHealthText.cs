using DuckTunes.ScriptableObjects;
using UnityEngine;
using TMPro;

namespace DuckTunes.DebugAndTesting
{
    public class DebugHealthText : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _playerText, _enemyText;
        [SerializeField] FloatReference _currentHP, _maxHP;

        private void Update()
        {
            var val = (int)_currentHP.Value / _maxHP.Value;
            Mathf.Clamp(val, 0, _maxHP.Value);
            val *= 100f;

            _playerText.text = "Player: " + val.ToString();
            _enemyText.text = (_maxHP.Value - val).ToString() + " :Enemy";
        }
    }
}