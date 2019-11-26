using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicWeaponController : MonoBehaviour
{
    [SerializeField] protected Weapon weapon;
    [SerializeField] protected Animator weaponAnimator;
    protected WeaponState currentState;
    protected WeaponState previousState;
    protected int currentAmmo;
    protected bool fire, fireLock, fireWait, reload, reloadLock;
    
    protected float timeBetweenBullets = 1;
    protected void Awake()
    {
        timeBetweenBullets = 1 / weapon.FireRate;
        Debug.Log(timeBetweenBullets);
    }
    void Start()
    {

    }

    void FixedUpdate()
    {
        previousState = currentState;
        //TODO make this more generic
        fire = Input.GetMouseButton(0);
        reload = Input.GetKey(KeyCode.R);

        if(reload && !reloadLock && currentAmmo != weapon.AmmoCount)
        {
            Reload();
        }

        if (fire && currentAmmo > 0)
        {
            switch (weapon.FireMode)
            {
                case WeaponFireMode.Automatic:
                    if (currentState != WeaponState.Shooting)
                    {
                        StartCoroutine(AutomaticFire());
                        currentState = WeaponState.Shooting;
                    }
                    break;
                case WeaponFireMode.Single_Shot:
                    if (!fireLock && !fireWait)
                    {
                        fireLock = true;
                        StartCoroutine(SingleFire());
                        currentState = WeaponState.Shooting;
                    }
                    break;
                default:
                    break;
            }
        }
        else
        {
            fireLock = false;
            currentState = WeaponState.Idle;
            StartCoroutine(AutomaticFire());
        }
    }
    void Update()
    {
        // UpdateAnimator();
    }

    protected void Shoot()
    {
        Debug.Log("Fire!");
        reloadLock = false;
        currentAmmo--;
        weaponAnimator.SetTrigger("Shoot");
    }
    protected void Reload()
    {
        reloadLock = true;
        weaponAnimator.SetTrigger("Reload");
        currentState = WeaponState.Reloading;
    }
    protected void ReloadByAnimator()
    {
        currentState = WeaponState.Idle;
        currentAmmo = weapon.AmmoCount;
        reloadLock = false;
    }
    private IEnumerator AutomaticFire()
    {
        if (fire && currentAmmo > 0)
        {
            Shoot();
            yield return new WaitForSecondsRealtime(timeBetweenBullets);
            StartCoroutine(AutomaticFire());
        }
    }
    private IEnumerator SingleFire()
    {
        fireWait = true;
        Shoot();
        yield return new WaitForSecondsRealtime(timeBetweenBullets);
        fireWait = false;
    }

    protected void UpdateAnimator()
    {

        weaponAnimator.SetBool("Shoot", currentState == WeaponState.Shooting);

    }
}

public enum WeaponState
{
    Idle, Shooting, Reloading, Cocking
}
