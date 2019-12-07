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
    void Start()
    {
        var pilotFolder = "Pilots/";
        kill = Resources.Load<AudioClip>(pilotFolder + "PlayerPilot/miscsounds_pilot kill");
        walking = Resources.Load<AudioClip>(pilotFolder + "PilotFullWalk");
    }

    [ButtonMethod]
    void playKill(){
        effect.GetComponent<AudioSource>().clip = kill;
        effect.GetComponent<AudioSource>().Play();
    }

    void playWalking(){
        backGround.GetComponent<AudioSource>().clip = walking;
        backGround.GetComponent<AudioSource>().Play();
    }
}
