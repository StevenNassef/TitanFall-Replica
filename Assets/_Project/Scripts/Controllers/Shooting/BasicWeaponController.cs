using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using MyBox;
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class BasicWeaponController : MonoBehaviour
{
    public PilotHudController pilotHudController;
    [SerializeField] protected Weapon weapon;
    [SerializeField] private LayerMask hitmask;
    [SerializeField] private Transform barrelOpening;
    [SerializeField] private GameObject mazzleFlash;
    [SerializeField] private GameObject scope;
    protected Animator weaponAnimator;
    [Header("SFX")]
    protected AudioSource audioSource;
    protected WeaponState currentState;
    protected WeaponState previousState;
    protected int currentAmmo;
    protected bool fire, fireLock, fireWait, reload, reloadLock, currentlyAiming, previouslyAiming, aimLock;
    protected WeaponHolderController weaponHolder;
    protected float timeBetweenBullets = 1;
    protected Vector3 screenCenter;
    protected float lastDisableTime;
    private RayCastDamage castDamage;
    private Camera fpsCamera;
    protected void Awake()
    {
        timeBetweenBullets = 1 / weapon.FireRate;
        currentAmmo = weapon.AmmoCount;
        castDamage = new RayCastDamage(weapon.Damage, weapon.Range);
        screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        audioSource = GetComponent<AudioSource>();
        weaponAnimator = GetComponent<Animator>();
        weaponHolder = GetComponentInParent<WeaponHolderController>();
    }
    protected virtual void Start()
    {
        //TODO make this access the player controller instead.
        fpsCamera = weaponHolder.StatsHandler.FPSCamera;
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
        if (aimLock)
        {
            currentlyAiming = false;
        }

        if (currentlyAiming != previouslyAiming)
        {
            weaponAnimator.SetBool("Aim", currentlyAiming);
        }
        InformHudAmmoCount();
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
        // Debug.Log("Fire!");
        reloadLock = false;
        aimLock = false;
        currentAmmo--;
        weaponAnimator.SetTrigger("Shoot");

        //Play The MazzleFlash
        mazzleFlash.SetActive(false);
        mazzleFlash.SetActive(true);

        switch (weapon.ShootingType)
        {
            case WeaponShootingType.Projectile:
                ShootProjectile();
                break;
            case WeaponShootingType.RayCast:
                ShootRayCast();
                break;
            default:
                break;
        }
    }
    protected virtual void ShootProjectile()
    {
        Ray ray = fpsCamera.ScreenPointToRay(screenCenter);
        // Debug.Log(ray.direction);
        GameObject proj = Instantiate(weapon.ProjectilePrefab, barrelOpening.position, Quaternion.LookRotation(ray.direction));
        proj.GetComponent<ProjectileController>().Fire(ray.direction, weaponHolder, weapon.Projectile);

    }
    protected virtual void ShootRayCast()
    {
        Ray ray = fpsCamera.ScreenPointToRay(screenCenter);
        RaycastHit hitInfo;
        Debug.DrawRay(ray.origin, ray.direction * weapon.Range, Color.red, 0.1f);
        if (Physics.Raycast(ray, out hitInfo, weapon.Range, hitmask, QueryTriggerInteraction.Collide))
        {
            StatsHandler handler = hitInfo.collider.GetComponent<StatsHandler>();

            if (handler)
            {
                //TODO add a parameter to deal damage to certain objects only
                ObjectType type;
                if (handler.TakeDamage(castDamage, out type))
                {
                    KillRewardHandler(type);
                }
            }

            //TODO : Apply Force on objects with rigidbody component
        }
    }
    protected virtual void Reload()
    {
        reloadLock = true;
        aimLock = true;
        currentState = WeaponState.Reloading;
        weaponAnimator.SetTrigger("Reload");
    }
    protected virtual void ReloadByAnimator()
    {
        currentState = WeaponState.Idle;
        currentAmmo = weapon.AmmoCount;
        reloadLock = false;
        aimLock = false;
    }

    protected void OpenScope()
    {
        if (scope != null)
            scope.SetActive(true);
    }

    protected void CloseScope()
    {
        if (scope != null)
            scope.SetActive(false);
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
        InformHudWeaponSelection();
        CloseScope();
        aimLock = false;
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
    
    public void UnequipWeapon()
    {
        if(weaponAnimator != null)
            weaponAnimator.SetTrigger("Unequip");
    }
    protected void DisableWeapon()
    {
        gameObject.SetActive(false);
    }
    protected void OnDisable()
    {
        CloseScope();
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

    protected void InformHudAmmoCount() {
        pilotHudController.AmmoValue = currentAmmo;
    }

    protected void InformHudWeaponSelection() {
        pilotHudController.ChangeWeapon(weapon.WeaponIcon,
                                            weapon.WeaponName,
                                             weapon.AmmoCount,
                                             currentAmmo);
    }
}

public enum WeaponState
{
    Cocking, Shooting, Reloading, Idle
}
