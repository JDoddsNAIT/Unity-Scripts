using System.Collections;
using UnityEngine;

public abstract class TriggerAction : Trigger
{
    [Min(0)] public float activationDelay;

    private bool previous;

    private void Update()
    {
        if (SignalChannel.BoolValue && !previous)
        {
            if (activationDelay != 0)
            {
                StartCoroutine(DelayedTrigger(activationDelay));
            }
            else
            {
                Trigger();
            }
        }
        previous = SignalChannel.BoolValue;
    }

    private IEnumerator DelayedTrigger(float delay)
    {
        yield return new WaitForSeconds(delay);
        Trigger();
    }

    public abstract void Trigger();
}
