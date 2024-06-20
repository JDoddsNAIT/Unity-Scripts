using UnityEngine;

[HelpURL("https://github.com/JDoddsNAIT/Unity-Scripts/tree/main/Scripts/Add-Gizmo"), AddComponentMenu("Miscellaneous/Gizmo")]
public class AddGizmo : MonoBehaviour
{
    public enum WireState { Solid, Wire, }
    public enum Shape { Sphere, Cube, Mesh, Ray, Line, }
    public enum DrawMode { Always, WhileSelected, }

    #region Inspector
    [SerializeField] Color color = Color.white;
    public WireState wire;
    public Shape shape = Shape.Sphere;
    [SerializeField] DrawMode drawMode = DrawMode.Always;
    [SerializeField] Mesh mesh;

    [SerializeField] bool useTransformPosition = true;
    [SerializeField] Vector3 position = Vector3.zero;

    [SerializeField] bool useTransformRotation = true;
    [SerializeField] Vector3 rotation = Vector3.zero;

    [SerializeField] bool useTransformScale = true;
    [SerializeField] Vector3 scale = Vector3.one;
    [SerializeField] float radius = .5f;

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

    private void Update() { }

    private void DrawGizmo()
    {
        static Vector3 multiplyVector3(Vector3 a, Vector3 b) => new(a.x * b.x, a.y * b.y, a.z * b.z);

        Gizmos.color = color;
        var transformPosition = useTransformPosition ? transform.position : Vector3.zero;
        var transformRotation = useTransformRotation ? transform.rotation.eulerAngles : Vector3.zero;
        var transformScale = useTransformScale ? transform.localScale : Vector3.one;

        Vector3 gizmoPosition = transformPosition + position;
        OperableQuaternion gizmoRotation = OperableQuaternion.Euler(transformRotation + rotation);
        Vector3 gizmoScale = multiplyVector3(transformScale, scale);

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
                    Gizmos.DrawWireCube(gizmoPosition, scale);
                }
                else
                {
                    Gizmos.DrawCube(gizmoPosition, scale);
                }
                break;
            case Shape.Mesh:
                if (wire == WireState.Wire)
                {
                    Gizmos.DrawWireMesh(mesh, gizmoPosition, gizmoRotation, gizmoScale);
                }
                else
                {
                    Gizmos.DrawMesh(mesh, gizmoPosition, gizmoRotation, gizmoScale);
                }
                break;
            case Shape.Ray:
                Gizmos.DrawRay(gizmoPosition, gizmoRotation * gizmoScale);
                break;
            case Shape.Line:
                Gizmos.DrawLine(
                    gizmoRotation * from + gizmoPosition,
                    gizmoRotation * to + gizmoPosition);
                break;
        }
    }
}
