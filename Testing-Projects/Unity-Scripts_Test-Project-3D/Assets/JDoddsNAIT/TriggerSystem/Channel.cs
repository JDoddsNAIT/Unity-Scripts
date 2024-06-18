using UnityEngine;

public class Channel : MonoBehaviour
{
    public int ChannelId;

    [SerializeField] private float value;
    public float FloatValue { get => value; set => this.value = value; }
    public int IntValue {  get => (int)value; set => this.value = value;}
    public bool BoolValue { get => value != default; set => this.value = value ? 1 : 0; }

    private void Update()
    {
        gameObject.name = $"Channel (id: {ChannelId})";
    }
}
