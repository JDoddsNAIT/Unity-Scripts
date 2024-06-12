using UnityEngine;

[HelpURL("https://github.com/JDoddsNAIT/Unity-Scripts/tree/main/Scripts/Add-Gizmo")]
public class AddGizmo : MonoBehaviour
{
    public enum WireState { Solid, Wire, }
    public enum Shape { Sphere, Cube, Mesh, Ray, Line, }
    public enum DrawMode { Always, WhileSelected, }

    #region Inspector
    public WireState wire;
    public Shape shape = Shape.Sphere;
    [SerializeField] Color color = Color.white;
    [SerializeField] Vector3 position = Vector3.zero;
    [SerializeField] DrawMode drawMode = DrawMode.Always;

    [SerializeField] float radius = .5f;
    [SerializeField] Vector3 size = Vector3.one;
    [SerializeField] Mesh mesh;
    [SerializeField] bool rotate = true;
    [SerializeField] Vector3 direction = Vector3.forward;
    [SerializeField] Vector3 from, to;
    #endregion

    private void OnDrawGizmosSelected()
    {
        if (enabled && drawMode == DrawMode.WhileSelected)
        {
            DrawGizmo();
        }
    }

    private void OnDrawGizmos()
    {
        if (enabled && drawMode == DrawMode.Always)
        {
            DrawGizmo();
        }
    }

    private void DrawGizmo()
    {
        Gizmos.color = color;
        Vector3 gizmoPosition = transform.position + position;
        switch (shape)
        {
            case Shape.Sphere:
                if (wire == WireState.Wire)
                {
                    Gizmos.DrawWireSphere(gizmoPosition, radius);
                }
                else
                {
                    Gizmos.DrawSphere(gizmoPosition, radius);
                }
                break;
            case Shape.Cube:
                if (wire == WireState.Wire)
                {
                    Gizmos.DrawWireCube(gizmoPosition, size);
                }
                else
                {
                    Gizmos.DrawCube(gizmoPosition, size);
                }
                break;
            case Shape.Mesh:
                if (wire == WireState.Wire)
                {
                    Gizmos.DrawWireMesh(mesh, gizmoPosition);
                }
                else
                {
                    Gizmos.DrawMesh(mesh, gizmoPosition);
                }
                break;
            case Shape.Ray:
                if (rotate)
                {
                    direction = transform.rotation * direction;
                }
                Gizmos.DrawRay(gizmoPosition, direction);
                break;
            case Shape.Line:
                Gizmos.DrawLine((rotate ? transform.rotation * from : from) + gizmoPosition, (rotate ? transform.rotation * to : to) + gizmoPosition);
                break;
        }
    }
}
