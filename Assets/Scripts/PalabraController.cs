using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PalabraController : MonoBehaviour
{
    List<SilabaController> silabas;

    GameManager gameManager;

    int anchoSilaba = 1;
    float segundosAntesDeQuieta = 2;

    Boolean __flagged = false;
    public Boolean moviendose = false;

    Rigidbody rb;

    #region eventos
    void OnEnable()
    {
        EventManager.silabaSeparadaDeSilaba += separarEnSilaba;
        EventManager.silabaSeparadaDeSilaba += flagForDestruction;

    }

    void OnDisable()
    {
        EventManager.silabaSeparadaDeSilaba -= separarEnSilaba;
        EventManager.silabaSeparadaDeSilaba -= flagForDestruction;
    }

    void OnMouseDown()
    {
        this.dejarQuieta();
    }
    #endregion

    #region ciclo de vida
    void Awake()
    {
        if (!rb)
        {
            rb = gameObject.GetComponent<Rigidbody>();
        }

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
        if(ls.Count == 0) { return; }

        GameObject padreDeSilabas = ls[0].getPalabra().transform.parent.gameObject;
        this.transform.SetParent(padreDeSilabas.transform);

        foreach(SilabaController sil in ls)
        {
            this.nuevaSilabaAlFinal(sil);
        }
    }

    public void setSilaba(string sil)
    {
        SilabaController silaba = gameManager.nuevaSilaba(sil);

        silaba.setPalabra(this.gameObject, this);

        if (this.silabas.Count > 0)
        {
            //eliminarSilabas();
        }

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

    internal void acomodarSilabasEnElEspacio()
    {
        if(silabas.Count <= 1){return;}

        //ajustamos la pos de la primera silaba

        Vector3 posInicial = silabas[0].transform.position;
        Vector3 posActual = posInicial;
        posActual.x += anchoSilaba;
        //acomodamos las silabas de izquierda a derecha
        for (int i = 1; i < silabas.Count; i++)
        {
            silabas[i].transform.position = posActual;
            posActual.x += anchoSilaba;
        }
    }

    public void aniadirPalabraAlFinal(List<SilabaController> silabasNuevas)
    {
        foreach (SilabaController s in silabasNuevas)
        {
            nuevaSilabaAlFinal(s);
        }
    }

    public void aniadirPalabraAlPrincipio(List<SilabaController> silabasNuevas)
    {
        for(int i = silabasNuevas.Count - 1; i>=0;i--)
        {
            nuevaSilabaAlPrincipio(silabasNuevas[i]);
        }
    }

    public void nuevaSilabaAlFinal(SilabaController silaba)
    {
        silaba.getPalabraController().flagForDestruction();
        silaba.setPalabra(this.gameObject,this);

        if(this.silabas.Count > 0)
        {
            SilabaController ultimaSilaba = silabas[silabas.Count - 1];
            ultimaSilaba.silabaDerecha = silaba;
            silaba.silabaIzquierda = ultimaSilaba;
        }

        silabas.Add(silaba);
    }

    public void nuevaSilabaAlPrincipio(SilabaController silaba)
    {
        silaba.getPalabraController().flagForDestruction();
        silaba.setPalabra(this.gameObject,this);
        silabas.Insert(0,silaba);
    }


    public void sacudirYColocarEnPantalla()
    {
        Debug.Log("sacudirYColocarEnPantalla NO implementado");
    }

    private void separarEnSilaba(SilabaController silabaASeparar)
    {
        if (!silabas.Contains(silabaASeparar)) {
            return;
        }

        int indexSilabaASeparar = silabas.FindIndex(x => x == silabaASeparar);
        int finalDeLista = silabas.Count - 1;


        List<SilabaController> silabasAntes = new List<SilabaController>();
        List<SilabaController> listaSilabaASeparar = new List<SilabaController>();
        List<SilabaController> silabasDespues = new List<SilabaController>();


        //tener en cuenta esto
        listaSilabaASeparar.Add(silabaASeparar);

        //casos faciles (principio y final)
        if (indexSilabaASeparar == finalDeLista || indexSilabaASeparar == 0) //esta al principio/final de la lista
        {
            silabas.Remove(silabaASeparar);

            gameManager.nuevaPalabra(listaSilabaASeparar);
        }
        else //la silaba está en el medio
        {
            for(int i = 0; i<silabas.Count; i++)
            {
                if(i < indexSilabaASeparar)
                {
                    silabasAntes.Add(silabas[i]);
                }
                if(i > indexSilabaASeparar)
                {
                    silabasDespues.Add(silabas[i]);
                }
            }

            gameManager.nuevaPalabra(silabasAntes);
            gameManager.nuevaPalabra(listaSilabaASeparar);
            gameManager.nuevaPalabra(silabasDespues);

            this.silabas = new List<SilabaController>();
            tryToDestroy(); //descanse en paz
        }

        
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

    public void romperEnSilabas()
    {
        if(silabas.Count == 0 || silabas.Count == 1) { return; }

        foreach (SilabaController sil in silabas)
        {
            sil.separarSilabaDeOtrasSilabas();
            gameManager.nuevaPalabra(sil);
        }

        this.silabas = new List<SilabaController>();
        flagForDestruction();

    }

    public void dejarQuieta()
    {
        foreach(SilabaController sil in silabas)
        {
            sil.dejarQuieta();
        }
    }

    private void encontrarPadre()
    {
        transform.SetParent(gameManager.getJuegoGameObject().transform);
    }

    #region garbage collection

    public void flagForDestruction(SilabaController s)
    {
        flagForDestruction();
    }

    public void flagForDestruction()
    {
        if (!__flagged) { 
            Invoke("tryToDestroy", 6f);
            __flagged = true;
        }
    }

    private void tryToDestroy()
    {
        if (silabas.Count == 0)
        {
            Destroy(this.gameObject);
        }
        else
        {
            __flagged = false;
        }
    }

    #endregion

    private void imprimirPalabra()
    {
        Debug.Log("palabra formada: " + getPalabraString());
    }


    #endregion
}
