using UnityEngine;

namespace DuckTunes.Utility
{
    public static class SplineUtil
    {
        public static Vector3 GetNearestPoint(Vector3[] splinePoints, Vector3 worldPoint, out Vector3 nearestInSpline, int resolution = 1024)
        {
            float smallestDist = float.MaxValue;
            float step = 1f / resolution;
            nearestInSpline = Vector3.zero;

            for (int i = 0; i < resolution; i++)
            {
                Vector3 p = GetPoint(splinePoints, i * step);
                float delta = (worldPoint - p).sqrMagnitude;

                if (delta < smallestDist)
                {
                    nearestInSpline = p;
                    smallestDist = delta;
                }
            }

            return nearestInSpline;
        }

        public static Vector3 GetPoint(Vector3[] points, float t)
        {
            int sections = points.Length - 3;
            int i = Mathf.Min(Mathf.FloorToInt(t * (float)sections), sections - 1);
            int count = points.Length;

            if (i < 0) { i += count; }
            float u = t * (float)sections - (float)i;
            Vector3 a = points[(i + 0) % count];
            Vector3 b = points[(i + 1) % count];
            Vector3 c = points[(i + 2) % count];
            Vector3 d = points[(i + 3) % count];

            return BezierUtility.CubicCurve(a, b, c, d, u);
        }
    }
}