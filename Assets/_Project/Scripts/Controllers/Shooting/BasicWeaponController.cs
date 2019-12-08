using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class BasicWeaponController : MonoBehaviour
{
    [SerializeField] protected Weapon weapon;
    protected Animator weaponAnimator;
    [Header("SFX")]
    protected AudioSource audioSource;
    protected WeaponState currentState;
    protected WeaponState previousState;
    protected int currentAmmo;
    protected bool fire, fireLock, fireWait, reload, reloadLock, currentlyAiming, previouslyAiming;
    protected WeaponHolderController weaponHolder;
    protected float timeBetweenBullets = 1;
    protected Vector3 screenCenter;
    protected float lastDisableTime;
    protected void Awake()
    {
        timeBetweenBullets = 1 / weapon.FireRate;
        currentAmmo = weapon.AmmoCount;
        screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        audioSource = GetComponent<AudioSource>();
        weaponAnimator = GetComponent<Animator>();
        weaponHolder = GetComponentInParent<WeaponHolderController>();
    }
    protected virtual void Start()
    {

    }

    protected void FixedUpdate()
    {
        previousState = currentState;
        previouslyAiming = currentlyAiming;
        //TODO make this more generic
        fire = Input.GetMouseButton(0);
        currentlyAiming = Input.GetMouseButton(1);
        reload = Input.GetKey(KeyCode.R);

        if (reload && !reloadLock && currentAmmo != weapon.AmmoCount)
        {
            currentlyAiming = false;
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

        if (currentlyAiming != previouslyAiming)
        {
            weaponAnimator.SetBool("Aim", currentlyAiming);
        }
    }
    void Update()
    {
        // UpdateAnimator();
    }
    protected void KillRewardHandler(ObjectType type)
    {
        Debug.Log(type);
        if (weaponHolder != null)
        {
            weaponHolder.EnemyKilled(type);
        }
        else
        {
            Debug.LogError("NO WeaponHolder Found!");
        }
    }

    protected virtual void Shoot()
    {
        Debug.Log("Fire!");
        reloadLock = false;
        currentAmmo--;
        weaponAnimator.SetTrigger("Shoot");
    }
    protected virtual void Reload()
    {
        reloadLock = true;
        currentState = WeaponState.Reloading;
        weaponAnimator.SetTrigger("Reload");
    }
    protected virtual void ReloadByAnimator()
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

    private IEnumerator EnableFiringAfterSeconds(float timeToWait)
    {
        yield return new WaitForSecondsRealtime(timeToWait);
        fireWait = false;
    }

    protected void UpdateAnimator()
    {

    }

    protected void OnEnable()
    {
        if (fireWait)
        {
            float delta = Time.timeSinceLevelLoad - lastDisableTime;
            if (delta > timeBetweenBullets)
            {
                fireWait = false;
            }
            else
            {
                StartCoroutine(EnableFiringAfterSeconds(timeBetweenBullets - delta));
            }
        }
    }

    protected void OnDisable()
    {
        if (fireWait)
        {
            lastDisableTime = Time.timeSinceLevelLoad;
        }
    }

    protected void PlaySFX()
    {
        switch (currentState)
        {
            case WeaponState.Cocking:
                audioSource.clip = weapon.SFXList.Cocking;
                break;
            case WeaponState.Idle:
                audioSource.clip = weapon.SFXList.Idle;
                break;
            case WeaponState.Reloading:
                audioSource.clip = weapon.SFXList.Reloading;
                break;
            case WeaponState.Shooting:
                audioSource.clip = weapon.SFXList.Shooting;
                break;
            default:
                break;
        }
        audioSource.Play();


    }

    protected void PlayStateSFX(WeaponState state)
    {
        switch (state)
        {
            case WeaponState.Cocking:
                audioSource.clip = weapon.SFXList.Cocking;
                break;
            case WeaponState.Idle:
                audioSource.clip = weapon.SFXList.Idle;
                break;
            case WeaponState.Reloading:
                audioSource.clip = weapon.SFXList.Reloading;
                break;
            case WeaponState.Shooting:
                audioSource.clip = weapon.SFXList.Shooting;
                break;
            default:
                break;
        }
        audioSource.Play();
    }
}

public enum WeaponState
{
    Cocking, Shooting, Reloading, Idle
}
