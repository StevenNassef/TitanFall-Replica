using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TitanFirstPersonController))]
public class TitanStatsHandler : CharacterStatsHandler
{
    private float lastDefensiveTime;
    private float maxCoolDownDefensiveTime = 15.0f;
    protected TitanFirstPersonController controller;
    protected override void InitializeHandler()
    {
        base.InitializeHandler();
        type = ObjectType.Titan;
        controller = GetComponent<TitanFirstPersonController>();
    }

    protected override void playCoreFireSounds() {
        controller.titanSounds.playCoreFire();
    }

    protected override void playCoreReadySounds() {
        controller.titanSounds.playCoreReady();
    }

    protected override float CalculateDamage(float damage, ObjectType type)
    {
        //ignore all damage when the titan is dashing
        if(controller.Dashing)
        {
            return 0;
        }

        return damage;
    }

    protected override void InformHudCoreAbility() {
        base.titanHudController.CoreAbilityBarValue =
                    (currentCorePoints/maxCorePoints)*100.0f;
    }

    protected override void InformHudHp() {
        base.titanHudController.HpValue =
            (base.currentHealthPoints/base.maxHealthPoints)*100.0f;
        if (base.currentHealthPoints <= 0) {
            base.pilotHudController.enabled = true;
            base.titanHudController.enabled = false;
        }
    }

    
    protected void InformHudDefensiveAbilityCoolDownValue(float coolDownTimer) {
        // TODO
        base.titanHudController.DefensiveAbilityTimerValue = coolDownTimer;
    }

    protected void TryToStartDefensiveAbility() {

        float currentTime = Time.time;

        if (currentTime - lastDefensiveTime >= maxCoolDownDefensiveTime) {
            InformHudDefensiveAbilityCoolDownValue(maxCoolDownDefensiveTime);
            if(Input.GetKey(KeyCode.V)) {
                InformHudDefensiveAbilityCoolDownValue(0);
                lastDefensiveTime = currentTime;
            }            
        } else if (currentTime - lastDefensiveTime < maxCoolDownDefensiveTime) {
            InformHudDefensiveAbilityCoolDownValue(currentTime - lastDefensiveTime);
        }
    }

    protected override void Update() {
        base.Update();
        TryToStartDefensiveAbility();
    }
}
