using UnityEngine;

public static class QuaternionExtensions
{
    public static Vector3 Forward(this Quaternion q) => q * Vector3.forward;
    public static Vector3 Back(this Quaternion q) => q * Vector3.back;
    public static Vector3 Right(this Quaternion q) => q * Vector3.right;
    public static Vector3 Left(this Quaternion q) => q * Vector3.left;
    public static Vector3 Up(this Quaternion q) => q * Vector3.up;
    public static Vector3 Down(this Quaternion q) => q * Vector3.down;

    public static Quaternion FromToRotation(this Quaternion from, Quaternion to) => Quaternion.FromToRotation(from.Forward(), to.Forward());

    public static Quaternion LookTowards(this Quaternion from, Vector3 toDirection, Vector3 up, float maxDegrees) =>
        Quaternion.RotateTowards(from, Quaternion.LookRotation(toDirection, up), maxDegrees);

    public static Quaternion LookTowards(this Quaternion from, Vector3 toDirection, float maxDegrees) =>
        Quaternion.RotateTowards(from, Quaternion.LookRotation(toDirection), maxDegrees);

    // Quaternion math
    public static Quaternion Add(this Quaternion a, Quaternion b) => Quaternion.Euler(a.eulerAngles + b.eulerAngles);

    public static Quaternion Scale(this Quaternion q, float s) => Quaternion.Euler(q.eulerAngles * s);
}

//public static class TransformExtensions
//{
//    public static void LookTowards(this Transform transform, Vector3 toDirection, Vector3 up, float maxDegrees)
//    {
//        transform.rotation = Quaternion.RotateTowards(
//            from: transform.rotation,
//            to: Quaternion.LookRotation(toDirection, up),
//            maxDegrees);
//    }
//}