using UnityEngine;

public abstract class Trigger : MonoBehaviour
{
    [Min(0)] public int ChannelID;

    [field: SerializeField] protected ChannelManager ChannelManager { get; private set; }
    protected Channel SignalChannel => ChannelManager.ChannelsDict[ChannelID];

    private void Awake()
    {
        ChannelManager = FindObjectOfType<ChannelManager>();
    }
}
