using UnityEngine;
using UnityEngine.Events;

namespace DuckTunes.Events
{
    public abstract class BaseGameEventListener<T, E, UER> : MonoBehaviour, IGameEventListener<T> where E : BaseGameEvent<T> where UER : UnityEvent<T>
    {
        [SerializeField] private E _gameEvent;
        public E GameEvent
        {
            get { return _gameEvent; }
            set { _gameEvent = value; }
        }

        [SerializeField] private UER _unityEventResponse;

        private void OnEnable()
        {
            GameEvent?.RegisterListener(this);
        }

        private void OnDisable()
        {
            GameEvent?.UnRegisterListener(this);
        }

        public void OnEventRaised(T item)
        {
            _unityEventResponse?.Invoke(item);
        }
    }
}