using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class PilotSounds : MonoBehaviour
{
    public GameObject background;
    public GameObject effect;
    
    private AudioClip walking;
    private AudioClip kill;
    private AudioClip dead;
    private AudioClip death;
    private AudioClip airBorn;
    private List<AudioClip> hit;

    public bool isWalking;
    public bool isAir;

    void Start()
    {

        isWalking = false;
        isAir = false;

        var pilotFolder = "Pilots/";

        kill = Resources.Load<AudioClip>(pilotFolder + "PlayerPilot/miscsounds_pilot kill");
        walking = Resources.Load<AudioClip>(pilotFolder + "PilotFullWalk");
        airBorn = Resources.Load<AudioClip>(pilotFolder + "airBorn");
        dead = Resources.Load<AudioClip>(pilotFolder + "Dead");
        death = Resources.Load<AudioClip>(pilotFolder + "Death");
        hit = new List<AudioClip>(Resources.LoadAll<AudioClip>(pilotFolder + "Hit"));
    }

    [ButtonMethod]
    public void playKill(){
        playEffect(kill, false);
    }

    [ButtonMethod]
    public void playHit(){
        playEffect(hit.GetRandom(), false);
    }

    [ButtonMethod]
    public void playDead(){
        playEffect(dead, false);
        playBackground(death, false);
    }


    [ButtonMethod]
    public void playWalking(){
        playBackground(walking, true);
        isWalking = true;

        isAir = false;
    }

    [ButtonMethod]
    public void playAirBorn(){
        playBackground(airBorn, true);
        isAir= true;

        isWalking = false;
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
        
        isWalking = false;
        isAir = false;
    }
}
