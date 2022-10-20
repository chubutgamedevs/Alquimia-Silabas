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

    public Boolean moviendose = false;

    Rigidbody rb;

    #region eventos
    void OnEnable()
    {
        EventManager.silabaSeparadaDeSilaba += separarEnSilaba;
    }

    void OnDisable()
    {
        EventManager.silabaSeparadaDeSilaba -= separarEnSilaba;
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
        silaba.setPalabra(this.gameObject,this);
        silabas.Insert(0,silaba);
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
        }

        
    }

    public void romperEnSilabasYColocarEnPantalla()
    {
        //si hay poco que hacer lo hacemos y retornamos por performance
        if (silabas.Count == 0) { return; }

        EventManager.onActivarConectoresDespuesDe(silabas,1f);

        if (silabas.Count == 1)
        {
            silabas[0].desactivarConectores();
            silabas[0].separarFuerteDeOtrasSilabas();
            silabas[0].empujarAleatoriamenteYDejarQuietaLuego();
            return;
        }

        foreach (SilabaController sil in silabas)
        {
            sil.desactivarConectores();
            sil.separarFuerteDeOtrasSilabas();
            sil.empujarAleatoriamenteYDejarQuietaLuego();
        }

        for (int i = 0; i < silabas.Count; i++)
        {
            //desconectamos las silabas y destruimos la palabra luego
            silabas[i].restablecerConectores();
            this.separarEnSilaba(silabas[i]);
        }
    }

    public void sacudirSilabas()
    {
        foreach(SilabaController sil in silabas)
        {
            sil.empujarAleatoriamenteYDejarQuietaLuego();

        }
    }
    public void dejarQuieta()
    {
        foreach (SilabaController sil in silabas)
        {
            sil.dejarQuieta();
        }
    }
    #endregion

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

    private void encontrarPadre()
    {
        transform.SetParent(gameManager.getJuegoGameObject().transform);
    }



    #endregion
}
