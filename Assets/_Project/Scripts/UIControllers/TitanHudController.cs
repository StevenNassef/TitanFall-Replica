using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitanHudController : MonoBehaviour
{

    [Header("HP Bar Settings")]
    public Text hpTxt;
    public Image hpBar, hpBarBg;
    [Range(1, 100)]
    public int hpAlert = 20;
    public Color hpAlertColor;
    public Color hpBarBgColor;
    public AudioClip hpBarSound;
    public bool hpBarSoundRepeat = false;
    public float hpBarAlertRepeatRate = 1f;
    private float nextPlayHpSound;
    private float hpValue = 20;
    public float HpValue
    {
        get { return hpValue; }

        set
        {
            value = Mathf.Clamp(value, 0, 100);
            hpValue = value;
            UpdateHpValue(hpValue);

        }
    }

    [Header("Dash Meter Settings")]
    public Text dashMeterTimeTxt;
    public Image[] dashesIcons;
    private int dashMeterValue = 1;
    public int DashMeterValue
    {
        get { return dashMeterValue; }

        set
        {
            value = Mathf.Clamp(value, 0, dashesIcons.Length);
            dashMeterValue = value;
            UpdateDashMeterValue(dashMeterValue);

        }
    }
    private int dashMeterTime;
    public int DashMeterTime
    {
        get {return dashMeterTime;}

        set
        {
            dashMeterTime = value;
            if(dashMeterTime > 0) {
                dashMeterTimeTxt.text = dashMeterTime.ToString();
            } else {
                dashMeterTimeTxt.text ="";
            }
        }
    }


    [Header("Defensive Ability Timer Settings")]
    public Text defensiveAbilityTimerTxt;
    public Image defensiveAbilityTimerBar, defensiveAbilityTimerBg;

    [Range(1, 100)]
    public int defensiveAbilityTimerAlert = 20;
    public Color defensiveAbilityTimerAlertColor;
    public Color defensiveAbilityTimerBgColor;
    public AudioClip defensiveAbilityTimerSound;
    public bool defensiveAbilityTimerSoundRepeat = false;
    public float defensiveAbilityTimerAlertRepeatRate = 1f;

    private float nextPlayDefensiveAbilityTimerSound;

    public int defensiveAbilityTimerMaxValue = 15;
    private int defensiveAbilityTimerValue = 10;
    public int DefensiveAbilityTimerValue
    {
        get { return defensiveAbilityTimerValue; }
        set
        {
            value = Mathf.Clamp(value, 0, defensiveAbilityTimerMaxValue);
            defensiveAbilityTimerValue = value;
            UpdateDefensiveAbilityTimerValue(defensiveAbilityTimerValue);

        }
    }


    [Header("Core Ability Settings")]
    public Text coreAbilityTxt;
    public Image coreAbilityIcon;
    public Image coreAbilityBar, coreAbilityBarBg;

    [Range(1, 100)]
    public int coreAbilityAlert = 20;
    public Color coreAbilityAlertColor;
    public Color coreAbilityBarBgColor;
    public AudioClip coreAbilityBarSound;
    public bool coreAbilityBarSoundRepeat = false;
    public float coreAbilityBarAlertRepeatRate = 1f;

    private float nextPlayCoreAbilitySound;

    private float coreAbilityBarValue = 32;
    public float CoreAbilityBarValue
    {
        get { return coreAbilityBarValue; }
        set
        {
            value = Mathf.Clamp(value, 0, 100);
            coreAbilityBarValue = value;
            UpdateCoreAbilityValue(coreAbilityBarValue);

        }
    }

    private AudioSource audiosource;



    private void Awake()
    {
        audiosource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        hpTxt.text = "HP";
        hpBarBg.color = hpBarBgColor;
        dashMeterTimeTxt.text = "DASH-TIMER";
        defensiveAbilityTimerTxt.text = "DEF-TIMER";
        defensiveAbilityTimerBg.color = defensiveAbilityTimerBgColor;
        coreAbilityTxt.text = "CORE";
        coreAbilityBarBg.color = coreAbilityBarBgColor;

        UpdateHpValue(hpValue);
        UpdateDashMeterValue(dashMeterValue);
        UpdateDefensiveAbilityTimerValue(defensiveAbilityTimerValue);  
        UpdateCoreAbilityValue(coreAbilityBarValue);

    }

    // Update is called once per frame
    private void Update()
    {
        /**
        • Health bar (0:400)
        • Dash bar/counter/meter (0:3 Dashes, 5 secs to generate 1 dash)
        • Defensive ability cooldown timer (15 secs to cool down)
        • Core ability bar (0:100)
        */       

        if (!Application.isPlaying)
        {
            hpBarBg.color = hpBarBgColor;
            coreAbilityBarBg.color = coreAbilityBarBgColor;

            UpdateHpValue(50);
            UpdateDashMeterValue(2);
            UpdateDefensiveAbilityTimerValue(10);  
            UpdateCoreAbilityValue(30);
        }
        else
        {
            UpdateHpAlertSound();
            UpdateDefensiveAbilityTimerAlertSound();
            UpdateCoreAbilityAlertSound();
        }
    }

    private void UpdateHpValue(float val)
    {
       
        hpBar.fillAmount = (val / 100)*1.0f;

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

    private void UpdateHpAlertSound()
    {
        if (hpAlert >= hpValue && Time.time > nextPlayHpSound)
        {
            nextPlayHpSound = Time.time + hpBarAlertRepeatRate;
            audiosource.PlayOneShot(hpBarSound);
        }
    }

    private void UpdateDashMeterValue(int val)
    {
        for(int i=0; i<dashesIcons.Length; i++) {
            if(i <= val) {
                dashesIcons[i].enabled = true;
            } else {
                dashesIcons[i].enabled = false;
            }
        }
    }

    private void UpdateDefensiveAbilityTimerValue(int val)
    {
    
        defensiveAbilityTimerBar.fillAmount = (1.0f - (val / defensiveAbilityTimerMaxValue))*0.7f;
        defensiveAbilityTimerTxt.text = val.ToString();

        if (defensiveAbilityTimerAlert <= val)
        {
            defensiveAbilityTimerBg.color = defensiveAbilityTimerAlertColor;
        }
        else
        {
            defensiveAbilityTimerBg.color = defensiveAbilityTimerBgColor;
        }

    }

    private void UpdateDefensiveAbilityTimerAlertSound()
    {
        if (defensiveAbilityTimerAlert >= defensiveAbilityTimerValue 
                    && Time.time > nextPlayDefensiveAbilityTimerSound)
        {
            nextPlayDefensiveAbilityTimerSound = Time.time + defensiveAbilityTimerAlertRepeatRate;
            audiosource.PlayOneShot(defensiveAbilityTimerSound);
        }
    }

    public void SetCoreAbilityIcon(Sprite icon) {
        coreAbilityIcon.sprite = icon;
    }
    
    private void UpdateCoreAbilityValue(float val)
    {
        
        coreAbilityBar.fillAmount = (val / 100);

        if (coreAbilityAlert <= val)
        {
            coreAbilityBarBg.color = coreAbilityAlertColor;
        }
        else
        {
            coreAbilityBarBg.color = coreAbilityBarBgColor;
        }

    }

    private void UpdateCoreAbilityAlertSound()
    {
        if (coreAbilityAlert <= coreAbilityBarValue && Time.time > nextPlayCoreAbilitySound)
        {
            nextPlayCoreAbilitySound = Time.time + coreAbilityBarAlertRepeatRate;
            audiosource.PlayOneShot(coreAbilityBarSound);
        }
    }

    public void ResetValues() {
        HpValue = 100;
        CoreAbilityBarValue = 0;
        DashMeterTime = 0;
        DashMeterValue = dashesIcons.Length;
    }
}
