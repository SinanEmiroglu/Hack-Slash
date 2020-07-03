using UnityEngine;

public static class Vector3Extensions
{
    /// <summary>
    /// if there is nothing referenced those parameters they are going to return null.
    /// if there is a  value they will return x; otherwise, original value.
    /// </summary>
    public static Vector3 With(this Vector3 original, float? x = null, float? y = null, float? z = null)
    {
        return new Vector3(x ?? original.x, y ?? original.y, z ?? original.z);
    }
}