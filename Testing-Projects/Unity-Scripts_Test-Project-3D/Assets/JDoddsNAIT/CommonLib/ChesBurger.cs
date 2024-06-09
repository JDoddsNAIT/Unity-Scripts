using System.Collections;
using UnityEngine;

public class Pair<T1, T2>
{
    public T1 Item1 { get; set; }
    public T2 Item2 { get; set; }

    public Pair(T1 item1, T2 item2)
    {
        Item1 = item1;
        Item2 = item2;
    }

    public Pair<T1, T2> Create(T1 item1, T2 item2) => new(item1, item2);
}
