using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class SilabaController : MonoBehaviour
{
    public SilabaController silabaIzquierda;
    public SilabaController silabaDerecha;

    private ConectoresManager conectoresManager;
    
    private GameObject palabraParent;
    private PalabraController palabraController;

    public Rigidbody rb;
    
    public String silaba = "CIS";

    public TMPro.TextMeshPro texto;

    private Drag drag;
    public bool moviendose = false;

    private BoxCollider boxCollider;

    #region eventos
    void OnEnable()
    {
        EventManager.modoRomperDesActivado += enableDrag;
        EventManager.modoRomperActivado += disableDrag;

    }

    void OnDisable()
    {
        EventManager.modoRomperDesActivado -= enableDrag;
        EventManager.modoRomperActivado -= disableDrag;
    }

    public void disableDrag()
    {
        this.drag.disableDrag();
    }

    public void enableDrag()
    {
        this.drag.enableDrag();
    }

    private void OnMouseDown()
    {
        EventManager.onSilabaEsClickeada(this);
    }
    void OnMouseDrag()
    {
        if (drag.dragEnabled)
        {

        moviendose = true;
        }
    }
    void OnMouseUp()
    {
        moviendose = false;
    }

    #endregion eventos

    #region ciclo de vida
    private void Awake()
    {
        drag = gameObject.GetComponent<Drag>();

        if (!rb)
        {
            rb = gameObject.GetComponent<Rigidbody>();
        }

        if (!texto)
        {
            texto = getTextMeshPro();
        }

        conectoresManager = getConectoresManager();
        boxCollider = GetComponent<BoxCollider>();

        setPalabra(this.transform.parent.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        silabaIzquierda = null;
        silabaDerecha = null;

        silaba = RandomString(2); /*testing*/
        texto.text = silaba;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion ciclo de vida

    #region getters & setters
    ConectoresManager getConectoresManager(){
        return gameObject.transform.GetChild(2).gameObject.GetComponent<ConectoresManager>();
    }

    TMPro.TextMeshPro getTextMeshPro()
    {
        return gameObject.transform.GetChild(1).gameObject.GetComponent<TMPro.TextMeshPro>();
    }

    public float getAncho()
    {
        return boxCollider.bounds.size.x;
    }

    public float getAlto()
    {
        return boxCollider.bounds.size.y;
    }

    public void setPalabra(GameObject palabraAux)
    {
        this.palabraParent = palabraAux;
        this.transform.SetParent(this.palabraParent.transform);

        this.palabraController = this.palabraParent.GetComponent<PalabraController>();
    }

    public void setPalabra(GameObject palabraAux, PalabraController controller)
    {
        this.palabraParent = palabraAux;
        this.transform.SetParent(this.palabraParent.transform);

        this.palabraController = controller;
    }

    public GameObject getPalabra()
    {
        return this.palabraParent;
    }

    public PalabraController getPalabraController()
    {
        return this.palabraController;
    }

    #endregion 

    #region metodos
    public List<SilabaController> getSilabasPalabra()
    {
        SilabaController recorredorSilabas = null;
        recorredorSilabas = this;

        List<SilabaController> palabra = new List<SilabaController>();

        //vamos a la primera silaba
        while (recorredorSilabas.silabaIzquierda)
        {
              recorredorSilabas = recorredorSilabas.silabaIzquierda;            
        }

        //vamos a la ultima silaba y obtenemos la palabra
        while (recorredorSilabas)
        {
            palabra.Add(recorredorSilabas);
            recorredorSilabas = recorredorSilabas.silabaDerecha;
        }

        return palabra;
    }
    public void dejarQuietaYQuitarControlDeMouse()
    {
        moviendose = false;
        drag.disableDrag();
    }

    public void separarSilabaDeOtrasSilabas()
    {

        if(silabaIzquierda | silabaDerecha) {
            if (silabaIzquierda)
            {
                silabaIzquierda.silabaDerecha = null;
                silabaIzquierda.conectoresManager.activarConectorDerecho();
            }

            if (silabaDerecha)
            {
                silabaDerecha.silabaIzquierda = null;
                silabaDerecha.conectoresManager.activarConectorIzquierdo();
            }

            this.silabaIzquierda = null;
            this.silabaDerecha  = null;

            this.dejarQuietaYQuitarControlDeMouse();
            Vector3 newPos = transform.position;
            newPos.y += getAlto() * Mathf.Sign(random.Next(-1, 1));

            transform.position = newPos;

            conectoresManager.activarConectores();

            EventManager.onSilabaSeparadaDeSilaba(this);
        }
    }

    void restablecerConectores()
    {
        if (!silabaIzquierda)
        {
            conectoresManager.activarConectorIzquierdo();
        }
        if (!silabaDerecha)
        {
            conectoresManager.activarConectorDerecho();
        }

        drag.enableDrag();
    }

    #endregion

    #region testing

    //for testing purposes only
    private static Random random = new Random();
    public static string RandomString(int length)
    {

        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    #endregion testing
}
