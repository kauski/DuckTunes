using System.Collections.Generic;
using UnityEngine;
using DuckTunes.TargetTool.Utility;

namespace DuckTunes.TargetTool.Data
{
    [System.Serializable]
    public class Spline : SplineBase
    {
        [SerializeField, HideInInspector] private bool _autoSetHandlePoints;

        public Spline(Vector2 centre)
        {
            Points = new List<Vector2>
            {
                centre + Vector2.left,
                centre + (Vector2.left + Vector2.up) * 0.5f,
                centre + (Vector2.right + Vector2.down) * 0.5f,
                centre + Vector2.right
            };
        }

        public Vector2 this[int i] => Points[i];

        public int NumPoints => Points.Count;
        
        public int NumSegments => Points.Count / 3;

        public List<Vector2> GetVectorArray => Points;

        public bool AutoSetHandlePoints
        {
            get
            {
                return _autoSetHandlePoints;
            }
            set
            {
                if (_autoSetHandlePoints != value)
                {
                    _autoSetHandlePoints = value;
                    if (_autoSetHandlePoints)
                    {
                        AutoSetAllHandlePoints();
                    }
                }
            }
        }

        public void AddSegment(Vector2 anchorPos)
        {
            Create3PointsFromAnchorPos(anchorPos);
            if (_autoSetHandlePoints)
            {
                AutoSetAllAffectedHandlePoints(Points.Count - 1);
            }
        }

        public void SplitSegment(Vector2 anchorPos, int segmentIndex)
        {
            InsertRangeOfPoints(anchorPos, segmentIndex);

            if (_autoSetHandlePoints)
            {
                AutoSetAllAffectedHandlePoints(segmentIndex * 3 + 3);
            }
            else
            {
                AutoSetAnchorHandlePoints(segmentIndex * 3 + 3);
            }
        }

        public void DeleteSegment(int anchorIndex)
        {
            if (NumSegments > 2)
            {
                RemoveSegmentPoints(anchorIndex);
            }
        }   

        public Vector2[] GetPointsInSegment(int segmentNumber)
        {
            return new Vector2[] { Points[segmentNumber * 3], Points[segmentNumber * 3 + 1], Points[segmentNumber * 3 + 2], Points[LoopIndex(segmentNumber * 3 + 3, Points)] };
        }

        public void PlacePointsOnSegment(float spacing, float resolution, List<Vector2> evenlySpacedPoints, ref Vector2 previousPoint, ref float dstSinceLastPoint)
        {
            for (int segmentIndex = 0; segmentIndex < NumSegments; segmentIndex++)
            {
                Vector2[] p = GetPointsInSegment(segmentIndex);
                int divisions = CalculateDivisions(resolution, p);
                float t = 0;
                while (t <= 1)
                {
                    t += 1f / divisions;

                    Vector2 pointOnCurve = CalculateDistanceFromPreviousPoint(previousPoint, ref dstSinceLastPoint, p, t);

                    while (dstSinceLastPoint >= spacing)
                    {
                        AddEvenlySpacedPoint(spacing, evenlySpacedPoints, ref previousPoint, ref dstSinceLastPoint, pointOnCurve);
                    }

                    previousPoint = pointOnCurve;
                }
            }
        }

        public void MovePoint(int pointIndex, Vector2 pos)
        {
            Vector2 deltaMove = pos - Points[pointIndex];
            HandleAnchorPointMove(pointIndex, pos, deltaMove, AutoSetHandlePoints);
        }

        public Vector2[] CalculateEvenlySpacedPoints(float spacing, float resolution = 1)
        {
            List<Vector2> evenlySpacedPoints;
            Vector2 previousPoint;
            float dstSinceLastPoint;
            InitializeValues(out evenlySpacedPoints, out previousPoint, out dstSinceLastPoint);
            PlacePointsOnSegment(spacing, resolution, evenlySpacedPoints, ref previousPoint, ref dstSinceLastPoint);

            return evenlySpacedPoints.ToArray();
        }
    }
}