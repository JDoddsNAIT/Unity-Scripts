using System.Collections;
using UnityEngine;

public class TriggerCollision : Trigger
{
    enum Activation { Trigger, Collision, Both };
    [SerializeField] private Activation activation;

    private void OnTriggerStay(Collider other)
    {
        if (activation != Activation.Collision)
        {
            SignalChannel.FloatValue += Time.deltaTime;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (activation != Activation.Collision)
        {
            SignalChannel.FloatValue = 0; 
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (activation != Activation.Trigger)
        {
            SignalChannel.FloatValue += Time.deltaTime;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (activation != Activation.Trigger)
        {
            SignalChannel.FloatValue = 0;
        }
    }
}
