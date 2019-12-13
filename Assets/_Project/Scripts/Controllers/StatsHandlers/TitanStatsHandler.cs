using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TitanFirstPersonController))]
public class TitanStatsHandler : CharacterStatsHandler
{

    protected TitanFirstPersonController controller;
    protected override void InitializeHandler()
    {
        base.InitializeHandler();
        type = ObjectType.Titan;
        controller = GetComponent<TitanFirstPersonController>();
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

    
    protected void InformHudDefensiveAbilityCoolDownValue(int coolDownTimer) {
        // TODO
        base.titanHudController.DefensiveAbilityTimerValue = coolDownTimer;
    }
    
}
