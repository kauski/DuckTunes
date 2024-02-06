namespace DuckTunes.Events
{
    public interface IGameEventListener<T>
    {
        public void OnEventRaised(T item);
    }
}
