using UnityEngine;

public class ProjectileLauncher2D : ProjectileLauncher<Rigidbody2D>
{
    [SerializeField, Range(-180, 180)] float launchAngle = 0f;

    protected override Vector3 LaunchDirection => Quaternion.Euler(0, 0, launchAngle) * transform.right;

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
                        Quaternion.Euler(p.transform.rotation.eulerAngles + transform.rotation.eulerAngles + new Vector3(0, 0, launchAngle)));
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

        // Direction
        if (showLaunchVelocity)
        {
            Gizmos.color = launchVelocityColor;
            Gizmos.DrawRay(transform.position, velocity);
        }
        // Trajectory
        if (showTrajectory)
        {
            Gizmos.color = trajectoryColor;
            float timeStep = lifeTime * (1f / trajectoryResolution);
            Vector3 previousPosition = ProjectileMotion2D(projectile, 0, velocity, transform.position);
            for (int i = 1; i <= trajectoryResolution; i++)
            {
                Vector3 position = ProjectileMotion2D(projectile, timeStep * i, velocity, transform.position);
                Gizmos.DrawLine(previousPosition, position);
                previousPosition = position;
            }
        }

        if (showFinalPosition)
        {
            Gizmos.color = finalPositionColor;
            Gizmos.DrawWireSphere(ProjectileMotion2D(projectile, lifeTime, velocity, transform.position), finalPositionRadius);
        }
    }
    #endregion
}
