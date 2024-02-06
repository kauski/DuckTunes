using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuckTunes.Utility;
using DuckTunes.Targets;

namespace DuckTunes.Systems.Interaction
{
    public class TryActivateTargetResponse : MonoBehaviour, ISelectionResponse
    {
        public void OnSelect(Transform[] transform)
        {
            for (int i = 0; i < transform.Length; i++)
            {
                Transform parent = transform[i].parent;
                if (parent == null) { return; }

                if (parent.TryGetComponent(out Target target))
                {
                    //target.ActivateTarget();
                }
            }
            
        }

        public void OnDeselect(Transform[] transform)
        {

        }
    }
}

