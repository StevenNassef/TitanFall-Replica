using UnityEngine;
using MyBox;

[System.Serializable]
[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Basic Weapon")]
public class Weapon : ScriptableObject
{

    [Header("Weapon Stats")]
    [SerializeField] private string weaponName;
    [Tooltip("The amount of damage each bullet deals.")]
    [SerializeField] private float damagePerBullet = 5;
    [Tooltip("How many bullets are fired per second.")]
    [SerializeField] private WeaponFireMode fireMode;
    [Tooltip("Whether this weapon shots Projectiles or Just uses RayCast, If Projectile type was choosen the weaponProjectile field must be set.")]
    [SerializeField] private float fireRate = 1;

    [Tooltip("The maximum amount of bullets a magazine can hold.")]
    [SerializeField] private int ammoCount = 30;
    [Tooltip("The distance the bullets is effective for, after which no damage is dealt.")]
    [SerializeField] private float range = 50;
    [Tooltip("Automatic (fires by holding)/ Single-shot(Fires on singles press).")]
    [Header("Projectile")]
    [SerializeField] private WeaponShootingType shootingType;
    [ConditionalField(nameof(shootingType),inverse:false, WeaponShootingType.Projectile)]
    [Tooltip("The type of projectile the weapon shots, (ONLY used when shootingType is set to Projectile).")]
    [SerializeField] private WeaponProjectile weaponProjectile;


    public string WeaponName => weaponName;
    public WeaponFireMode FireMode => fireMode;
    public WeaponShootingType ShootingType => shootingType;
    public WeaponProjectile Projectile => weaponProjectile;
    public float Damage => damagePerBullet;
    public float FireRate => fireRate;
    public int AmmoCount => ammoCount;
    public float Range => range;
}

public enum WeaponFireMode
{
    Automatic, Single_Shot
}

public enum WeaponShootingType
{
    Projectile, Hit_Scan
}