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
    protected bool fire;
    protected bool fireLock;
    protected bool fireWait;
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

        if (fire)
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
        weaponAnimator.SetTrigger("Shoot");
    }

    private IEnumerator AutomaticFire()
    {
        if (fire)
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
