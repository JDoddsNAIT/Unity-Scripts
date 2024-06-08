using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    #region Inspector
    [Header("Projectile Settings")]
    [SerializeReference] public Projectile projectile;
    [Min(1)] public int maxProjectiles = 10;
    [SerializeReference] public Transform spawnParent;
    [Tooltip("Time in seconds until the projectile de-spawns.")]
    [Min(0)] public float lifeTime = 1.15f;

    [Header("Launch Settings")]
    public bool spawnOnStart = false;
    [Min(0.01f)] public float spawnDelay = 1.15f;
    [Space]
    [Min(0)] public float launchForce = 8.0f;
    [Range(-180, 180)] public float launchAngle = 45.0f;
    public bool flipX = false;

    [Header("Gizmo Settings")]
    [SerializeField] Color _color = Color.yellow;
    [Space]
    [SerializeField] bool _showLaunchVelocity = false;
    [Space]
    [SerializeField] bool _showTrajectory = true;
    [SerializeField, Range(1, 100)] int _resolution = 25;
    [Space]
    [SerializeField] bool _showFinalPosition = true;
    [SerializeField, Range(0, 1)] float _radius = 0.2f;
    #endregion

    private ObjectPool<Projectile> _projectilePool;
    private bool _spawning;

    private Vector2 LaunchDirection
    {
        get
        {
            int flip = flipX ? -1 : 1;
            return Quaternion.Euler(0, 0, launchAngle * flip) * (Vector2.right * flip);
        }
    }

    private void Start()
    {
        try
        {
            if (projectile == null)
            {
                throw new System.ArgumentNullException(nameof(projectile));
            }

            _projectilePool = new ObjectPool<Projectile>(
                size: maxProjectiles,
                p => { p = Instantiate(projectile, spawnParent); p.gameObject.SetActive(false); },
                p => p.gameObject.activeInHierarchy);

            if (spawnOnStart)
            {
                StartCoroutine(SpawnProjectile(0));
            }
            else
            {
                _spawning = true;
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogException(ex);
            Destroy(this);
        }
    }

    private void Update()
    {
        if (_spawning)
        {
            StartCoroutine(SpawnProjectile(spawnDelay));
        }
    }

    private IEnumerator SpawnProjectile(float seconds)
    {
        _spawning = false;

        yield return new WaitForSeconds(seconds);

        try
        {
            _projectilePool.ActivateNext(projectile =>
            {
                projectile.gameObject.SetActive(true);
                projectile.transform.SetPositionAndRotation(
                    transform.position,
                    Quaternion.Euler(projectile.transform.rotation.eulerAngles + (flipX ? -1 : 1) * launchAngle * Vector3.forward));
                projectile.Body.AddForce(LaunchDirection * launchForce, ForceMode2D.Impulse);
                projectile.Despawn(lifeTime);
            });
        }
        catch (System.Exception ex)
        {
            Debug.LogException(ex);
        }

        _spawning = true;
    }

    // Gizmos
    private void OnDrawGizmosSelected()
    {
        var force = LaunchDirection * launchForce;
        Vector2 velocity = (projectile != null)
            ? force / projectile.Body.mass
            : force;

        Gizmos.color = _color;
        // Direction
        if (_showLaunchVelocity)
        {
            Gizmos.DrawRay(transform.position, velocity);
        }
        // Trajectory
        if (_showTrajectory)
        {
            float timeStep = lifeTime * (1f / _resolution);
            var previousPosition = Projectile.ProjectileMotion(0, velocity, transform.position, projectile);
            for (int i = 1; i <= _resolution; i++)
            {
                var position = Projectile.ProjectileMotion(timeStep * i, velocity, transform.position, projectile);
                Gizmos.DrawLine(previousPosition, position);
                previousPosition = position;
            }
        }

        if (_showFinalPosition)
        {
            Gizmos.DrawWireSphere(Projectile.ProjectileMotion(lifeTime, velocity, transform.position, projectile), _radius);
        }
    }

    [ContextMenu("Auto set maxProjectiles")]
    private void SetMax() => maxProjectiles = Mathf.CeilToInt(lifeTime / spawnDelay) + 1;
}
