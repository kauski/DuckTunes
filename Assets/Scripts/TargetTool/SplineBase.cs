using System.Collections.Generic;
using UnityEngine;
using DuckTunes.Utility;

namespace DuckTunes.TargetTool
{
    [System.Serializable]
    public class SplineBase
    {
        [HideInInspector] public List<Vector2> Points;

        public void AutoSetAllAffectedHandlePoints(int updatedAnchorIndex)
        {
            for (int i = updatedAnchorIndex - 3; i <= updatedAnchorIndex + 3; i += 3)
            {
                if (i >= 0)
                {
                    AutoSetAnchorHandlePoints(LoopIndex(i, Points));
                }
            }

            AutoSetStartAndEndHandles();
        }

        public void AutoSetAllHandlePoints()
        {
            for (int i = 0; i < Points.Count; i += 3)
            {
                AutoSetAnchorHandlePoints(i);

            }
            AutoSetStartAndEndHandles();
        }

        public void AutoSetAnchorHandlePoints(int anchorIndex)
        {
            Vector2 anchorPos = Points[anchorIndex];
            Vector2 dir = Vector2.zero;
            float[] neighbourDistances = new float[2];
            dir = CalculateNeighbourDistance(anchorIndex, anchorPos, dir, neighbourDistances);

            dir.Normalize();
            SetHandlePositions(anchorIndex, anchorPos, dir, neighbourDistances);
        }

        private void AutoSetStartAndEndHandles()
        {
            Points[1] = (Points[0] + Points[2]) * 0.5f;
            Points[Points.Count - 2] = (Points[Points.Count - 1] + Points[Points.Count - 3]) * 0.5f;
        }

        public void Create3PointsFromAnchorPos(Vector2 anchorPos)
        {
            Points.Add(Points[Points.Count - 1] * 2 - Points[Points.Count - 2]);
            Points.Add((Points[Points.Count - 1] + anchorPos) * 0.5f);
            Points.Add(anchorPos);
        }

        public void InsertRangeOfPoints(Vector2 anchorPos, int segmentIndex)
        {
            Points.InsertRange(segmentIndex * 3 + 2, new Vector2[] {
                Vector2.zero,
                anchorPos,
                Vector2.zero
            });
        }

        public void RemoveSegmentPoints(int anchorIndex)
        {
            if (anchorIndex == 0)
            {
                Points.RemoveRange(0, 3);
            }
            else if (anchorIndex == Points.Count - 1)
            {
                Points.RemoveRange(anchorIndex - 2, 3);
            }
            else
            {
                Points.RemoveRange(anchorIndex - 1, 3);
            }
        }

        public void HandleHandlesMove(int pointIndex, Vector2 pos)
        {
            bool isNextPointAnchorPoint = (pointIndex + 1) % 3 == 0;
            int corresPondingPointIndex = (isNextPointAnchorPoint) ? pointIndex + 2 : pointIndex - 2;
            int anchorIndex = (isNextPointAnchorPoint) ? pointIndex + 1 : pointIndex - 1;

            if (corresPondingPointIndex >= 0 && corresPondingPointIndex < Points.Count)
            {
                float dst = (Points[LoopIndex(anchorIndex, Points)] - Points[LoopIndex(corresPondingPointIndex, Points)]).magnitude;
                Vector2 dir = (Points[LoopIndex(anchorIndex, Points)] - pos).normalized;
                Points[LoopIndex(corresPondingPointIndex, Points)] = Points[LoopIndex(anchorIndex, Points)] + dir * dst;
            }
        }

        public void HandleFirstAndLastAnchor(int pointIndex, Vector2 deltaMove)
        {
            if (pointIndex + 1 < Points.Count)
            {
                Points[LoopIndex(pointIndex + 1, Points)] += deltaMove;
            }
            if (pointIndex - 1 >= 0)
            {
                Points[LoopIndex(pointIndex - 1, Points)] += deltaMove;
            }
        }

        public void HandleManualMove(int pointIndex, Vector2 pos, Vector2 deltaMove)
        {
            if (pointIndex % 3 == 0)
            {
                HandleFirstAndLastAnchor(pointIndex, deltaMove);
            }
            else
            {
                HandleHandlesMove(pointIndex, pos);
            }
        }

