using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.Audio;
using UnityEngine.UI;

public class opciones : MonoBehaviour
{
    public AudioMixer audioMixer;
    public AudioSource musicMixer;

    public Slider brillo;

    private void Start()
    {
        musicMixer = GameObject.Find("Musica").GetComponent<AudioSource>();
    }
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetVolumemusica(float volume)
    {
        musicMixer.volume = volume;
    }

    void OnGUI()
    {
        new WaitForSeconds(3);
    }

    void Update()
    { 
        Screen.brightness = brillo.value;
    }
}


