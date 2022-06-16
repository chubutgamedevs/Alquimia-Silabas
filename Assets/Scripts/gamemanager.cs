using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Audio;
using UnityEngine.UI;

public enum nivelesDeLectura
{
    nula = 0,
    sabe = 1,
    sabeBien = 2
}
public static class Globales
{
    public static float TiempoDeMuestraDeCartasBase = 2f;
    public static float TiempoDeRotacionBase = 0.5f;
    public static float TiempoDeMuestraDeCartas = 2f;
    public static float TiempoDeRotacion = 0.5f;

    public static bool SabeLeer = false;

    public static void aplicarCoeficiente(float coef)
    {
        Globales.TiempoDeMuestraDeCartas = Globales.TiempoDeMuestraDeCartasBase * coef;
        Globales.TiempoDeRotacion = Globales.TiempoDeRotacionBase;
    }
}

public class gamemanager : MonoBehaviour
{
    public GameObject transition;   //Se indica el prefab "Transicion"
    public AudioSource musicMixer;  //Se indica el mixer "Musica"


    public static int puntaje = 0;
    public static string nivelActual = "Geometria";

    public static nivelesDeLectura nivelDeLectura;

    // eventos
    void OnEnable()
    {
        EventManager.JuegoGanado += MostrarFinJuego;
    }

    void OnDisable()
    {
        EventManager.JuegoGanado -= MostrarFinJuego;

    }

    private void Start()
    {
        Application.targetFrameRate = 70;
        musicMixer = GameObject.Find("MusicManager").GetComponent<AudioSource>();   //Busca el objeto musica para cambiarle el volumen

        if (transition == null)
        {
            transition = GameObject.Find("Canvas").transform.Find("Transicion").gameObject;
        }
        transition.SetActive(true);                             //Activa capa "Transicion"
        transition.GetComponent<Animator>().Play("entrada");                        //Activa la animacion de entrada a la escena

        IniciarJuego(nivelActual);
    }

    public void setearNivelDeLecturaNoSabe()
    {
        setearNivelDeLectura(nivelesDeLectura.nula);
    }
    public void setearNivelDeLecturaSabe()
    {
        setearNivelDeLectura(nivelesDeLectura.sabe);
    }

    public void setearNivelDeLecturaSabeBien()
    {
        setearNivelDeLectura(nivelesDeLectura.sabeBien);
    }

    private void setearNivelDeLectura(nivelesDeLectura nivel)
    {
        nivelDeLectura = nivel;

        switch (nivelDeLectura)
        {
            case nivelesDeLectura.nula:
                Globales.aplicarCoeficiente(1.5f);
                Globales.SabeLeer = false;
                break;
            case nivelesDeLectura.sabe:
                Globales.aplicarCoeficiente(1f);
                Globales.SabeLeer = true;
                break;
            case nivelesDeLectura.sabeBien:
                Globales.aplicarCoeficiente(0.8f);
                Globales.SabeLeer = true;
                break;

            default:
                Globales.aplicarCoeficiente(1.5f);
                Globales.SabeLeer = true;
                break;
        }
    }

    private void IniciarJuego(string nivel)
    {
        if (SceneManager.GetActiveScene().name.Contains("Juego"))
        {
            EventManager.OnGameStarted(nivel);
        }
    }

    private void MostrarFinJuego(int Puntaje)
    {
        gamemanager.puntaje = Puntaje;

        IrAEscenaScore();
    }

    public void LoadScene(string scene)     //Resuelve en una corutina a que escena realizar el cambio y su animacion
    {
        StartCoroutine(TransitionOut(scene));
    }

    public void IrAEscenaScore()
    {
        LoadScene("Score");

    }
    public void RestartToMenu()
    {
        LoadScene("Main");
    }
    public void RestartToGameEasy()
    {
        EventManager.OnGameStarted(nivelActual);
        LoadScene("JuegoFacil");
    }
    public void RestartToGameMedium()
    {
        EventManager.OnGameStarted(nivelActual);
        LoadScene("JuegoMedio");
    }
    public void RestartToGameHard()
    {
        EventManager.OnGameStarted(nivelActual);
        LoadScene("JuegoDificil");
    }

    public void CargarEscenaNiveles()
    {
        LoadScene("Niveles");
    }
    public void CargarEscenaDificultades()
    {
        LoadScene("Dificultades");
    }

    public void SeleccionarNivel(string nivel)
    {
        nivelActual = nivel;
        CargarEscenaDificultades();
    }

    public void RestartScene()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator TransitionOut(string scene)
    {
        transition.GetComponent<Animator>().Play("salida");     //Ejecuta la animacion "salida" de "Transicion"
        yield return new WaitForSeconds(1);                     //Espera 1 segundo para poder ejecutar la animacion completa
        SceneManager.LoadScene(scene);                         //Carga escena "string"
    }

    public void Exit()                                 //Salir de la aplicacion
    {
        Application.Quit();
        System.Diagnostics.Process.GetCurrentProcess().Kill();
    }

    public void NotifyButtonClick()
    {
        EventManager.OnButtonClickeado();
    }

}
