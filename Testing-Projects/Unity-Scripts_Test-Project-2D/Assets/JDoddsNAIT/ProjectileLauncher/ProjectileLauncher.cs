﻿using System.Collections;
using System.Linq;
using UnityEngine;

public abstract class ProjectileLauncher<TBody> : MonoBehaviour
{
    #region Inspector
    [SerializeReference] public TBody projectile;
    [Min(1)] public int maxProjectiles = 10;
    [Tooltip("Time in seconds until the projectile de-spawns.")]
    [Min(0)] public float lifeTime = 1.15f;

    [SerializeReference] public Transform spawnParent;
    public bool spawnOnStart = false;
    [Min(0.01f)] public float spawnDelay = 1.15f;
    [Min(0)] public float launchForce = 8.0f;

    [SerializeField] protected Color color = Color.yellow;
    [Space]
    [SerializeField] public bool showLaunchVelocity = false;
    [Space]
    [SerializeField] public bool showTrajectory = true;
    [SerializeField, Range(1, 100)] protected int resolution = 25;
    [Space]
    [SerializeField] public bool showFinalPosition = true;
    [SerializeField, Range(0, 1)] protected float radius = 0.2f;
    #endregion

    protected GizmoPen _pen = new();

    protected abstract Vector3 LaunchDirection { get; }

    protected ObjectPool<TBody> _projectilePool;
    protected bool _spawning;

    public IEnumerator SpawnProjectile(float seconds)
    {
        TBody projectile = _projectilePool.NextInactive;

        _spawning = false;
        yield return new WaitForSeconds(seconds);
        _spawning = true;

        if (projectile != null)
        {
            _projectilePool.Activate(projectile);
            yield return new WaitForSeconds(lifeTime);
            _projectilePool.Deactivate(projectile);
        }
    }

    public static Vector3 ProjectileMotion3D(Rigidbody projectile, float t, Vector3 vi, Vector3 di)
    {
        Vector3 F = Vector3.zero;
        Vector3 G = Physics.gravity;

        if (projectile != null)
        {
            var forces = projectile.GetComponents<ConstantForce>();
            F = new Vector3(
                forces.Sum(f => f.force.x) + forces.Sum(f => f.relativeForce.x),
                forces.Sum(f => f.force.y) + forces.Sum(f => f.relativeForce.y),
                forces.Sum(f => f.force.z) + forces.Sum(f => f.relativeForce.z)) / projectile.mass;
        }

        Vector3 A = G + F;
        return (t * t * 0.5f * A) + (vi * t) + di;
    }

    public static Vector2 ProjectileMotion2D(Rigidbody2D projectile2D, float t, Vector2 vi, Vector2 di)
    {
        Vector2 F = Vector2.zero;
        Vector2 G = Physics2D.gravity;

        if (projectile2D != null)
        {
            var forces = projectile2D.GetComponents<ConstantForce2D>();
            F = new Vector2(
                forces.Sum(f => f.force.x) + forces.Sum(f => f.relativeForce.x),
                forces.Sum(f => f.force.y) + forces.Sum(f => f.relativeForce.y)) / projectile2D.mass;
            G *= projectile2D.gravityScale;
        }

        Vector2 A = G + F;
        return (t * t * 0.5f * A) + (vi * t) + di;
    }
}
