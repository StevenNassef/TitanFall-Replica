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
    private AudioClip airBorn;
    private List<AudioClip> hit;

    void Start()
    {
        var pilotFolder = "Pilots/";

        kill = Resources.Load<AudioClip>(pilotFolder + "PlayerPilot/miscsounds_pilot kill");
        walking = Resources.Load<AudioClip>(pilotFolder + "PilotFullWalk");
        airBorn = Resources.Load<AudioClip>(pilotFolder + "airBorn");
        hit = new List<AudioClip>(Resources.LoadAll<AudioClip>(pilotFolder + "Hit"));
    }

    [ButtonMethod]
    void playKill(){
        playEffect(kill, false);
    }

    [ButtonMethod]
    void playHit(){
        playEffect(hit.GetRandom(), false);
    }


    [ButtonMethod]
    void playWalking(){
        playBackground(walking, true);
    }

    [ButtonMethod]
    void playAirBorn(){
        playBackground(airBorn, true);
    }


    void playEffect(AudioClip clip, bool loop) {
        effect.GetComponent<AudioSource>().loop = loop;
        effect.GetComponent<AudioSource>().clip = clip;
        effect.GetComponent<AudioSource>().Play();
    }

    void playBackground(AudioClip clip, bool loop) {
        background.GetComponent<AudioSource>().loop = loop;
        background.GetComponent<AudioSource>().clip = clip;
        background.GetComponent<AudioSource>().Play();
    }

    
    [ButtonMethod]
    void stopBackground(){
        background.GetComponent<AudioSource>().loop = false;
        background.GetComponent<AudioSource>().Stop();
    }
}
