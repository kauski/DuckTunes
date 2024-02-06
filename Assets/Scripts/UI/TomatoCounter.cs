using DuckTunes.ScriptableObjects;
using TMPro;
using UnityEngine;

namespace DuckTunes.UI
{
    public class TomatoCounter : MonoBehaviour
    {
        private TextMeshProUGUI _text;
        [SerializeField] private IntReference _tomatoCount;

        private void Awake()
        {
            _text = GetComponentInChildren<TextMeshProUGUI>();
        }

        private void Start()
        {
            _text.text = _tomatoCount.Value.ToString();
        }

        public void UpdateTomatoCount()
        {
            _text.text = _tomatoCount.Value.ToString();
        }
    }   
}

