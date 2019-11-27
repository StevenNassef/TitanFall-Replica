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
    [SerializeField] protected List<AudioClip> SFXList;
    protected WeaponState currentState;
    protected WeaponState previousState;
    protected int currentAmmo;
    protected bool fire, fireLock, fireWait, reload, reloadLock;

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
    }
    protected virtual void Start()
    {

    }

    protected void FixedUpdate()
    {
        previousState = currentState;
        //TODO make this more generic
        fire = Input.GetMouseButton(0);
        reload = Input.GetKey(KeyCode.R);

        if (reload && !reloadLock && currentAmmo != weapon.AmmoCount)
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
        if (SFXList.Count > (int)currentState)
        {
            audioSource.clip = SFXList[(int)currentState];
            audioSource.Play();
        }
    }

    protected void PlayStateSFX(WeaponState state)
    {
        if (SFXList.Count > (int)state)
        {
            audioSource.clip = SFXList[(int)state];
            audioSource.Play();
        }
    }
}

public enum WeaponState
{
    Cocking, Shooting, Reloading, Idle
}
