using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public enum Titan { 
    Ion,
    Legion 
}

public class TitanSounds : MonoBehaviour
{

    public Titan titanName = Titan.Ion;

    public GameObject background;
    public GameObject talk;
    public GameObject effect;


    private List<AudioClip> welcome;
    private List<AudioClip> coreReady;
    private List<AudioClip> coreFire;
    private List<AudioClip> eject;
    private List<AudioClip> defence;
    private List<AudioClip> criticalDamage;
    private AudioClip walking;
    // Start is called before the first frame update
    void Start()
    {
        var titanFolder = "Titans/";

        welcome = new List<AudioClip>(Resources.LoadAll<AudioClip>(titanFolder + titanName + "/Welcome"));
        coreReady = new List<AudioClip>(Resources.LoadAll<AudioClip>(titanFolder + titanName + "/CoreReady"));
        coreFire = new List<AudioClip>(Resources.LoadAll<AudioClip>(titanFolder + titanName + "/CoreFire"));
        eject = new List<AudioClip>(Resources.LoadAll<AudioClip>(titanFolder + titanName + "/Eject"));
        defence = new List<AudioClip>(Resources.LoadAll<AudioClip>(titanFolder + titanName + "/Defence"));
        criticalDamage = new List<AudioClip>(Resources.LoadAll<AudioClip>(titanFolder + titanName + "/CriticalDamage"));
        walking = Resources.Load<AudioClip>(titanFolder + "miscsounds_Legion Walking Loop");
    }

    [ButtonMethod]
    void playWelcome() {
        talk.GetComponent<AudioSource>().clip = welcome.GetRandom();
        talk.GetComponent<AudioSource>().Play();
    }
    
    [ButtonMethod]
    void playEject() {
        talk.GetComponent<AudioSource>().clip = eject.GetRandom();
        talk.GetComponent<AudioSource>().Play();
    }
    
    [ButtonMethod]
    void playCriticalDamage() {
        talk.GetComponent<AudioSource>().clip = criticalDamage.GetRandom();
        talk.GetComponent<AudioSource>().Play();
    }
    
    [ButtonMethod]
    void playDefence() {
        effect.GetComponent<AudioSource>().clip = defence.GetRandom();
        effect.GetComponent<AudioSource>().Play();
    }
    
    [ButtonMethod]
    void playCoreReady() {
        talk.GetComponent<AudioSource>().clip = coreReady.GetRandom();
        talk.GetComponent<AudioSource>().Play();
    }
    
    [ButtonMethod]
    void playCoreFire() {
        talk.GetComponent<AudioSource>().clip = coreFire.GetRandom();
        talk.GetComponent<AudioSource>().Play();
    }

    [ButtonMethod]
    void playWalking() {
        background.GetComponent<AudioSource>().clip = walking;
        background.GetComponent<AudioSource>().Play();
    }

}
