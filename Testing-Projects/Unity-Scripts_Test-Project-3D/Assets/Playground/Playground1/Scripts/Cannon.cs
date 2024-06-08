using System.Collections;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public Missile missile;
    public int maxMissiles;
    [Min(0)] public float shootingDelay;
    public Transform spawnPoint;

    private ObjectPool<Missile> _pool;
    private bool canShoot = true;

    void Start()
    {

        _pool = new ObjectPool<Missile>(
            size: maxMissiles,
            initialize: m => { m = Instantiate(missile); m.gameObject.SetActive(false); },
            activeCriteria: m => m.gameObject.activeInHierarchy);

        if (spawnPoint == null)
        {
            Debug.LogError($"No value was assigned for member {nameof(spawnPoint)} on {this.gameObject.name}.");
        }
    }

    void Update()
    {
        if (canShoot)
        {
            StartCoroutine(Shoot(shootingDelay));
        }
    }

    private IEnumerator Shoot(float delay)
    {
        //canShoot = false;
        //try
        //{
        //    _pool.ActivateNext(projectile => projectile.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation));
        //}
        //catch (System.Exception ex)
        //{
        //    Debug.LogException(ex);
        //}
        yield return new WaitForSeconds(delay);
        //canShoot = true;
    }
}
