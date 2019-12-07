using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
public class CharacterStatsHandler : StatsHandler
{
    [Header("Core Ability")]
    [ReadOnly] [SerializeField] protected float currentCorePoints;
    [SerializeField] protected float maxCorePoints;
    [SerializeField] protected float pointsForPilotKill = 10f;
    [SerializeField] protected float pointForTitanKill = 50f;
    [SerializeField] protected KeyCode coreAbilityKey =  KeyCode.V;
    protected bool coreActivated;

    public event OnStatsHandlerEvent OnCoreAvailable;
    public event OnStatsHandlerEvent OnCoreActivated;
    public event OnStatsHandlerEvent OnCoreDeactivated;
    public bool isCoreActivated => coreActivated;
    protected override void InitializeHandler()
    {
        base.InitializeHandler();
        currentCorePoints = 0;
    }

    protected override void Update()
    {
        base.Update();

        if(Input.GetKey(coreAbilityKey) && currentCorePoints >= maxCorePoints)
        {
            StartCoreAbility();
        }
    }

    protected void UpdateCore(ObjectType type)
    {
        //if the core ability is activated or the bar is at max return
        if(coreActivated || currentCorePoints >= maxCorePoints)
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
        if(currentCorePoints >= maxCorePoints)
        {
            currentCorePoints = maxCorePoints;

            //Invoke the OnCoreAvailable to notify listeners
            if(OnCoreAvailable != null)
            {
                OnCoreAvailable.Invoke();
            }
        }
    }

    protected virtual void CoreAbilityLogic()
    {
        Debug.Log("CORE ABILITY INITIATED!");
    }

    private void StartCoreAbility()
    {
        if(currentCorePoints >= maxCorePoints)
        {
            currentCorePoints = 0;
            coreActivated = true;

            if(OnCoreActivated != null)
            {
                OnCoreActivated.Invoke();
            }
            CoreAbilityLogic();
        }
    }
}
