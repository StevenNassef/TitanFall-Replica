using UnityEngine;
using MyBox;
[CreateAssetMenu(fileName = "Projectile", menuName = "Projectiles/Basic Projectile")]
[System.Serializable]
public class WeaponProjectile : ScriptableObject
{
    [SerializeField] private ProjectileType projectileType;

    [Header("Flying Settings")]
    [SerializeField] private float flyingSpeed;

    [Header("Damage Settings")]
    [SerializeField] private float damge;
    [SerializeField] private float damangeRange;
    [SerializeField] private float explosiveForce;
    [SerializeField] private LayerMask hitMask;
    [Header("Graphics and VFX")]
    [SerializeField] private GameObject _GFXPrefab;
    [SerializeField] private GameObject _ExplosionVFXPrefab;
    [SerializeField] private GameObject _FlyingVFXPrefab;

    public ProjectileType ProjectileType => projectileType;
    public float FlyingSpeed => flyingSpeed;
    public float Damage => damge;
    public float DamangeRange => damangeRange;
    public float ExplosiveForce => explosiveForce;
    public LayerMask HitMask => hitMask;
}


public enum ProjectileType
{
    Straight, Curve
}