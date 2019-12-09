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
        effect.GetComponent<AudioSource>().clip = kill;
        effect.GetComponent<AudioSource>().Play();
    }

    [ButtonMethod]
    void playWalking(){
        background.GetComponent<AudioSource>().clip = walking;
        background.GetComponent<AudioSource>().loop = true;
        background.GetComponent<AudioSource>().Play();
    }

    [ButtonMethod]
    void playAirBorn(){
        background.GetComponent<AudioSource>().clip = airBorn;
        background.GetComponent<AudioSource>().loop = true;
        background.GetComponent<AudioSource>().Play();
    }

    [ButtonMethod]
    void playHit(){
        effect.GetComponent<AudioSource>().clip = hit.GetRandom();
        effect.GetComponent<AudioSource>().Play();
    }

    [ButtonMethod]
    void stopBackground(){
        background.GetComponent<AudioSource>().loop = false;
        background.GetComponent<AudioSource>().Stop();
    }
}
