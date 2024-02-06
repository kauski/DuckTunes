using UnityEngine;
using DuckTunes.Utility;

namespace DuckTunes.Systems.Interaction
{
    public class SelectionManager : MonoBehaviour
    {
        private ISelectionResponse _response;
        private ISelector _selector;

        [SerializeField] private Transform[] _currentSelection;

        private void Awake()
        {
            _selector = GetComponent<ISelector>();
            _response = GetComponent<ISelectionResponse>();
        }

        public Transform[] Select(Vector3 touchPos, bool useResponse = true)
        {
            if (_currentSelection != null) { _response.OnDeselect(_currentSelection); }

            _selector.Check(Utilities.CreateRay(touchPos));
            _currentSelection = _selector.GetSelection();

            if (_currentSelection != null && useResponse) { _response.OnSelect(_currentSelection); }
            
            return _currentSelection;
        }
    }
}
