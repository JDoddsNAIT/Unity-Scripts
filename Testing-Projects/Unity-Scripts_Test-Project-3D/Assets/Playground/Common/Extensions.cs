using UnityEngine;

public static class QuaternionExtensions
{
    public static Vector3 Forward(this Quaternion q) => q * Vector3.forward;
    public static Vector3 Back(this Quaternion q) => q * Vector3.back;
    public static Vector3 Right(this Quaternion q) => q * Vector3.right;
    public static Vector3 Left(this Quaternion q) => q * Vector3.left;
    public static Vector3 Up(this Quaternion q) => q * Vector3.up;
    public static Vector3 Down(this Quaternion q) => q * Vector3.down;

    public static Quaternion RotateTowards(this Quaternion from, Vector3 to, float maxDegrees) =>
        Quaternion.RotateTowards(from, Quaternion.FromToRotation(Vector3.forward, to), maxDegrees);

    public static Quaternion LookTowards(this Quaternion from, Vector3 to, Vector3 up, float maxDegrees) =>
        Quaternion.RotateTowards(from, Quaternion.LookRotation(to, up), maxDegrees);

    public static Quaternion LookTowards(this Quaternion from, Vector3 to, float maxDegrees) =>
        Quaternion.RotateTowards(from, Quaternion.LookRotation(to), maxDegrees);
}

//public static class TransformExtensions
//{
//    public static void LookTowards(this Transform transform, Vector3 to, Vector3 up, float maxDegrees)
//    {
//        transform.rotation = Quaternion.RotateTowards(
//            from: transform.rotation,
//            to: Quaternion.LookRotation(to, up),
//            maxDegrees);
//    }
//}