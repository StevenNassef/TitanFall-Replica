using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PilotHudController : MonoBehaviour
{
    [Header("Weapon Panel Settings")]
    public Weapon currentWeaponData;
    public Image weaponIcon;
    public Text weaponNameTxt;
    public Text ammoCountTxt;
    private int ammoMaxValue;

    // private int ammoMaxValue;
    // private int ammoValue = 2;
    // public int AmmoValue
    // {
    //     get { return ammoValue; }
    //     set
    //     {
    //         value = Mathf.Clamp(value, 0, ammoMaxValue);
    //         ammoValue = value;
    //         UpdateAmmoValue(ammoValue);

    //     }
    // }

    [Header("Hp Bar Settings")]
    public Text hpTxt;
    public Image hpBar, hpBarBg;
     [Range(1, 70)]
    public int hpAlert = 20;
    public Color hpAlertColor;
    public Color hpBarBgColor;
    
    public AudioClip hpBarSound;
    public bool hpBarSoundRepeat = false;
    public float hpBarSoundRepeatRate = 1f;
    private float nextPlayHpSound;

    private float hpBarValue = 20;
    public float HpBarValue
    {
        get { return hpBarValue; }

        set
        {
            value = Mathf.Clamp(value, 0, 100);
            hpBarValue = value;
            UpdateHpValue(hpBarValue);

        }
    }

    [Header("Titanfall Bar Settings")]
    public Image titanfallBar, titanfallBarBg;
   
    [Range(1, 100)]
    public int titanfallAlert = 20;
    public Color titanfallBarAlertColor;
    public Color titanfallBarBgColor;
    public AudioClip titanfallBarSound;
    public bool titanfallBarSoundRepeat = false;
    public float titanfallBarSoundRepeatRate = 1f;
    private float nextPlayTitanFallSound;
    private AudioSource audiosource;
    private float titanfallBarValue = 32;
    public float TitanfallBarValue
    {
        get { return titanfallBarValue; }
        set
        {
            value = Mathf.Clamp(value, 0, 100);
            titanfallBarValue = value;
            UpdateTitanfallValue(titanfallBarValue);

        }
    }

    private void Awake()
    {
        audiosource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        hpTxt.text = "HP";

        hpBarBg.color = hpBarBgColor;
        titanfallBarBg.color = titanfallBarBgColor;

        UpdateHpValue(hpBarValue);
        UpdateTitanfallValue(titanfallBarValue);
        // JUST FOR DEBUG
        UpdateAmmoValue(7); 
    }

    // Update is called once per frame
    private void Update()
    {
       

        if (!Application.isPlaying)
        {
            hpBarBg.color = hpBarBgColor;
            titanfallBarBg.color = titanfallBarBgColor;

            UpdateHpValue(50);
            UpdateTitanfallValue(30);
            // JUST FOR DEBUG
            UpdateAmmoValue(7);   
        }
        else
        {
            UpdateHpAlertSound();
            UpdateTitanfallAlertSound();
        }
    }

    private void UpdateAmmoValue(int val) {
        if (currentWeaponData != null) {
            weaponIcon.sprite = currentWeaponData.WeaponIcon;
            weaponNameTxt.text = currentWeaponData.WeaponName;
            ammoMaxValue = currentWeaponData.AmmoCount;
            ammoCountTxt.text = val + "\n" + ammoMaxValue;
        }

    }

    public void UpdateWeaponValues(Sprite icon, string weaponName, int maxAmmoCount, int currentAmmoCount) {
        weaponIcon.sprite = icon;
        weaponNameTxt.text = weaponName;
        ammoMaxValue = maxAmmoCount;
        ammoCountTxt.text = currentAmmoCount + "\n" + ammoMaxValue;
    }
    
    private void UpdateHpValue(float val)
    {
       
        hpBar.fillAmount = (val / 100)*0.75f;

        hpTxt.text = val + "%";

        if (hpAlert >= val)
        {
            hpBarBg.color = hpAlertColor;
        }
        else
        {
            hpBarBg.color = hpBarBgColor;
        }

    }

   private void UpdateTitanfallValue(float val)
    {
       
        titanfallBar.fillAmount = (val / 100);

        if (titanfallAlert <= val)
        {
            titanfallBarBg.color = titanfallBarAlertColor;
        }
        else
        {
            titanfallBarBg.color = titanfallBarBgColor;
        }

    }

    private void UpdateHpAlertSound()
    {
        if (hpAlert >= hpBarValue && Time.time > nextPlayHpSound)
        {
            nextPlayHpSound = Time.time + hpBarSoundRepeatRate;
            audiosource.PlayOneShot(hpBarSound);
        }
    }

    private void UpdateTitanfallAlertSound()
    {
        if (titanfallAlert <= titanfallBarValue && Time.time > nextPlayTitanFallSound)
        {
            nextPlayTitanFallSound = Time.time + titanfallBarSoundRepeatRate;
            audiosource.PlayOneShot(titanfallBarSound);
        }
    }
}
