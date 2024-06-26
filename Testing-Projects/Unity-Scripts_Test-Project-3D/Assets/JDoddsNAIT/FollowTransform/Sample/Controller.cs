using UnityEngine;

public class Controller : MonoBehaviour
{
    public enum Axis
    {
        Vertical, Horizontal
    }
    [Min(0)] public float moveSpeed = 2;
    public Axis axis;

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = axis == Axis.Vertical 
            ? new Vector3(horizontal, vertical, 0).normalized
            : new Vector3(horizontal, 0, vertical).normalized;
        transform.Translate(moveSpeed * Time.deltaTime * direction);
    }
}
