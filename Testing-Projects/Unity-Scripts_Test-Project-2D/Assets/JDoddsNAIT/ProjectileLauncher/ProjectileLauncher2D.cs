using UnityEngine;

public class ProjectileLauncher2D : ProjectileLauncher<Rigidbody2D>
{
    protected override Vector3 LaunchDirection => transform.right;

    private void Start()
    {
        try
        {
            if (projectile == null)
            {
                throw new System.ArgumentNullException(nameof(projectile));
            }

            _projectilePool = new(
                size: maxProjectiles,
                initialize: p => { p = Instantiate(projectile, spawnParent); p.gameObject.SetActive(false); return p; },
                activeCriteria: p => p.gameObject.activeInHierarchy)
            {
                Activate = p =>
                {
                    p.gameObject.SetActive(true);
                    p.transform.SetPositionAndRotation(
                        transform.position,
                        Quaternion.Euler(p.transform.rotation.eulerAngles + transform.rotation.eulerAngles));
                    p.AddForce(LaunchDirection * launchForce, ForceMode2D.Impulse);
                },
                Deactivate = p =>
                {
                    p.Sleep();
                    p.gameObject.SetActive(false);
                }
            };

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

    [ContextMenu("Auto set maxProjectiles")]
    public void SetMax() => maxProjectiles = Mathf.CeilToInt(lifeTime / spawnDelay) + 1;

    #region Gizmos
    private void OnDrawGizmosSelected()
    {
        Vector3 force = LaunchDirection * launchForce;
        Vector3 velocity = (projectile != null)
            ? force / projectile.mass
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
            _pen.Resolution = _resolution;
            _pen.DrawSpline(t => ProjectileMotion2D(projectile, lifeTime * t, velocity, transform.position));
        }

        if (_showFinalPosition)
        {
            Gizmos.DrawWireSphere(ProjectileMotion2D(projectile, lifeTime, velocity, transform.position), _radius);
        }
    }
    #endregion
}
