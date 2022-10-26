using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;


public class PalabraController : MonoBehaviour
{
    List<SilabaController> silabas;

    GameManager gameManager;

    public Boolean moviendose = false;
    public Rigidbody rb;
    private RigidbodyConstraints rbInitialConstraints;

    #region 

    void OnEnable()
    {
        EventManager.modoRomperDesActivado += acomodarSilabasEnElEspacio;
    }

    void OnDisable()
    {
        EventManager.modoRomperDesActivado -= acomodarSilabasEnElEspacio;
    }


    void OnMouseDown()
    {
        this.dejarQuieta();
    }


    #endregion

    #region rb y movimiento

    public void irAlPuntoInicial()
    {
        irAlPunto(silabas[0].puntoInicial);

    }
    public void irAlPunto(Vector3 punto)
    {
        desactivarConectores();
        activarConectoresDespuesDe(Constants.tiempoHastaIrAlPunto);
        if (this.transform == null)
        {
            return;
        }
        transform.DOMove(punto, Constants.tiempoHastaIrAlPunto).SetEase(Ease.OutElastic);
    }


    internal void habilitarMovimientoRb()
    {
        this.rb.constraints = this.rbInitialConstraints;
    }
    internal void dejarQuieta()
    {
        this.rb.velocity = Vector3.zero;
        this.rb.constraints = RigidbodyConstraints.FreezeAll;
    }
    #endregion
    #region ciclo de vida
    void Awake()
    {

        if (!rb)
        {
            rb = gameObject.GetComponent<Rigidbody>();
        }
        rbInitialConstraints = rb.constraints;

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

    public void setSilabas(List<SilabaController> ls)
    {
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

    IEnumerator destruirseFinDelJuego()
    {
        dejarQuieta();

        if (!(this.transform == null))
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
        this.silabas[silabas.Count - 1].desactivarConectores();
    }


    private void encontrarPadre()
    {
        transform.SetParent(gameManager.getJuegoGameObject().transform);
    }



    #endregion
}
