using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public enum Axis
    {
        Vertical, Horizontal
    }
    [Min(0)] public float moveSpeed = 2;
    public Axis axis;

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = axis == Axis.Vertical 
            ? new Vector3(horizontal, vertical, 0).normalized
            : new Vector3(horizontal, 0, vertical).normalized;
        transform.Translate(moveSpeed * Time.deltaTime * direction);
    }
}
