using UnityEngine;

namespace DuckTunes.Events
{
    [CreateAssetMenu(fileName = "New void Event", menuName = "Game Event/Void Event")]
    public class VoidEvent : BaseGameEvent<Void>
    {
        public void Raise() => Raise(new Void());
    }
}