using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorExtensions
{
    public static Vector3 With(this Vector3 orig, float? x = null, float? y = null, float? z = null)
    {
        return new Vector3(x ?? orig.x, y ?? orig.y, z ?? orig.z);
    }
}
