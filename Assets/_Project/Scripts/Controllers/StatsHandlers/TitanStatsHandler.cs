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
}
