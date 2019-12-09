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
    private List<AudioClip> walking;
    private List<AudioClip> dash;

    private AudioClip firePlay;
    private AudioClip fireEnd;
    private AudioClip fire;
    private AudioClip reload;
    private AudioClip sprint;

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

        walking = new List<AudioClip>(Resources.LoadAll<AudioClip>(titanFolder + "Walk"));
        dash = new List<AudioClip>(Resources.LoadAll<AudioClip>(titanFolder + "Dash"));

        fire = Resources.Load<AudioClip>(titanFolder + titanName + "/fire");
        firePlay = Resources.Load<AudioClip>(titanFolder + titanName + "/firePlay");
        fireEnd = Resources.Load<AudioClip>(titanFolder + titanName + "/fireEnd");
        reload = Resources.Load<AudioClip>(titanFolder + titanName + "/reload");
        
        sprint = Resources.Load<AudioClip>(titanFolder + "sprint");
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
        background.GetComponent<AudioSource>().clip = walking.GetRandom();
        background.GetComponent<AudioSource>().loop = true;
        background.GetComponent<AudioSource>().Play();
    }

    [ButtonMethod]
    void playSprint() {
        background.GetComponent<AudioSource>().clip = sprint;
        background.GetComponent<AudioSource>().loop = true;
        background.GetComponent<AudioSource>().Play();
    }

    [ButtonMethod]
    void playDash() {
        background.GetComponent<AudioSource>().loop = false;
        background.GetComponent<AudioSource>().clip = dash.GetRandom();
        background.GetComponent<AudioSource>().Play();
    }

    [ButtonMethod]
    void playFire() {
        StartCoroutine(fireCoroutine());
    }

    IEnumerator fireCoroutine()
    {
        effect.GetComponent<AudioSource>().loop = false;
        effect.GetComponent<AudioSource>().clip = firePlay;
        effect.GetComponent<AudioSource>().Play();

        yield return new WaitForSeconds(firePlay.length);
        
        effect.GetComponent<AudioSource>().clip = fire;
        effect.GetComponent<AudioSource>().loop = true;
        effect.GetComponent<AudioSource>().Play();
    }

    [ButtonMethod]
    void playReload() {
        effect.GetComponent<AudioSource>().loop = false;
        effect.GetComponent<AudioSource>().clip = reload;
        effect.GetComponent<AudioSource>().Play();
    }

    [ButtonMethod]
    void stopFire(){
        effect.GetComponent<AudioSource>().clip = fireEnd;
        effect.GetComponent<AudioSource>().loop = false;
        effect.GetComponent<AudioSource>().Play();
    }

    [ButtonMethod]
    void stopBackground(){
        background.GetComponent<AudioSource>().loop = false;
        background.GetComponent<AudioSource>().Stop();
    }

    [ButtonMethod]
    void stopEffect(){
        effect.GetComponent<AudioSource>().loop = false;
        effect.GetComponent<AudioSource>().Stop();
    }

}
