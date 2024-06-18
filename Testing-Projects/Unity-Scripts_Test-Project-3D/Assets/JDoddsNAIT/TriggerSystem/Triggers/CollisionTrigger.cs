using System.Collections;
using UnityEngine;

public class CollisionTrigger : Trigger
{
    enum Activation { Trigger, Collision, Both };
    [SerializeField] private Activation activation;

    private void OnTriggerStay(Collider other)
    {
        if (activation != Activation.Collision)
        {
            channel.FloatValue += Time.deltaTime;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (activation != Activation.Collision)
        {
            channel.FloatValue = 0; 
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (activation != Activation.Trigger)
        {
            channel.FloatValue += Time.deltaTime;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (activation != Activation.Trigger)
        {
            channel.FloatValue = 0;
        }
    }
}
