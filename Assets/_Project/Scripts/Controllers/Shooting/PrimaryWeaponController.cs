// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityStandardAssets.Characters.FirstPerson;
// public class PrimaryWeaponController : BasicWeaponController
// {
//     [SerializeField] private LayerMask hitmask;
//     private Camera fpsCamera;
//     protected override void Start()
//     {
//         base.Start();
//         //TODO make this access the player controller instead.
//         fpsCamera = RigidbodyFirstPersonController.instance.cam;
//     }

//     protected override void Shoot()
//     {
//         base.Shoot();
//         Ray ray = fpsCamera.ScreenPointToRay(screenCenter);
//         RaycastHit hitInfo;
//         Debug.DrawRay(ray.origin, ray.direction * weapon.Range, Color.red, 0.1f);
//         if (Physics.Raycast(ray, out hitInfo, weapon.Range, hitmask))
//         {
//             StatsHandler handler = hitInfo.collider.GetComponent<StatsHandler>();

//             if(handler)
//             {
//                 //TODO add a parameter to deal damage to certain objects only
//                 ObjectType type;
//                 if(handler.TakeDamage(weapon.Damage, out type))
//                 {
//                     KillRewardHandler(type);
//                 }
//             }

//             //TODO : Apply Force on objects with rigidbody component
//         }
//     }

// }
