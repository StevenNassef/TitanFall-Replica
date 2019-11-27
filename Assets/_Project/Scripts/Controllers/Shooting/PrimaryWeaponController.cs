using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
public class PrimaryWeaponController : BasicWeaponController
{
    [SerializeField] private LayerMask hitmask;
    private Camera fpsCamera;
    protected override void Start()
    {
        base.Start();
        //TODO make this access the player controller instead.
        fpsCamera = RigidbodyFirstPersonController.instance.cam;
    }

    protected override void Shoot()
    {
        base.Shoot();
        Ray ray = fpsCamera.ScreenPointToRay(screenCenter);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, weapon.Range, hitmask))
        {
            DamageHandler handler = hitInfo.collider.GetComponent<DamageHandler>();

            if(handler)
            {
                //TODO add a parameter to deal damage to certain objects only
                handler.TakeDamage(weapon.Damage);
            }
        }
    }
}
