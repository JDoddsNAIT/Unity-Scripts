using System;
using System.Linq;
using UnityEngine;

public class AngleTester : MonoBehaviour
{
    class RayGizmo
    {
        public Vector2 Direction { get; set; }
        public Color Color { get; set; }

        public RayGizmo(float angle, Color defaultColor)
        {
            Direction = Quaternion.Euler(0, 0, angle) * Vector2.right;
            Color = defaultColor;
        }

        public void Draw(Vector3 position)
        {
            Gizmos.color = Color;
            Gizmos.DrawRay(position, Direction);
        }
    }

    public Color inputColor = Color.cyan;
    [Range(-360, 360)] public float inputAngle = 0;
    public float[] angles = new float[3];
    public Color angleColor = Color.red;
    public Color limitColor = Color.blue;
    public Color onColor = Color.green;

    private void OnDrawGizmos()
    {
        int index(int i) => (int)Mathf.Repeat(i, angles.Length);

        Vector2 input = inputAngle.ToVector2(); /*new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized*/;
        Gizmos.color = inputColor;
        Gizmos.DrawRay(transform.position, input);

        RayGizmo[] rayGizmos = new RayGizmo[angles.Length];
        Gizmos.color = limitColor;
        for (int i = 0; i < angles.Length; i++)
        {
            Gizmos.DrawRay(transform.position,
                (Vector2.Angle(angles[i].ToVector2(), angles[index(i + 1)].ToVector2()) / 2 + angles[i]).ToVector2());

            rayGizmos[i] = new RayGizmo(angles[i], angleColor);
        }

        if (input != Vector2.zero)
        {
            rayGizmos.OrderBy(r => Vector2.Angle(input, r.Direction)).First().Color = onColor;
        }
        for (int i = 0; i < rayGizmos.Length; i++)
        {
            rayGizmos[i].Draw(transform.position);
        }
    }

    [ContextMenu("Order Angles")] public void OrderAngles() => angles = angles.OrderBy(x => x % 360).ToArray();
}

public static class Extensions
{
    public static Vector2 ToVector2(this float value) => Quaternion.Euler(0, 0, value) * Vector2.right;
}