using UnityEngine;

namespace DuckTunes.Systems.Interaction
{
    public interface ISelectionResponse
    {
        public void OnSelect(Transform[] transforms);
        public void OnDeselect(Transform[] transforms);
    }
}
