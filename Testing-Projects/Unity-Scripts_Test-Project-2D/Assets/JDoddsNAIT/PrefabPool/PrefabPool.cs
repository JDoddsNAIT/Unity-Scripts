using UnityEngine;

[HelpURL("https://github.com/JDoddsNAIT/Unity-Scripts/tree/main/Scripts/Prefab-Pool")]
public class PrefabPool : MonoBehaviour
{
    private ObjectPool<GameObject> _pool;

    [Tooltip("The prefab that the script will create a pool of.")]
    public GameObject prefab;
    [Tooltip("The amount of objects to allocate memory for.")]
    [Min(1)] public int poolSize;
    [Tooltip("The parent of all objects in the pool.")]
    public Transform prefabParent;

    public GameObject[] Actives
    {
        get
        {
            GameObject[] result;
            try
            {
                result = _pool.ActiveObjects;
            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
                result = null;
            }
            return result;
        }
    }
    public GameObject[] Inactives
    {
        get
        {
            GameObject[] result;
            try
            {
                result = _pool.InactiveObjects;
            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
                result = null;
            }
            return result;
        }
    }

    private void Awake()
    {
        if (prefab == null)
        {
            Debug.LogError($"No value was assigned for member {nameof(prefab)} on {this.gameObject.name}.");
        }
        else
        {
            _pool = new ObjectPool<GameObject>(
                size: poolSize,
                initialize: obj => { obj = Instantiate(prefab, prefabParent); obj.SetActive(false); return obj; },
                activeCriteria: obj => obj.activeInHierarchy)
            {
                Activate = obj => obj.SetActive(true),
                Deactivate = obj => obj.SetActive(false)
            };
        }
    }

    public GameObject GetNext(System.Action<GameObject> activate)
    {
        GameObject next;
        try
        {
            next = _pool.NextInactive;
            _pool.ActivateObject(next, _pool.Activate);
        }
        catch (System.Exception ex)
        {
            Debug.LogException(ex);
            next = null;
        }
        return next;
    }
}
