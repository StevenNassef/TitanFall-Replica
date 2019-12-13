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
    private AudioClip nukeEject;
    private AudioClip exit;
    private AudioClip ejectBeeps;

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
        nukeEject = Resources.Load<AudioClip>(titanFolder + "NukeEject");
        exit = Resources.Load<AudioClip>(titanFolder + "TitanExit");
        ejectBeeps = Resources.Load<AudioClip>(titanFolder + "EjectBeeps");
    }

    [ButtonMethod]
    public void playWelcome() {
        playTalk(welcome.GetRandom(), false);
    }
    
    [ButtonMethod]
    public void playEject() {
        playTalk(eject.GetRandom(), false);
    }
    
    [ButtonMethod]
    public void playCriticalDamage() {
        playTalk(criticalDamage.GetRandom(), false);
    }

    [ButtonMethod]
    public void playCoreReady() {
        playTalk(coreReady.GetRandom(), false);
    }

    [ButtonMethod]
    public void playCoreFire() {
        playTalk(coreFire.GetRandom(), false);
    }
    

    [ButtonMethod]
    public void playDefence() {
        playEffect(defence.GetRandom(), false);
    }

    [ButtonMethod]
    public void playFire() {
        StartCoroutine(fireCoroutine());
    }

    IEnumerator fireCoroutine()
    {
        playEffect(firePlay, false);

        yield return new WaitForSeconds(firePlay.length);
        
        playEffect(fire, true);
    }

    [ButtonMethod]
    public void playReload() {
        playEffect(reload, false);
    }

    [ButtonMethod]
    public void stopFire(){
        playEffect(fireEnd, false);
    }

    [ButtonMethod]
    public void playNukeEject() {
        playEffect(nukeEject, false);
    }

    [ButtonMethod]
    public void playExit() {
        playEffect(exit, false);
    }

    [ButtonMethod]
    public void playEjectBeeps() {
        playEffect(ejectBeeps, false);
    }


    [ButtonMethod]
    public void playWalking() {
        playBackground(walking.GetRandom(), true);
    }

    [ButtonMethod]
    public void playSprint() {
        playBackground(sprint, true);
    }

    [ButtonMethod]
    public void playDash() {
        playBackground(dash.GetRandom(), false);
    }


    public void playTalk(AudioClip clip, bool loop) {
        talk.GetComponent<AudioSource>().loop = loop;
        talk.GetComponent<AudioSource>().clip = clip;
        talk.GetComponent<AudioSource>().Play();
    }

    public void playEffect(AudioClip clip, bool loop) {
        effect.GetComponent<AudioSource>().loop = loop;
        effect.GetComponent<AudioSource>().clip = clip;
        effect.GetComponent<AudioSource>().Play();
    }

    public void playBackground(AudioClip clip, bool loop) {
        background.GetComponent<AudioSource>().loop = loop;
        background.GetComponent<AudioSource>().clip = clip;
        background.GetComponent<AudioSource>().Play();
    }


    [ButtonMethod]
    public void stopBackground(){
        background.GetComponent<AudioSource>().loop = false;
        background.GetComponent<AudioSource>().Stop();
    }

    [ButtonMethod]
    public void stopEffect(){
        effect.GetComponent<AudioSource>().loop = false;
        effect.GetComponent<AudioSource>().Stop();
    }

}
