using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
public class CharacterStatsHandler : StatsHandler
{
    [MustBeAssigned]
    [SerializeField]
    protected PilotHudController pilotHudController;
    [MustBeAssigned]
    [SerializeField]
    protected TitanHudController titanHudController;
    [MustBeAssigned]
    [SerializeField]
    protected WeaponHolderController weaponHolder;
    [MustBeAssigned]
    [SerializeField]
    protected BasicFirstPersonController fpsController;
    [Header("Camera")]
    [SerializeField] private Camera fpsCamera;

    [Header("Core Ability")]
    [ReadOnly] [SerializeField] protected float currentCorePoints;
    [SerializeField] protected float maxCorePoints;
    [SerializeField] protected float pointsForPilotKill = 10f;
    [SerializeField] protected float pointForTitanKill = 50f;
    [SerializeField] protected KeyCode coreAbilityKey = KeyCode.V;
    protected bool coreActivated;
    public event OnStatsHandlerEvent OnCoreAvailable;
    public event OnStatsHandlerEvent OnCoreActivated;
    public event OnStatsHandlerEvent OnCoreDeactivated;
    public bool isCoreActivated => coreActivated;
    public Camera FPSCamera => fpsCamera;
    protected override void InitializeHandler()
    {
        base.InitializeHandler();
        currentCorePoints = 0;
        // weaponHolder = GetComponentInChildren<WeaponHolderController>();

        if (weaponHolder != null)
        {
            weaponHolder.OnKillingEnemy += UpdateCore;
        }

        // this.OnCoreActivated += InformHudCoreAbility;
        // this.OnCoreDeactivated += InformHudCoreAbility;
        // this.OnCoreAvailable += InformHudCoreAbility;
        OnDamageTaken += InformHudHp;
    }

    protected override void Update()
    {
        base.Update();

        CoreAbilityCheck();
    }

    protected virtual void CoreAbilityCheck()
    {
        if (Input.GetKey(coreAbilityKey) && currentCorePoints >= maxCorePoints)
        {
            StartCoreAbility();
        }
    }

    protected void UpdateCore(ObjectType type)
    {
        //if the core ability is activated or the bar is at max return
        if (coreActivated || currentCorePoints >= maxCorePoints)
            return;

        switch (type)
        {
            case ObjectType.Pilot:
                currentCorePoints += pointsForPilotKill;
                break;
            case ObjectType.Titan:
                currentCorePoints += pointForTitanKill;
                break;
            default:
                break;
        }
        if (currentCorePoints >= maxCorePoints)
        {
            currentCorePoints = maxCorePoints;
            InformHudCoreAbility();
            
            playCoreReadySounds();

            //Invoke the OnCoreAvailable to notify listeners
            if (OnCoreAvailable != null)
            {
                OnCoreAvailable.Invoke();
            }
        }
    }

    protected virtual void CoreAbilityLogic()
    {
        Debug.Log("CORE ABILITY INITIATED!");
    }

    protected void StartCoreAbility()
    {
        if (currentCorePoints >= maxCorePoints)
        {
            currentCorePoints = 0;
            coreActivated = true;

            playCoreFireSounds();

            if (OnCoreActivated != null)
            {
                OnCoreActivated.Invoke();
            }
            CoreAbilityLogic();
        }
    }

    protected virtual void playCoreFireSounds() {

    }

    protected virtual void playCoreReadySounds() {

    }

    protected virtual void InformHudCoreAbility() {

    }

    protected virtual void InformHudHp() {
              
    }
    
}
