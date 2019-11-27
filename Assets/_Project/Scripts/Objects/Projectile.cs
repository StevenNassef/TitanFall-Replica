using UnityEngine;
using MyBox;
[CreateAssetMenu(fileName = "Projectile", menuName = "Projectiles/Basic Projectile")]
public class WeaponProjectile : ScriptableObject
{
    [SerializeField] private ProjectileType projectileType;

    [Header("Flying Settings")]
    [SerializeField] private float flyingSpeed;
    
    [Header("Damage Settings")]
    [SerializeField] private float damge;
    [SerializeField] private float damangeRange;
}


public enum ProjectileType
{
    Straight, Curve
}