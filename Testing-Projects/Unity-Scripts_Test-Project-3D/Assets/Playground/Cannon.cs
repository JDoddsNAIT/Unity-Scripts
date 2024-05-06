using System.Collections;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public PrefabPool pool;
    [Min(0)] public float shootingDelay;
    public Transform spawnPoint;

    private bool canShoot = true;

    void Start()
    {
        if (pool == null)
        {
            Debug.LogError($"No value was assigned for member {nameof(pool)} on {this.gameObject.name}.");
        }

        if (spawnPoint == null)
        {
            Debug.LogError($"No value was assigned for member {nameof(spawnPoint)} on {this.gameObject.name}.");
        }
    }

    void Update()
    {
        if (canShoot)
        {
            canShoot = false;
            StartCoroutine(Shoot(shootingDelay));
        }
    }

    private IEnumerator Shoot(float delay)
    {
        GameObject projectile = pool.Next;
        projectile.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
        yield return new WaitForSeconds(delay);
        canShoot = true;
    }
}
