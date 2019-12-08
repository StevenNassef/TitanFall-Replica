using UnityEngine;
using MyBox;
[System.Serializable]
public class WeaponProjectile
{
    [SerializeField] public ProjectileType projectileType = ProjectileType.Straight;

    [Header("Flying Settings")]
    [SerializeField] private float flyingSpeed = 11;
    [SerializeField] private float maxFlyingDistance = 50f;

    [Header("Damage Settings")]
    [SerializeField] private float damage = 125;
    [SerializeField] private float damangeRange = 4;
    [SerializeField] private float explosiveForce = 20;
    [SerializeField] private LayerMask hitMask = new LayerMask();
    [Header("Graphics and VFX")]
    [SerializeField] private GameObject _GFXPrefab;
    [SerializeField] private GameObject _ExplosionVFXPrefab;
    [SerializeField] private GameObject _FlyingVFXPrefab;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip _FlyingSFX;
    [SerializeField] private AudioClip _ExplosionSFX;
    public ProjectileType ProjectileType => projectileType;
    public float FlyingSpeed => flyingSpeed;
    public float MaxFlyingDistance => maxFlyingDistance;
    public float Damage => damage;
    public float DamangeRange => damangeRange;
    public float ExplosiveForce => explosiveForce;
    public LayerMask HitMask => hitMask;
    public AudioClip ExplosionSFX => _ExplosionSFX;
    public AudioClip FlyingSFX => _FlyingSFX;
}


public enum ProjectileType
{
    Straight, Curve
}