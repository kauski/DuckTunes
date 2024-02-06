using System;
using UnityEngine;
using DuckTunes.TargetTool.Data;
using DuckTunes.TargetTool.Utility;

namespace DuckTunes.TargetTool
{
    public class TargetCreator : TargetCreatorBase, ITargetCreator
    {
#if UNITY_EDITOR
        public void Create()
        {
            Spline = new Spline(transform.position);
            Circles = CreateCircles();
            UpdateAllHandlePositions();
            UpdateAllAnchorPositions();
            Handles = GetHandlePositions();
            Anchors = GetAnchorPositions();

            CurrentTarget = new TargetDataHolder(
                Spline,
                Circles,
                Handles,
                Anchors
            );
        }

        public override void Save()
        {
            base.Save();
        }
#endif
    }
}