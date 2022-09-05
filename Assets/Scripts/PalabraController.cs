using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PalabraController : MonoBehaviour
{
    List<SilabaController> silabas;

    GameManager gameManager;

    Boolean __flagged = false;

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

    #endregion

    #region ciclo de vida
    void Awake()
    {
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
            nuevaSilabaAlFinal(sil);
        }
    }

    public string getPalabraString()
    {
        return silabas.Select(x => x.silaba).ToString();
    }
    #endregion

    #region metodos

    public void aniadirPalabraAlFinal(List<SilabaController> silabasNuevas)
    {
        foreach (SilabaController s in silabasNuevas)
        {
            nuevaSilabaAlFinal(s);
        }
    }
    public void nuevaSilabaAlFinal(SilabaController silaba)
    {
        silaba.getPalabraController().flagForDestruction();
        silaba.setPalabra(this.gameObject,this);
        silabas.Add(silaba);
    }

    public void nuevaSilabaAlPrincipio(SilabaController silaba)
    {
        silaba.getPalabraController().flagForDestruction();
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

            nuevaPalabra(listaSilabaASeparar);
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

            nuevaPalabra(silabasAntes);
            nuevaPalabra(listaSilabaASeparar);
            nuevaPalabra(silabasDespues);

            this.silabas = new List<SilabaController>();
            tryToDestroy(); //descanse en paz
        }

        
    }

    public GameObject nuevaPalabra(List<SilabaController> silabas)
    {
        GameObject palabraObj = gameManager.nuevaPalabra();
        PalabraController palabraController = palabraObj.GetComponent<PalabraController>();

        palabraController.setSilabas(silabas);

        return palabraObj;
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
