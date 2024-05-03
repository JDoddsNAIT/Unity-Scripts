using System.Linq;
using UnityEngine;

public class PrefabPool : MonoBehaviour
{
    public GameObject prefab;
    [Min(1)] public int poolSize;
    public Transform prefabParent;

    public GameObject[] Pool { get; private set; }
    public GameObject[] ActivePool => Pool.Where(prefab => prefab.activeInHierarchy).ToArray();
    public bool IsActive => ActivePool.Length > 0;
    public bool IsEmpty => ActivePool.Length >= poolSize;

    private void Awake()
    {
        if (prefab == null)
        {
            Debug.LogError($"No value was assigned for member {nameof(prefab)} on {this.gameObject.name}.");
        }
        else
        {
            Pool = new GameObject[poolSize];
            for (int c = 0; c < poolSize; c++)
            {
                Pool[c] = prefabParent != null ? Instantiate(prefab, prefabParent) : Instantiate(prefab);
                Pool[c].SetActive(false);
            }
        }
    }

    public GameObject Next
    {
        get
        {
            GameObject returnObject = Pool.Where(prefab => !prefab.activeInHierarchy).FirstOrDefault();
            if (returnObject == null)
            {
                Debug.LogWarning($"{prefab.name} pool is empty, null was returned.");
            }
            else
            {
                returnObject.SetActive(true);
            }
            return returnObject;
        }
    }
}
