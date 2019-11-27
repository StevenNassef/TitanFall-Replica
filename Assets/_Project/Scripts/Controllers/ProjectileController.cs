﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class ProjectileController : MonoBehaviour
{
    private AudioSource audioSource;
    private Rigidbody body;
    [SerializeField] private WeaponProjectile projectile;
    [SerializeField] private GameObject projectileGFX;
    [SerializeField] private GameObject explisionEffect;
    [SerializeField] private GameObject flyEffect;

    [ReadOnly] private bool fired;
    [ReadOnly] private bool exploded;
    protected void Awake()
    {
        body = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        InitializeProjectile();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateRotation();
    }
    [ButtonMethod]
    protected void InitializeProjectile()
    {
        body.useGravity = false;
        fired = false;
        exploded = false;
        projectileGFX.SetActive(true);
        body.WakeUp();
        flyEffect.SetActive(false);
    }

    protected void UpdateRotation()
    {
        if (body.velocity.sqrMagnitude > 0.2f)
        {
            transform.rotation = Quaternion.LookRotation(body.velocity.normalized);
        }
    }

    [ButtonMethod]
    private void Fire()
    {
        Fire(transform.forward);
    }
    protected void Fire(Vector3 direction)
    {
        if (fired)
            return;

        //TODO : make flying effect
        flyEffect.SetActive(true);
        //TODO : make flying sound

        if (projectile.ProjectileType == ProjectileType.Curve)
            body.useGravity = true;
        body.AddForce(direction.normalized * projectile.FlyingSpeed, ForceMode.Impulse);
        fired = true;
    }

    protected void Explode()
    {
        //Disabling mesh colliders and rigidbody compoenet of the projectile
        projectileGFX.SetActive(false);
        body.Sleep();
        flyEffect.SetActive(false);

        //Play the explosion effect
        explisionEffect.SetActive(true);

        Collider[] colliders = Physics.OverlapSphere(transform.position, projectile.DamangeRange, projectile.HitMask);

        if (colliders.Length > 0)
        {
            Debug.Log("FoundColliders");
            foreach (Collider collider in colliders)
            {
                //Deal damage to the object if it has a damage handler component
                DamageHandler damageHandler = collider.gameObject.GetComponent<DamageHandler>();
                if (damageHandler != null)
                {
                    damageHandler.TakeDamage(projectile.Damage);
                }

                //Add an explosive force to the objects if they had a rigidbody Component
                Rigidbody body = collider.gameObject.GetComponent<Rigidbody>();
                if (body != null)
                {
                    body.AddExplosionForce(projectile.ExplosiveForce, transform.position, projectile.DamangeRange, 2, ForceMode.Impulse);
                }

            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (fired && !exploded)
        {
            exploded = true;
            Explode();
        }
    }
}