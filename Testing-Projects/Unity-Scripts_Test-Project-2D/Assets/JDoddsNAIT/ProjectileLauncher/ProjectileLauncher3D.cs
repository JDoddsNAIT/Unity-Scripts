﻿using UnityEngine;

public class ProjectileLauncher3D : ProjectileLauncher<Rigidbody>
{
    protected override Vector3 LaunchDirection => transform.forward;

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
                    p.AddForce(LaunchDirection * launchForce, ForceMode.Impulse);
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
            float timeStep = lifeTime * (1f / _resolution);
            Vector3 previousPosition = ProjectileMotion3D(projectile, 0, velocity, transform.position);
            for (int i = 1; i <= _resolution; i++)
            {
                Vector3 position = ProjectileMotion3D(projectile, timeStep * i, velocity, transform.position);
                Gizmos.DrawLine(previousPosition, position);
                previousPosition = position;
            }
        }

        if (_showFinalPosition)
        {
            Gizmos.DrawWireSphere(ProjectileMotion3D(projectile, lifeTime, velocity, transform.position), _radius);
        }
    }
    #endregion
}
