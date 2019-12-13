using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameInitializations
{
    private static Weapon primaryWeapon, heavyWeapon;
    private static OneTitan titan;
    private static float masterVolume;
    private static float musicVolume;
    private static float sfxVolume;
    private static bool firstLevelIsParkour;
    public static Weapon PrimaryWeapon 
    {
        get 
        {
            return primaryWeapon;
        }
        set 
        {
            primaryWeapon = value;
        }
    }

    public static Weapon HeavyWeapon 
    {
        get 
        {
            return heavyWeapon;
        }
        set 
        {
            heavyWeapon = value;
        }
    }

    public static OneTitan Titan 
    {
        get 
        {
            return titan;
        }
        set 
        {
            titan = value;
        }
    }

    public static float MasterVolume 
    {
        get 
        {
            return masterVolume;
        }
        set 
        {
            masterVolume = value;
        }
    }

    public static float MusicVolume 
    {
        get 
        {
            return musicVolume;
        }
        set 
        {
            musicVolume = value;
        }
    }

   public static float SfxVolume 
    {
        get 
        {
            return sfxVolume;
        }
        set 
        {
            sfxVolume = value;
        }
    }

    public static bool FirstLevelIsParkour
    {
        get
        {
            return firstLevelIsParkour;
        }
        set
        {
            firstLevelIsParkour = value;
        }
    }
}
