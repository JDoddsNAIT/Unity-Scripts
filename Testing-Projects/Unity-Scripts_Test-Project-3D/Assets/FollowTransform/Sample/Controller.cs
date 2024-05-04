using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [Min(0)] public float moveSpeed = 2;

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        transform.Translate(moveSpeed * Time.deltaTime * direction);
    }
}
