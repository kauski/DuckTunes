using System.Collections.Generic;
using UnityEngine;

namespace DuckTunes.Events
{
    public class BaseGameEvent<T> : ScriptableObject
    {
        private readonly List<IGameEventListener<T>> _listeners = new List<IGameEventListener<T>>();

        public virtual void Raise(T item)
        {
            for (int i = _listeners.Count - 1; i >= 0; i--)
            {
                _listeners[i].OnEventRaised(item);
            }
        }

        public virtual void RegisterListener(IGameEventListener<T> listener)
        {
            if (!_listeners.Contains(listener))
            {
                _listeners.Add(listener);
            }
        }

        public virtual void UnRegisterListener(IGameEventListener<T> listener)
        {
            if (_listeners.Contains(listener))
            {
                _listeners.Remove(listener);
            }
        }
    }
}

