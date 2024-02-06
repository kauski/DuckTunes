using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuckTunes.Targets
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class CheckPoint : MonoBehaviour
    {
        public bool IsCleared => _isCleared;
        private bool _isCleared = false;

        private void Start()
        {
            if (TryGetComponent(out CircleCollider2D col)) { col.isTrigger = true; }
            else { Debug.LogWarning(gameObject.name + " missing circle collider 2D"); }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.transform.parent != null)
            {
                if (other.transform.parent.TryGetComponent(out Target data))
                {
                    _isCleared = true;
                }
            }
        }

        public void ResetCheckPoint()
        {
            _isCleared = false;
        }
    }
}
