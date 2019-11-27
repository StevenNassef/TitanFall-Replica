using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
public class HeavyWeaponController : BasicWeaponController
{
    [SerializeField] private LayerMask hitmask;
    [SerializeField] private Transform barrelOpening;
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
        GameObject proj = Instantiate(weapon.Projectile, barrelOpening.position, Quaternion.LookRotation(ray.direction));
        proj.GetComponent<ProjectileController>().Fire(ray.direction);
        
    }
}
