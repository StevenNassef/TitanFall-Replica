using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
public class PilotSounds : MonoBehaviour
{
    public GameObject backGround;
    public GameObject effect;
    
    private AudioClip walking;
    private AudioClip kill;
    private List<AudioClip> hit;

    void Start()
    {
        var pilotFolder = "Pilots/";

        kill = Resources.Load<AudioClip>(pilotFolder + "PlayerPilot/miscsounds_pilot kill");
        walking = Resources.Load<AudioClip>(pilotFolder + "PilotFullWalk");
        hit = new List<AudioClip>(Resources.LoadAll<AudioClip>(pilotFolder + "Hit"));
    }

    [ButtonMethod]
    void playKill(){
        effect.GetComponent<AudioSource>().clip = kill;
        effect.GetComponent<AudioSource>().Play();
    }

    [ButtonMethod]
    void playWalking(){
        backGround.GetComponent<AudioSource>().clip = walking;
        backGround.GetComponent<AudioSource>().loop = true;
        backGround.GetComponent<AudioSource>().Play();
    }

    [ButtonMethod]
    void playHit(){
        effect.GetComponent<AudioSource>().clip = hit.GetRandom();
        effect.GetComponent<AudioSource>().Play();
    }

    [ButtonMethod]
    void stopBackground(){
        backGround.GetComponent<AudioSource>().loop = false;
        backGround.GetComponent<AudioSource>().Stop();
    }
}
