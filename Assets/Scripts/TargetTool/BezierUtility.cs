using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuckTunes.Utility
{
    public static class BezierUtility
    {
        public static Vector2 Lerp(Vector2 pointA, Vector2 pointB, float t)
        {
            return pointA + (pointB - pointA) * t;
        }

        public static Vector2 QuadricLerp(Vector2 pointA, Vector2 pointB, Vector2 pointC, float t )
        {
            Vector2 ab = Lerp(pointA, pointB, t);
            Vector2 bc = Lerp(pointB, pointC, t);
            return Lerp(ab, bc, t);
        }

        public static Vector2 CubicCurve(Vector2 pointA, Vector2 pointB, Vector2 pointC, Vector2 pointD, float t )
        {
            Vector2 abc = QuadricLerp(pointA, pointB, pointC, t);
            Vector2 bcd = QuadricLerp(pointB, pointC, pointD, t);
            return Lerp(abc, bcd, t);
        }
    }
}

