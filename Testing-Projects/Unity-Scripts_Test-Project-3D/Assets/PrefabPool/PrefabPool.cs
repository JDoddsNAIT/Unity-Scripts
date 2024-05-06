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
    /// Returns an array of all objects in the <see cref="PrefabPool"/> that are active in the hierarchy. Logs a warning if none are found.
    /// </summary>
    public GameObject[] ActivePool
    {
        get
        {
            var activePool = Pool.Where(prefab => prefab.activeInHierarchy).ToArray();
            if (activePool.Length == 0)
            {
                Debug.LogWarning($"{prefab.name} pool is empty, null was returned.");
            }
            return activePool;
        }
    }

    /// <summary>
    /// Returns an array of all objects in the <see cref="PrefabPool"/> that are not active in the hierarchy. Logs a warning if none are found.
    /// </summary>
    public GameObject[] InactivePool
    {
        get
        {
            var inactivePool = Pool.Where(prefab => !prefab.activeInHierarchy).ToArray();
            if (inactivePool.Length == 0)
            {
                Debug.LogWarning($"{prefab.name} pool is empty, null was returned.");
            }
            return inactivePool;
        }
    }

    /// <summary>
    /// Returns and activates the next inactive object in <see cref="PrefabPool"/>.
    /// </summary>
    public GameObject Next
    {
        get
        {
            GameObject returnObject = InactivePool.FirstOrDefault();
            if (returnObject != null)
            {
                returnObject.SetActive(true);
            }
            return returnObject;
        }
    }
}
