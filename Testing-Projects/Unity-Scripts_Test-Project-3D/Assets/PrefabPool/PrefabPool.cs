using System.Linq;
using UnityEngine;

[HelpURL("https://github.com/JDoddsNAIT/Unity-Scripts/tree/main/Scripts/Prefab-Pool")]
public class PrefabPool : MonoBehaviour
{
    [Tooltip("The prefab that the script will create a pool of.")]
    public GameObject prefab;
    [Tooltip("The amount of objects to allocate memory for.")]
    [Min(1)] public int poolSize;
    [Tooltip("The parent of all objects in the pool.")]
    public Transform prefabParent;

    // Properties
    /// <summary>
    /// Returns the entire pool of objects.
    /// </summary>
    public GameObject[] Pool { get; private set; }

    /// <summary>
    /// Returns an array of all objects in the <see cref="PrefabPool"/> that are active in the hierarchy.
    /// </summary>
    public GameObject[] ActivePool => Pool.Where(prefab => prefab.activeInHierarchy).ToArray();

    /// <summary>
    /// Returns true if any objects in the <see cref="PrefabPool"/> are active in the hierarchy.
    /// </summary>
    public bool IsActive => ActivePool.Length > 0;

    /// <summary>
    /// Returns true if all objects in the <see cref="PrefabPool"/> are active in the hierarchy.
    /// </summary>
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
                Pool[c] = Instantiate(prefab, prefabParent);
                Pool[c].SetActive(false);
            }
        }
    }

    /// <summary>
    /// Returns the next inactive object in <see cref="PrefabPool"/>. Logs a warning if no object is found.
    /// </summary>
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
