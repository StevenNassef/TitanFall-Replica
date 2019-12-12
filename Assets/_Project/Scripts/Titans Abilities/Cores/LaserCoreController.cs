using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
public class LaserCoreController : MonoBehaviour
{
    [SerializeField] private float maxDistance;
    [SerializeField] private float sphereCastRadius;
    [SerializeField] private float speed;
    [SerializeField] private LayerMask hitmask;
    [SerializeField] private float damage = 10000;
    [SerializeField] private GameObject laserCoreGFX;
    private float currentDistance;
    private Vector3 laserScale;
    [ReadOnly] public Vector3 hitPoint;
    [ReadOnly] public string hitObjectName;
    private void Awake()
    {
        laserScale = new Vector3(sphereCastRadius * 2, sphereCastRadius * 2, currentDistance);
    }
    void Start()
    {

    }

    void Update()
    {
        ShootLaser();
    }
    private void OnEnable()
    {
        currentDistance = 0;
    }
    private void ShootLaser()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, sphereCastRadius, transform.forward, out hit,
        currentDistance, hitmask, QueryTriggerInteraction.Ignore))
        {
            currentDistance = Mathf.Abs((transform.position - hit.point).magnitude);
            hitPoint = hit.point;
            hitObjectName = hit.collider.gameObject.name;

            StatsHandler handler = hit.collider.gameObject.GetComponent<StatsHandler>();
            if (handler != null)
                handler.TakeDamage(damage);
        }
        else
        {
            if (currentDistance < maxDistance)
            {
                currentDistance += Time.deltaTime * speed;
            }
        }
        laserScale.z = currentDistance;
        laserCoreGFX.transform.localScale = laserScale;
    }
}
