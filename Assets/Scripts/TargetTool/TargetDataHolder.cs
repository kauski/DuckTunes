using System.Collections.Generic;
using UnityEngine;

namespace DuckTunes.TargetTool.Data
{
    [System.Serializable]
    public struct TargetDataHolder
    {
        readonly public Spline Spline;
        readonly public Circle[] Circles;
        readonly public List<Vector2> Handles;
        readonly public List<Vector2> Anchors;

        public TargetDataHolder(Spline spline, Circle[] circles, List<Vector2> handles, List<Vector2> anchors)
        {
            Spline = spline;
            Circles = circles;
            Handles = handles;
            Anchors = anchors;
        }

        public bool IsNull()
        {
            if (Spline != null && Circles[0] != null && Circles[1] != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}