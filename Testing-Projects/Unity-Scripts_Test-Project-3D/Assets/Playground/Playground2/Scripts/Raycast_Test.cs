using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast_Test : MonoBehaviour
{
    public Vector3 direction = Vector3.forward;
    [Min(0)] public float maxDistance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Color rayColour;
        if (Physics.Raycast(transform.position, direction, out var hitInfo, maxDistance))
        {
            rayColour = Color.white;
            Debug.Log("Raycast hit!");
        }
        else
        {
            rayColour = Color.yellow;
        }
        Debug.DrawRay(transform.position, direction * maxDistance, rayColour);
    }
}
