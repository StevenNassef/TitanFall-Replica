using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
public class VortexSheildController : StatsHandler
{
    [SerializeField] WeaponHolderController weaponHolderController;
    [ReadOnly] private List<RayCastDamage> collectedHitScanDamages;
    [ReadOnly] private List<ProjectileController> collectedProjectiles;
    private float sheildTime = 5;
    protected override void InitializeHandler()
    {
        base.InitializeHandler();
        ResetSheild();
        type = ObjectType.Sheild;
        //TODO : get the weaponholder from parent
    }

    private void ResetSheild()
    {
        collectedHitScanDamages = new List<RayCastDamage>();
        collectedProjectiles = new List<ProjectileController>();
    }

    [ButtonMethod]
    protected void ShootCollected()
    {
        foreach (RayCastDamage castDamage in collectedHitScanDamages)
        {
            RaycastHit hitInfo;
            // Debug.DrawRay(ray.origin, ray.direction * weapon.Range, Color.red, 0.1f);
            if (Physics.Raycast(transform.position, transform.forward, out hitInfo, castDamage.Range))
            {
                StatsHandler handler = hitInfo.collider.GetComponent<StatsHandler>();

                if (handler)
                {
                    //TODO add a parameter to deal damage to certain objects only
                    ObjectType type;
                    if (handler.TakeDamage(castDamage, out type))
                    {
                        weaponHolderController.EnemyKilled(type);
                    }
                }

                //TODO : Apply Force on objects with rigidbody component
            }
        }

        foreach (ProjectileController controller in collectedProjectiles)
        {
            Debug.Log("Firing Projectile");
            controller.transform.Translate(transform.forward * 0.5f, Space.World);
            controller.transform.rotation = Quaternion.LookRotation(transform.forward, transform.up);
            controller.transform.parent = null;
            controller.Fire(transform.forward, weaponHolderController);
        }
        ResetSheild();
    }
    public override bool TakeDamage(RayCastDamage castDamage, out ObjectType type)
    {
        type = this.type;
        collectedHitScanDamages.Add(castDamage);
        return false;
    }
    public override bool TakeDamage(float damage, out ObjectType type)
    {
        type = this.type;
        return false;
    }
    private void OnTriggerEnter(Collider other)
    {
        ProjectileController projectileController = other.gameObject.GetComponent<ProjectileController>();
        if (projectileController != null && projectileController.Fired)
        {
            Debug.Log("FoundProjectile");
            projectileController.DisableProjectile();
            other.gameObject.transform.SetParent(transform);
            collectedProjectiles.Add(projectileController);
        }
    }
}