        public List<Vector2> InitializeVector2List()
        {
            List<Vector2> evenlySpacedPoints = new List<Vector2>();
            evenlySpacedPoints.Add(Points[0]);
            return evenlySpacedPoints;
        }

        public void AddEvenlySpacedPoint(float spacing, List<Vector2> evenlySpacedPoints, ref Vector2 previousPoint, ref float dstSinceLastPoint, Vector2 pointOnCurve)
        {
            float overshootDst = dstSinceLastPoint - spacing;
            Vector2 newEvenlySpacedPoint = pointOnCurve + (previousPoint - pointOnCurve).normalized * overshootDst;
            evenlySpacedPoints.Add(newEvenlySpacedPoint);
            dstSinceLastPoint = overshootDst;
            previousPoint = newEvenlySpacedPoint;
        }

        public Vector2 CalculateDistanceFromPreviousPoint(Vector2 previousPoint, ref float dstSinceLastPoint, Vector2[] p, float t)
        {
            Vector2 pointOnCurve = BezierUtility.CubicCurve(p[0], p[1], p[2], p[3], t);
            dstSinceLastPoint += Vector2.Distance(previousPoint, pointOnCurve);
            return pointOnCurve;
        }

        public int CalculateDivisions(float resolution, Vector2[] p)
        {
            float handleNetLength = Vector2.Distance(p[0], p[1]) + Vector2.Distance(p[1], p[2]) + Vector2.Distance(p[2], p[3]);
            float estimatedCurveLength = Vector2.Distance(p[0], p[3]) + handleNetLength / 2f;
            int divisions = Mathf.CeilToInt(estimatedCurveLength * resolution * 10);
            return divisions;
        }

        public void InitializeValues(out List<Vector2> evenlySpacedPoints, out Vector2 previousPoint, out float dstSinceLastPoint)
        {
            evenlySpacedPoints = InitializeVector2List();
            previousPoint = Points[0];
            dstSinceLastPoint = 0;
        }

        public Vector2 CalculateNeighbourDistance(int anchorIndex, Vector2 anchorPos, Vector2 dir, float[] neighbourDistances)
        {
            if (anchorIndex - 3 >= 0)
            {
                Vector2 offset = Points[LoopIndex(anchorIndex - 3, Points)] - anchorPos;
                dir += offset.normalized;
                neighbourDistances[0] = offset.magnitude;
            }
            if (anchorIndex + 3 >= 0)
            {
                Vector2 offset = Points[LoopIndex(anchorIndex + 3, Points)] - anchorPos;
                dir -= offset.normalized;
                neighbourDistances[1] = -offset.magnitude;
            }

            return dir;
        }

        public void SetHandlePositions(int anchorIndex, Vector2 anchorPos, Vector2 dir, float[] neighbourDistances)
        {
            for (int i = 0; i < 2; i++)
            {
                int handleIndex = anchorIndex + i * 2 - 1;
                if (handleIndex >= 0 && handleIndex < Points.Count)
                {
                    Points[LoopIndex(handleIndex, Points)] = anchorPos + dir * neighbourDistances[i] * 0.5f;
                }
            }
        }

        public void HandleAnchorPointMove(int pointIndex, Vector2 pos, Vector2 deltaMove, bool autoSetHandlePoints)
        {
            if (pointIndex % 3 == 0 || !autoSetHandlePoints)
            {
                Points[pointIndex] = pos;
            }

            if (!autoSetHandlePoints)
            {
                HandleManualMove(pointIndex, pos, deltaMove);
            }
            else
            {
                AutoSetAllAffectedHandlePoints(pointIndex);
            }
        }

        public Vector2[] GetStartAndEndPoints()
        {
            Vector2[] points = new Vector2[2];
            points[0] = Points[0];
            points[1] = Points[Points.Count - 1];
            return points;
        }

        public int LoopIndex(int i, List<Vector2> _points)
        {
            return (i + _points.Count) % _points.Count;
        }
    }
}
