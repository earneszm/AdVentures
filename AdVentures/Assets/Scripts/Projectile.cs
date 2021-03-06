﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : PooledMonoBehaviour
{
    public ProjectileUpgradeTypeEnum projectileType;

    [SerializeField]
    private float defaultForceAmount = 100f;

    private new Rigidbody2D rigidbody;
    private Vector2 direction = Vector2.up;

    public Rigidbody2D RigidBody { get { return rigidbody; } }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Launch(float? force = null)
    {
        rigidbody.AddForce(direction * (force ?? defaultForceAmount));
    }

    public void SetLaunchDirection(Vector2 direction)
    {
        this.direction = direction;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        var damageableObject = collider.gameObject.GetComponentInParent<ITakeDamage>();

        if (damageableObject != null)
            damageableObject.TakeDamage(1);

        ReturnToPool();
    }
}
