using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolderController : MonoBehaviour
{
    [SerializeField] KeyCode weaponSwitchButton = KeyCode.Z;
    private bool switchButtonLock = false;
    private List<GameObject> currentWeapons;
    int currentWeaponIndex = 0;
    private CharacterStatsHandler statsHandler;
    public delegate void OnWeaponHolderEventArgs(ObjectType type);
    public event OnWeaponHolderEventArgs OnKillingEnemy;

    void Start()
    {
        statsHandler = GetComponentInParent<CharacterStatsHandler>();
        InitializeWeapons();
    }
    void Update()
    {
        if (Input.GetKey(weaponSwitchButton))
        {
            if (!switchButtonLock)
            {
                SwitchWeapon();
                switchButtonLock = true;
            }
        }
        else
        {
            switchButtonLock = false;
        }
    }

    public void EnemyKilled(ObjectType type)
    {
        if(OnKillingEnemy != null)
        {
            OnKillingEnemy.Invoke(type);
        }
    }

    public void SwitchWeapon()
    {
        LoadWeapon((currentWeaponIndex + 1) % currentWeapons.Count);
    }
    private void InitializeWeapons()
    {
        currentWeaponIndex = 0;
        currentWeapons = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            currentWeapons.Add(transform.GetChild(i).gameObject);
            transform.GetChild(i).gameObject.SetActive(false);
        }
        currentWeapons[currentWeaponIndex].SetActive(true);
    }

    private void LoadWeapon(int index)
    {

        foreach (GameObject weapon in currentWeapons)
        {
            weapon.GetComponent<BasicWeaponController>().UnequipWeapon();
        }
        currentWeaponIndex = index;
        currentWeapons[currentWeaponIndex].SetActive(true);
    }
}
