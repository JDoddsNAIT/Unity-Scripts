using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBody : MonoBehaviour
{
    public float turnSpeed;
    
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, turnSpeed * 90 * Time.deltaTime, 0);
    }
}
