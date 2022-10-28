using System;
using System.Collections.Generic;
using UnityEngine;
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
        EventManager.modoRomperDesActivado += acomodarSilabasEnElEspacio;
        EventManager.comprobarBounds += comprobarBounds;
    }

    void OnDisable()
    {
        EventManager.modoRomperDesActivado -= acomodarSilabasEnElEspacio;
        EventManager.comprobarBounds -= comprobarBounds;
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
    }
    #endregion

    #region setters y getters
    void comprobarBounds()
    {
        Vector3 vectorAux = new Vector3(Constants.anchoSilaba, 0, 0);

        for (int i = 0; i<silabas.Count;i++)
        {
            //nos fijamos si la silaba volviendo al punto inicial con su offset correspondiente est� dentro de los bounds del juego
            if (!Ubicador.estaDentroDelJuego(silabas[0].puntoInicial + vectorAux*i))
            {
                this.settearPuntoInicialRandom();
                return;
            }
        }

        //si llegamos hasta ac� volvemos al punto inicial?
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

        return palabraAux;
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
            silabas[i].transform.localPosition= posBase*i;
        }
    }

    public void romperEnSilabasYColocarEnPantalla()
    {
        //si hay poco que hacer lo hacemos y retornamos por performance
        if (silabas.Count == 0) { return; }

        foreach (SilabaController sil in silabas)
        {
            sil.desactivarConectores();
            sil.separarFuerteDeOtrasSilabas();
        }

        for (int i = 0; i < silabas.Count; i++)
        {
            //desconectamos las silabas y destruimos la palabra luego
            PalabraController nuevaPalabra = gameManager.nuevaPalabra(silabas[i]);
            nuevaPalabra.irAlPuntoInicial();
            silabas[i].restablecerConectores();
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
        float tiempoAnimacion = Constants.tiempoDeAnimacionPalabraCorrecta * 1000 * 0.2f;
        int i = 100;
        int incremento = 100;

        float tiempoDeEliminacion = tiempoAnimacion + i;
        foreach(SilabaController sil in silabasAEliminar)
        {
            //Dividimos por 1000 por milisegundos
            sil.eliminarLuegoDeFormacionPalabraEnSegundos(tiempoDeEliminacion/1000);

            i += incremento;
            tiempoDeEliminacion = tiempoAnimacion + i;
        }

        Invoke("romperEnSilabasYColocarEnPantalla", tiempoDeEliminacion);

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
        //activamos el derecho de la ultima silaba
        this.silabas[silabas.Count-1].restablecerConectores();
    }

    internal void desactivarConectores()
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
