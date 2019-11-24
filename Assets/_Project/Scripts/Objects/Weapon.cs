using UnityEngine;

[System.Serializable]
public class Weapon
{

    [SerializeField] private string weaponName;
    [Tooltip("Automatic (fires by holding)/ Single-shot(Fires on singles press).")]
    [SerializeField] private WeaponFireMode fireMode;
    [Tooltip("Whether this weapon shots Projectiles or Just uses RayCast, If Projectile type was choosen the weaponProjectile field must be set.")]
    [SerializeField] private WeaponShootingType shootingType;
    [Tooltip("The type of projectile the weapon shots, (ONLY used when shootingType is set to Projectile).")]
    [SerializeField] private WeaponProjectile weaponProjectile;
    [Tooltip("The amount of damage each bullet deals.")]
    [SerializeField] private float damagePerBullet;
    [Tooltip("How many bullets are fired per second.")]
    [SerializeField] private float fireRate;

    [Tooltip("The maximum amount of bullets a magazine can hold.")]
    [SerializeField] private int ammoCount;
    [Tooltip("The distance the bullets is effective for, after which no damage is dealt.")]
    [SerializeField] private float range;
}

public abstract class WeaponProjectile : MonoBehaviour
{
    // public virtual void ShotProjectile();

}

public enum WeaponFireMode
{
    Automatic, Single_Shot
}

public enum WeaponShootingType
{
    Projectile, Hit_Scan
}