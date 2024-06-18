using System.Linq;
using UnityEngine;

public abstract class Trigger : MonoBehaviour
{
    [Min(0)] public int ChannelID;
    protected Channel channel;

    private void Awake()
    {
        channel = FindObjectsOfType<Channel>().Where(c => c.ChannelId == ChannelID).FirstOrDefault();
    }
}
