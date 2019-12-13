using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    public AudioMixer masterMixer;

    public void setMasterVolume(float v) {
        masterMixer.SetFloat("masterVolume", v);
    }

    public void setMusicVolume(float v) {
        masterMixer.SetFloat("musicVolume", v);
    }

    public void setSFXVolume(float v) {
        masterMixer.SetFloat("sfxVolume", v);
    }

}
