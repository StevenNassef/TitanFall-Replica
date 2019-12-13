using UnityEngine;
using UnityEngine.UI;
using MyBox;

[System.Serializable]
[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Basic Weapon")]
public class Weapon : ScriptableObject
{

    [Header("Weapon Stats")]
    [SerializeField] private Sprite weaponIcon;
    [SerializeField] private string weaponName = "weapon";
    [Tooltip("The amount of damage each bullet deals.")]
    // [ConditionalField(nameof(shootingType), inverse: true, WeaponShootingType.Projectile)]
    [SerializeField] private float damagePerBullet = 4;
    [Tooltip("Automatic (fires by holding)/ Single-shot(Fires on singles press).")]
    [SerializeField] private WeaponFireMode fireMode = WeaponFireMode.Automatic;
    [Tooltip("How many bullets are fired per second.")]
    [SerializeField] private float fireRate = 1;

    [Tooltip("The maximum amount of bullets a magazine can hold.")]
    [SerializeField] private int ammoCount = 30;
    // [ConditionalField(nameof(shootingType), inverse: true, WeaponShootingType.Projectile)]
    [Tooltip("The distance the bullets is effective for, after which no damage is dealt.")]
    [SerializeField] private float range = 50;
    [Header("Sound Effects")]
    [SerializeField] private WeaponSFXList _SFXList;
    [Header("Projectile")]
    [Tooltip("Whether this weapon shots Projectiles or Just uses RayCast, If Projectile type was choosen the weaponProjectile field must be set.")]
    [SerializeField] private WeaponShootingType shootingType = WeaponShootingType.RayCast;
    
    [Tooltip("The type of projectile the weapon shots, (ONLY used when shootingType is set to Projectile).")]
    [SerializeField] private GameObject weaponProjectilePrefab;
    [SerializeField] private WeaponProjectile projectile;


    public Sprite WeaponIcon => weaponIcon;
    public string WeaponName => weaponName;
    public WeaponFireMode FireMode => fireMode;
    public WeaponShootingType ShootingType => shootingType;
    public GameObject ProjectilePrefab => weaponProjectilePrefab;
    public WeaponProjectile Projectile => projectile;
    public float Damage => damagePerBullet;
    public float FireRate => fireRate;
    public int AmmoCount => ammoCount;
    public float Range => range;
    public WeaponSFXList SFXList => _SFXList;
}

public enum WeaponFireMode
{
    Automatic, Single_Shot
}

public enum WeaponShootingType
{
    Projectile, RayCast
}
[System.Serializable]
public struct WeaponSFXList
{
    [SerializeField] private AudioClip cocking;
    [SerializeField] private AudioClip shooting;
    [SerializeField] private AudioClip reloading;
    [SerializeField] private AudioClip idle;

    public AudioClip Cocking => cocking;
    public AudioClip Shooting => shooting;
    public AudioClip Reloading => reloading;
    public AudioClip Idle => idle;
}