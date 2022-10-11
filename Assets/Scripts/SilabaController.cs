using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;
using RandomU = UnityEngine.Random;

public class SilabaController : MonoBehaviour
{
    public SilabaController silabaIzquierda;
    public SilabaController silabaDerecha;

    private ConectoresManager conectoresManager;
    
    private GameObject palabraParent;
    private PalabraController palabraController;

    public Rigidbody rb;
    private RigidbodyConstraints rbInitialConstraints;
    
    public String silaba = "CIS";

    public TMPro.TextMeshPro texto;

    private Drag drag;

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
        this.dejarQuieta();
    }
    void OnMouseDrag()
    {
        if (drag.dragEnabled)
        {
            this.palabraController.moviendose = true;
        }
    }

    internal void dejarQuieta()
    {
        this.rb.velocity = Vector3.zero;
        this.rb.constraints = RigidbodyConstraints.FreezeAll;
    }
    internal void dejarQuietaDespuesDe(float segundos)
    {
        Invoke("dejarQuieta", segundos);
    }

    internal void dejarQuietaDespuesDeRandom(float segundos)
    {
        Invoke("dejarQuieta", RandomU.Range(segundos, segundos*2));
    }

    

    internal void habilitarMovimientoRb()
    {
        this.rb.constraints = this.rbInitialConstraints;
    }


    void OnMouseUp()
    {
        this.palabraController.moviendose = false;
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
        rbInitialConstraints = rb.constraints;

        if (!texto)
        {
            texto = getTextMeshPro();
        }

        conectoresManager = getConectoresManager();
        boxCollider = GetComponent<BoxCollider>();
    }

    internal float getYReal()
    {
        return this.transform.position.y + palabraParent.transform.position.y;
    }

    // Start is called before the first frame update
    void Start()
    {
        silabaIzquierda = null;
        silabaDerecha = null;

        texto.text = silaba;

        setPalabra(this.transform.parent.gameObject);
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

    internal void setSilaba(string silaba)
    {
        this.silaba = silaba.ToUpper();
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

    public string getPalabraString()
    {
        return this.palabraController.getPalabraString();
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
        this.palabraController.moviendose = false;
        drag.disableDrag();
        this.dejarQuieta();
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

    public void separarFuerteDeOtrasSilabas()
    {
        this.silabaIzquierda = null;
        this.silabaDerecha = null;
        this.separarSilabaDeOtrasSilabas();
    }

    public void empujarEnDireccionAleatoria()
    {
        //hardcoding bad
        habilitarMovimientoRb();
        this.rb.AddForce(UnityEngine.Random.onUnitSphere * 15, ForceMode.Impulse);
    }

    public void restablecerConectores()
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

    public void desactivarConectores()
    {
        conectoresManager.desActivarConectores();
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
