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
        EventManager.ganaste += iniciarDestruccionFinDelJuego;
    }

    void OnDisable()
    {
        EventManager.comprobarBounds -= comprobarBounds;
        EventManager.ganaste -= iniciarDestruccionFinDelJuego;
    }



    #endregion

    #region rb y movimiento
    public void handleModoRomperActivado()
    {
        foreach(SilabaController sil in silabas)
        {
            sil.handleModoRomperActivado();
        }
    }
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

    public void irAlPuntoInicialLuegoDeFormarPalabra()
    {
        desactivarConectores();
        activarConectoresDespuesDe(Constants.tiempoDeAnimacionPalabraCorrecta);
        if (this.transform == null)
        {
            return;
        }
        else
        {
            transform.DOMove(silabas[0].puntoInicial, Constants.tiempoHastaIrAlPunto).SetEase(Ease.OutElastic);
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
        encontrarPadre();
        acomodarSilabasEnElEspacio();
    }
    #endregion

    #region setters y getters
    void comprobarBounds()
    {
        float ancho = Constants.anchoSilaba;
        int maxIndice = silabas.Count - 1;
        Vector3 vectorAux = new Vector3(ancho, 0, 0);


        if (maxIndice < 0)
        {
            return;
        }

        Vector3 posPrimeraSilaba = new Vector3(silabas[0].transform.position.x * ancho + Mathf.Sign(silabas[0].transform.position.x) * ancho / 2, 1, 0);
        Vector3 posUltimaSilaba= new Vector3(silabas[maxIndice].transform.position.x * ancho + Mathf.Sign(silabas[maxIndice].transform.position.x) * ancho / 2, 1, 0);

        //nos fijamos si la silaba volviendo al punto inicial con su offset correspondiente está dentro de los bounds del juego
        if (!Ubicador.estaDentroDelJuego(posPrimeraSilaba) && 
            !Ubicador.estaDentroDelJuego(posUltimaSilaba))
        {
            this.settearPuntoInicialRandom();
            this.irAlPuntoInicialLento();
            return;
        }

        //si llegamos hasta acá volvemos al punto inicial?
    }
    public void settearPuntoInicialRandom()
    {
        this.silabas[0].setPunto(gameManager.getRandomPunto());
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
            silabas[i].transform.localPosition =  posBase*i;
        }

    }

    internal void acomodarSilabasEnElEspacioADerecha(int cantSilabasAnterior)
    {
        Vector3 posBase = new Vector3(Constants.anchoSilaba, 0, 0);

        //acomodamos las silabas de izquierda a derecha
        for (int i = cantSilabasAnterior; i < silabas.Count; i++)
        {
            silabas[i].transform.localPosition = silabas[i - 1].transform.localPosition + posBase;
        }

    }

    internal void acomodarSilabasEnElEspacioAIzquierda(int cantSilabasAnterior)
    {
        Vector3 posBase = new Vector3(Constants.anchoSilaba, 0, 0);

        //acomodamos las silabas de derecha a izquierda
        for (int i = silabas.Count-1; i > 0; i--)
        {
            silabas[i-1].transform.localPosition = silabas[i].transform.localPosition - posBase;
        }

    }

    public void romperEnSilabasYColocarEnPantallaLuegoDeFormacion()
    {
        if (silabas.Count == 0) { return; }

        foreach (SilabaController sil in silabas)
        {
            sil.separarFuerteDeOtrasSilabas();
        }

        for (int i = 0; i < silabas.Count; i++)
        {
            //desconectamos las silabas y destruimos la palabra luego
            PalabraController nuevaPalabra = gameManager.nuevaPalabra(silabas[i]);
            nuevaPalabra.irAlPuntoInicialLento();
        }

        Destroy(this.gameObject);
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
            nuevaPalabra.irAlPuntoInicial();
         }

        Destroy(this.gameObject);
    }

    public void sacudirSilabas()
    {
        romperEnSilabasYColocarEnPantalla();
    }

    #endregion

    public void iniciarDestruccionFinDelJuego()
    {
        StartCoroutine(destruirseFinDelJuego());
    }

    public void eliminarSilabasLuegoDeFormacion(List<SilabaController> silabasAEliminar)
    {
        //multiplicamos por 1000 porque vamos a trabajar con milisegundos
        float tiempoAnimacion = Constants.tiempoDeAnimacionPalabraCorrecta * 1000 /2;
        int incremento = 100;

        float tiempoDeEliminacion = tiempoAnimacion + incremento;
        foreach(SilabaController sil in silabasAEliminar)
        {
            //Dividimos por 1000 por milisegundos
            sil.eliminarLuegoDeFormacion();
            this.silabas.Remove(sil);

            tiempoDeEliminacion = tiempoAnimacion + incremento;
        }


        Invoke("romperEnSilabasYColocarEnPantallaLuegoDeFormacion", tiempoDeEliminacion/1000);

    }

    IEnumerator destruirseFinDelJuego()
    {
        if ((this.transform != null))
        {
        transform.DOScale(new Vector3(0, 0, 0), Constants.tiempoAnimacionDestruccion).SetEase(Ease.InOutElastic)    ;
        }

        yield return new WaitForSeconds(Constants.tiempoAnimacionDestruccion);
        
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


    private void encontrarPadre()
    {
        transform.SetParent(gameManager.getJuegoGameObject().transform);
    }

    public void OnTriggerEnter(Collider other)
    {
        comprobarBounds();
    }


    #endregion
}
