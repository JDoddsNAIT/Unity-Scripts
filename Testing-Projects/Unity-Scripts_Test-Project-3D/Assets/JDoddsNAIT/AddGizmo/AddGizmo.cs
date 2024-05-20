using UnityEngine;

[HelpURL("https://github.com/JDoddsNAIT/Unity-Scripts/tree/main/Scripts/Add-Gizmo")]
public class AddGizmo : MonoBehaviour
{
    public enum Shape
    {
        Sphere,
        WireSphere,
        Cube,
        WireCube,
        Ray,
    }

    public Shape gizmo;
    public Color color = Color.red;
    public bool onSelected;
    [Space]
    public Space space = Space.World;
    public Vector3 size = Vector3.right;
    public Vector3 position;

    private void OnDrawGizmos()
    {
        if (enabled && !onSelected)
        {
            DrawGizmo();
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (enabled && onSelected)
        {
            DrawGizmo();
        }
    }

    private void DrawGizmo()
    {
        Vector3 relativePosition = transform.rotation * new Vector3(x: transform.localScale.x * position.x,
                                                                    y: transform.localScale.y * position.y,
                                                                    z: transform.localScale.z * position.z);
        Vector3 relativeSize = new(x: transform.localScale.x * size.x,
                                   y: transform.localScale.y * size.y,
                                   z: transform.localScale.z * size.z);

        Vector3 gizmoPosition = space == Space.Self
            ? transform.position + relativePosition
            : transform.position + position;
        Vector3 gizmoSize = space == Space.Self
            ? relativeSize
            : size;
        Gizmos.color = color;
        switch (gizmo)
        {
            case Shape.Sphere:
                Gizmos.DrawSphere(gizmoPosition, gizmoSize.magnitude);
                break;
            case Shape.WireSphere:
                Gizmos.DrawWireSphere(gizmoPosition, gizmoSize.magnitude);
                break;
            case Shape.Cube:
                Gizmos.DrawCube(gizmoPosition, gizmoSize);
                break;
            case Shape.WireCube:
                Gizmos.DrawWireCube(gizmoPosition, gizmoSize);
                break;
            case Shape.Ray:
                gizmoSize = space == Space.Self ? transform.rotation * gizmoSize : gizmoSize;
                Gizmos.DrawRay(gizmoPosition, gizmoSize);
                break;
        }
    }
}
