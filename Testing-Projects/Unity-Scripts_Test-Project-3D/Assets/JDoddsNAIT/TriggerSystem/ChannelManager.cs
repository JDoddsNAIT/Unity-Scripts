using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChannelManager : MonoBehaviour
{
    public Transform ChannelParent;
    public Channel ChannelPrefab;
    [SerializeField, NonReorderable] Channel[] Channels;

    public Dictionary<int, Channel> ChannelsDict;

    [ContextMenu(nameof(CreateChannels))]
    public void CreateChannels()
    {
        ChannelsDict = new Dictionary<int, Channel>();
        int[] ids = FindObjectsOfType<Trigger>().Select(x => x.ChannelID).Distinct().OrderBy(x => x).ToArray();

        for (int i = 0; i < ids.Length; i++)
        {
            var channel = Instantiate(ChannelPrefab, ChannelParent);
            channel.ChannelId = ids[i];
            channel.BoolValue = false;

            ChannelsDict.Add(channel.ChannelId, channel);
        }

        Channels = ChannelsDict.Select(x => x.Value).ToArray();
    }

    private void Awake()
    {
        CreateChannels();
    }
}
