using UnityEngine;

namespace DuckTunes.Systems.Interaction
{
    public interface ISelector
    {
        public void Check(Ray ray);
        public Transform[] GetSelection();
    }
}

