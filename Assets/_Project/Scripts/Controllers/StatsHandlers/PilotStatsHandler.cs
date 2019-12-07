using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
public class PilotStatsHandler : CharacterStatsHandler
{
    public event OnStatsHandlerEvent OnHealing;

    //the to wait after the last taken damage to start regenrating health.
    private float timeToRegenHealth = 3;

    // the value that the autoregen will add to the health every cycle.
    private float regenValue = 5f;

    //the time between each regen cycle in seconds
    private float regenCycleTime = 1f;
    private bool regenrating;

    protected override void InitializeHandler()
    {
        base.InitializeHandler();
        type = ObjectType.Pilot;
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
}
