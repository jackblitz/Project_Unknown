using UnityEngine;
public class RotationUtils
{
    public static float AngleBetweenVector(Vector3 to, Vector3 from)
    {
        return Quaternion.FromToRotation(Vector3.back, to - from).eulerAngles.y;
    }

    public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }
}
