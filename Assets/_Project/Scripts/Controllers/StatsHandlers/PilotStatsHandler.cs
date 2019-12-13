using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
public class PilotStatsHandler : CharacterStatsHandler
{
    [SerializeField] private float titanSpwanMaxDistance = 10;
    [SerializeField] private float maxSurfaceAngle = 40;
    [SerializeField] private GameObject playerTitan;

    public event OnStatsHandlerEvent OnHealing;

    //the to wait after the last taken damage to start regenrating health.
    private float timeToRegenHealth = 3;

    // the value that the autoregen will add to the health every cycle.
    private float regenValue = 5f;

    //the time between each regen cycle in seconds
    private float regenCycleTime = 1f;
    private bool regenrating;
    private Vector2 screenCenter;
    private RaycastHit currentTitanFallHit;
    protected override void InitializeHandler()
    {
        base.InitializeHandler();
        type = ObjectType.Pilot;
        screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        OnHealing += InformHudHp;
    }

    protected override void Update()
    {
        base.Update();
    }
    
    private void OnEnable()
    {
        OnDamageTaken += UpdateHealth;
    }
    private void OnDisable()
    {
        OnDamageTaken -= UpdateHealth;
    }
    private void UpdateHealth()
    {
        StopAllCoroutines();
        if (currentHealthPoints < maxHealthPoints)
        {
            Debug.Log("Wait to Regen");
            regenrating = true;
            StartCoroutine(WaitToRegen());
        }
    }

    private IEnumerator WaitToRegen()
    {
        yield return new WaitForSeconds(timeToRegenHealth);
        StartCoroutine(RegenrateHealth());
    }
    protected override void CoreAbilityCheck()
    {
        if (Input.GetKey(coreAbilityKey) && currentCorePoints >= maxCorePoints)
        {
            Ray ray = FPSCamera.ScreenPointToRay(screenCenter);
            RaycastHit hitInfo;
            Debug.DrawRay(ray.origin, ray.direction * titanSpwanMaxDistance, Color.yellow, 0.1f);
            if (Physics.Raycast(ray, out hitInfo, titanSpwanMaxDistance))
            {
                if (Mathf.Abs(Vector3.Angle(hitInfo.normal, Vector3.up)) < maxSurfaceAngle)
                {
                    currentTitanFallHit = hitInfo;
                    StartCoreAbility();
                }
                else
                {
                    //TODO : Can't Make TitanFall Here
                }
            }
        }
    }
    protected override void CoreAbilityLogic()
    {
        playerTitan.transform.position = currentTitanFallHit.point;
        playerTitan.SetActive(true);
        InformHudTitanSelection();
    }

    private IEnumerator RegenrateHealth()
    {
        if (regenrating)
        {
            //Call OnHealing Event to notify listners
            // TODO : make UI Listen to this event
            if (OnHealing != null)
            {
                OnHealing.Invoke();
            }
            currentHealthPoints += regenValue;
            if (currentHealthPoints >= maxHealthPoints)
            {
                currentHealthPoints = maxHealthPoints;
                regenrating = false;
            }
            yield return new WaitForSeconds(regenCycleTime);
            StartCoroutine(RegenrateHealth());
        }
    }

    protected override void HitEffect(float damage)
    {
        // TODO: make an effect for the pilot
    }

    protected override void DestroyObject()
    {
        StopAllCoroutines();
        regenrating = false;
        Debug.Log("Game Over!");
        // TODO: make GameOver Logic
    }


    protected void InformHudTitanSelection() {
        base.titanHudController.enabled = true;
        base.titanHudController.ResetValues();
        base.titanHudController.SetCoreAbilityIcon(GameInitializations.Titan.TitanIcon);
        base.pilotHudController.enabled = false;
    }

    protected override void InformHudCoreAbility() {
        // Debug.Log("TITAN VALUE SHOULD GET UPDATED");
        base.pilotHudController.TitanfallValue =
                    (currentCorePoints/maxCorePoints)*100.0f;
    }

    protected override void InformHudHp() {
        base.pilotHudController.HpValue =
            (base.currentHealthPoints/base.maxHealthPoints)*100.0f;
    }

}
