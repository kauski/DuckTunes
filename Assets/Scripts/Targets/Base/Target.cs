using UnityEngine;
using DuckTunes.Systems.ObjectPooling;

namespace DuckTunes.Targets
{
    public abstract class Target : PoolObject
    {
        [HideInInspector] public bool _hasBeenInitialized = false;
        [HideInInspector] public bool _hasBeenDisabled = false;
        public bool IsActive => gameObject.activeSelf;
        public Vector2 CurrentTouchPos { get; set; }
        public float Duration;

        public abstract void Spawn();
        public abstract void Tap();
        public abstract void Tick();
        public abstract void SetVisuals();
        public abstract override void OnObjectReuse();
        public abstract void DisableTarget();
    }
}


