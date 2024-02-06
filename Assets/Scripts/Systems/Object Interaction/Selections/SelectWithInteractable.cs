using DuckTunes.Targets;
using UnityEngine;
using DuckTunes.Utility;
using System.Collections.Generic;

namespace DuckTunes.Systems.Interaction
{
    public class SelectWithInteractable : MonoBehaviour, ISelector
    {
        private List<Transform> _selection = new List<Transform>();
        private Camera _cam;
        private Interactable _target;

        public void Check(Ray ray)
        {
            if (_cam == null) { _cam = Camera.main; }

            Transform selection = null;
            _target = null;
            _selection.Clear();

            var hits = Physics2D.RaycastAll(ray.origin, ray.direction, _cam.farClipPlane);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider != null)
                {
                    selection = hits[i].transform;
                    _target = Utilities.TryGetComponentTFromGameObject<Interactable>(selection);
                    if (_target != null)
                    {
                        _selection.Add(selection);
                    }
                }
            }
        }

        public Transform[] GetSelection()
        {
            return _selection.ToArray();
        }
    }
}