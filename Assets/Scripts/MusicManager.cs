using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public AudioSource musicMixer;
    private static MusicManager instance;
    public AudioClip click;
    private AudioSource mouse;
    public AudioClip flip;
    private AudioSource flipado;
    public AudioClip cartaok;
    private AudioSource cardbien;

    public static MusicManager GetInstance()
    {
        return instance;
    }

    void OnEnable()
    {
        EventManager.SeClickeaBoton += Reproducirclick;
        EventManager.CartaComienzaADescubrirse += sonidocarta;
        EventManager.SeOcultaCarta += sonidocarta;
        EventManager.ParIgual += cartabien;
    }

    void OnDisable()
    {
        EventManager.SeClickeaBoton -= Reproducirclick;
        EventManager.CartaComienzaADescubrirse -= sonidocarta;
        EventManager.SeOcultaCarta -= sonidocarta;
        EventManager.ParIgual -= cartabien;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        if (instance != null && instance != this) //jonathan asi se hace
        {
            Destroy(this.gameObject);
        }
    }

    public void SetVolumemusica(float volume)   //Cambia el volumen de la Musica 
    {

        musicMixer = gameObject.GetComponent<AudioSource>();
    }

    private void Start()
    {
        mouse = GetComponent<AudioSource>();
        flipado = GetComponent<AudioSource>();
        cardbien = GetComponent<AudioSource>();


    }

    public void Reproducirclick()
    {
        mouse.PlayOneShot(click, 0.8f);
    }
    public void sonidocarta(CardController carta)
    {
        flipado.PlayOneShot(flip, 0.8f);
    }

    public void cartabien()
    {
        cardbien.PlayOneShot(cartaok, 0.5f);
    }
}
