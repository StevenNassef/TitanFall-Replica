using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(GameInitializations.PrimaryWeapon.WeaponName);
        Debug.Log(GameInitializations.MusicVolume);
        Debug.Log(GameInitializations.FirstLevelIsParkour);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
