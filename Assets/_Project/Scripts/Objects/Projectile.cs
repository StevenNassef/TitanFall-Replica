using UnityEngine;
using MyBox;
[CreateAssetMenu(fileName = "Projectile", menuName = "Projectiles/Basic Projectile")]
[System.Serializable]
public class WeaponProjectile : ScriptableObject
{
    [SerializeField] private ProjectileType projectileType = ProjectileType.Straight;

    [Header("Flying Settings")]
    [SerializeField] private float flyingSpeed = 11;

    [Header("Damage Settings")]
    [SerializeField] private float damge = 10;
    [SerializeField] private float damangeRange = 4;
    [SerializeField] private float explosiveForce = 10;
    [SerializeField] private LayerMask hitMask = new LayerMask();
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