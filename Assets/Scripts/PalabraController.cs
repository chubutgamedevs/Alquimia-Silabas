using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections;


public class PalabraController : MonoBehaviour
{
    public List<SilabaController> silabas;

    GameManager gameManager;

    public Boolean moviendose = false;

    #region eventos

    void OnEnable()
    {
        EventManager.comprobarBounds += comprobarBounds;
        EventManager.onGanaste += iniciarDestruccionFinDelJuego;
    }

    void OnDisable()
    {
        EventManager.comprobarBounds -= comprobarBounds;
        EventManager.onGanaste -= iniciarDestruccionFinDelJuego;
    }



    #endregion

    #region rb y movimiento

    public void irAlPuntoInicial()
    {
        if(silabas.Count > 0)
        {
            irAlPunto(silabas[0].puntoInicial);
        }
    }

    public void disableDrag()
    {
        foreach(SilabaController sil in silabas)
        {
            sil.disableDragPorSegundos(Constants.tiempoDeAnimacionPalabraCorrecta);
        }
    }

    public void irAlPunto(Vector3 punto)
    {
        desactivarConectores();
        activarConectoresDespuesDe(Constants.tiempoHastaIrAlPunto);
        if (this.transform == null)
        {
            return;
        }
        else
        {
        transform.DOMove(punto, Constants.tiempoHastaIrAlPunto).SetEase(Ease.OutElastic);
        }
    }

    public void irAlPuntoInicialLento()
    {
        irAlPuntoLento(silabas[0].puntoInicial);
    }

    public void irAlPuntoLento(Vector3 punto)
    {
        desactivarConectores();
        activarConectoresDespuesDe(Constants.tiempoHastaIrAlPunto);
        if (this.transform == null)
        {
            return;
        }
        else
        {
            transform.DOMove(punto, Constants.tiempoHastaIrAlPunto).SetEase(Ease.OutExpo);
        }
    }




    #endregion
    #region ciclo de vida
    void Awake()
    {
        gameManager = GameManager.GetInstance();

        cargarSilabaInicial();
    }

    private void cargarSilabaInicial()
    {
        silabas = new List<SilabaController>(); ;

        if(transform.childCount == 1)
        {
            SilabaController silabaInicial = transform.GetChild(0).GetComponent<SilabaController>();
            silabas.Add(silabaInicial);
            silabaInicial.setPalabra(this.gameObject, this);
        }
    }

    private void Start()
    {
        gameManager = GameManager.GetInstance();
        acomodarSilabasEnElEspacio();
        irAlPuntoInicialLento();
    }
    #endregion

    #region setters y getters
    void comprobarBounds()
    {

        //nos fijamos si la silaba volviendo al punto inicial con su offset correspondiente está dentro de los bounds del juego
        if (!Ubicador.estaDentroDelJuego(this.transform.position))
        {
            this.irAlPuntoInicialLento();
        }

    }

    Vector3 getNuevaUbicacion()
    {
        return gameManager.getRandomPunto();
    }

    public void settearPuntoInicialRandom()
    {
        this.settearPunto(gameManager.getRandomPunto());
    }

    public void settearPunto(Vector3 punto)
    {
        silabas[0].setPunto(punto);
    }
    public void setSilabas(List<SilabaController> ls)
    {
        if(ls[0] == null)
        {
            Destroy(this.gameObject);
            return;
        }

        this.silabas = ls;

        foreach(SilabaController sil in ls)
        {
            sil.setPalabra(this.gameObject,this);
        }
    }

    public void setSilaba(string sil, Vector3 punto)
    {
        SilabaController silaba = gameManager.nuevaSilaba(sil,punto);

        silaba.setPalabra(this.gameObject, this);

        silabas.Add(silaba);
    }


    public string getPalabraString()
    {
        string palabraAux = "";

        foreach(SilabaController silaba in silabas)
        {
            palabraAux += silaba.silaba;
        }

        return palabraAux.ToLower();
    }


    #endregion

    #region metodos

    #region separar unir y acomodar

    public void playAnimacionPalabraCorrecta()
    {
        foreach (SilabaController sil in silabas)
        {
            sil.playAnimacionSilabaCorrecta();
        }
    }

    internal void acomodarSilabasEnElEspacio()
    {
        Vector3 posBase = new Vector3(Constants.anchoSilaba, 0, 0);

        //acomodamos las silabas de izquierda a derecha
        for (int i = 0; i < silabas.Count; i++)
        {
            silabas[i].transform.DOLocalMove(posBase * i - silabas.Count/2 * posBase,0.2f).SetEase(Ease.OutCirc);
        }

    }

    public void romperEnSilabasYColocarEnPantalla()
    {
        if (silabas.Count == 0) { return; }

        foreach (SilabaController sil in silabas)
        {
            sil.desactivarConectoresPor(Constants.tiempoHastaIrAlPunto);
            sil.separarFuerteDeOtrasSilabas();
        }

        for (int i = 0; i < silabas.Count; i++)
        {
            //desconectamos las silabas y destruimos la palabra luego
            PalabraController nuevaPalabra = gameManager.nuevaPalabra(silabas[i]);
         }

        Destroy(this.gameObject);
    }


    public void handleFormacion()
    {
        desactivarConectoresIndefinidamente();
        foreach(SilabaController sil in silabas)
        {
            sil.playAnimacionSilabaCorrecta();
        }

        liberarPuntos();
        iniciarDestruccionFinDelJuego();
    }
    void liberarPuntos()
    {
        foreach(SilabaController sil in silabas) {
            EventManager.PuntoDevuelto(sil.puntoInicial);
        }
    }


    #endregion

    public void iniciarDestruccionFinDelJuego()
    {
        if ((this.transform != null))
        {
            transform.DOScale(new Vector3(0, 0, 0), Constants.tiempoAnimacionDestruccion).SetEase(Ease.InOutElastic).
                    OnComplete(() => Destroy(this.gameObject));
        }

    }

    public bool tieneUnaSolaSilaba()
    {
        return this.silabas.Count == 1;
    }
    internal void activarConectoresDespuesDe(float v)
    {
        Invoke("activarConectores", v);
    }

    internal void activarConectores()
    {
        if(silabas.Count == 0)
        {
            return;
        }
        //activamos el izquierdo de la silaba 0
        this.silabas[0].restablecerConectores();

        if (silabas.Count == 1)
        {
            return;
        }

        //activamos el derecho de la ultima silaba
        this.silabas[silabas.Count-1].restablecerConectores();
    }

    public void desactivarConectores()
    {
        if (silabas.Count == 0)
        {
            return;
        }
        this.silabas[0].desactivarConectores();

        if(silabas.Count == 1)
        {
            return;
        }
        this.silabas[silabas.Count - 1].desactivarConectores();
    }

    internal void desactivarConectoresIndefinidamente()
    {
        if (silabas.Count == 0)
        {
            return;
        }
        this.silabas[0].desactivarConectoresIndefinidamente();
        this.silabas[silabas.Count - 1].desactivarConectoresIndefinidamente();
    }



    public void OnTriggerEnter(Collider other)
    {
        comprobarBounds();
    }


    #endregion
}
