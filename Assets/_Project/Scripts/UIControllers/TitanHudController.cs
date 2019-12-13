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
    [Range(1, 70)]
    public int hpAlert = 20;
    public Color hpAlertColor;
    public Color hpBarBgColor;
    public AudioClip hpBarSound;
    public bool hpBarSoundRepeat = false;
    public float hpBarAlertRepeatRate = 1f;
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

    [Header("Dash Meter Settings")]
    public Text dashMeterTxt;
    public Image dashMeterBar, dashMeterBarBg;
    [Range(1, 3)]
    public int dashMeterAlert = 3;
    public Color dashMeterAlertColor;
    public Color dashMeterBarBgColor;
    public AudioClip dashMeterBarSound;
    public bool dashMeterBarSoundRepeat = false;
    public float dashMeterBarAlertRepeatRate = 1f;
    private float nextPlayDashMeterBarSound;

    public int dashMeterMaxValue = 3;
    private int dashMeterValue = 20;
    public int DashMeterValue
    {
        get { return dashMeterValue; }

        set
        {
            value = Mathf.Clamp(value, 0, dashMeterMaxValue);
            dashMeterValue = value;
            UpdateDashMeterValue(dashMeterValue);

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
        dashMeterTxt.text = "DASH";
        dashMeterBarBg.color = dashMeterBarBgColor;
        defensiveAbilityTimerTxt.text = "DEF-TIMER";
        defensiveAbilityTimerBg.color = defensiveAbilityTimerBgColor;
        coreAbilityTxt.text = "CORE";
        coreAbilityBarBg.color = coreAbilityBarBgColor;

        UpdateHpValue(hpBarValue);
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
            UpdateDashMeterAlertSound();
            UpdateDefensiveAbilityTimerAlertSound();
            UpdateCoreAbilityAlertSound();
        }
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

    private void UpdateHpAlertSound()
    {
        if (hpAlert >= hpBarValue && Time.time > nextPlayHpSound)
        {
            nextPlayHpSound = Time.time + hpBarAlertRepeatRate;
            audiosource.PlayOneShot(hpBarSound);
        }
    }

    private void UpdateDashMeterValue(int val)
    {
    
        dashMeterBar.fillAmount = (val / dashMeterMaxValue)*1.0f;

        dashMeterTxt.text = val.ToString();

        if (dashMeterAlert >= val)
        {
            dashMeterBarBg.color = dashMeterAlertColor;
        }
        else
        {
            dashMeterBarBg.color = dashMeterBarBgColor;
        }

    }

    private void UpdateDashMeterAlertSound()
    {
        if (dashMeterAlert >= dashMeterValue && Time.time > nextPlayDashMeterBarSound)
        {
            nextPlayDashMeterBarSound = Time.time + dashMeterBarAlertRepeatRate;
            audiosource.PlayOneShot(dashMeterBarSound);
        }
    }

    private void UpdateDefensiveAbilityTimerValue(int val)
    {
    
        defensiveAbilityTimerBar.fillAmount = 1.0f -(val / defensiveAbilityTimerMaxValue);
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

}
