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

    public Animator animadorSilaba;

    private Vector3 puntoInicial = new Vector3(0,0,0);

    #region eventos
    void OnEnable()
    {
        EventManager.modoRomperDesActivado += handleModoRomperDesactivado;
        EventManager.modoRomperActivado += handleModoRomperActivado;
    }

    void OnDisable()
    {
        EventManager.modoRomperDesActivado -= handleModoRomperDesactivado;
        EventManager.modoRomperActivado -= handleModoRomperActivado;
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
        if (this.palabraController.tieneUnaSolaSilaba())
        {
            this.irAlPuntoInicial();
        }
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
    public void setPunto(Vector3 punto)
    {
        this.puntoInicial = punto;
    }
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
  
    void handleModoRomperActivado()
    {
        this.disableDrag();
        this.entrarAnimacionModoRomper();
        this.desactivarConectores();
    }
    void handleModoRomperDesactivado()
    {
        this.enableDrag();
        this.salirDeTodasLasAnimaciones();
        this.restablecerConectores();
    }

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

        if (silabaIzquierda | silabaDerecha)
        {
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
            this.silabaDerecha = null;

            conectoresManager.activarConectores();

            EventManager.onSilabaSeparadaDeSilaba(this);
        }
    }

    public void separarFuerteDeOtrasSilabas()
    {
        this.silabaIzquierda = null;
        this.silabaDerecha = null;
    }

    public void empujarAleatoriamenteYDejarQuietaLuego()
    {
        dejarQuieta();
        irAlPuntoInicial();
    }

    public void irAlPuntoInicial()
    {
        irAlPunto(this.puntoInicial);
    }
    public void irAlPunto(Vector3 punto)
    {
        StartCoroutine(MovedorDeSilabas.IrHaciaElPuntoEn(this.transform,punto, Constants.tiempoHastaIrAlPunto));
    }

    public void restablecerConectoresDespuesDe(float t)
    {
        Invoke("restablecerConectores", t);
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


    #region animaciones
    public void playAnimacionSilabaCorrecta()
    {
        animadorSilaba.Play("silabaGreenearOK");
    }

    public void entrarAnimacionModoRomper()
    {
        animadorSilaba.Play("silabaEnModoRomper");
    }

    public void salirDeTodasLasAnimaciones()
    {
        animadorSilaba.Play("rest");
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



#region clases auxiliares

static class MovedorDeSilabas
{
    public static IEnumerator IrHaciaElPuntoEn(Transform transformToMove, Vector3 targetPosition, float tiempoHastaDejarQuieta)
    {
        var startPosition = transformToMove;

        var timePassed = 0f;
        while (timePassed < tiempoHastaDejarQuieta*0.4f)
        {
            var factor = timePassed / tiempoHastaDejarQuieta;
            //optional add ease -in and -out
            factor = Mathf.SmoothStep(0, 1f, factor);

            transformToMove.position = Vector3.Lerp(transformToMove.position, targetPosition, factor);

            timePassed += Time.deltaTime;

            // important! This tells Unity to interrupt here, render this frame
            // and continue from here in the next frame
            yield return null;
        }

        // to be sure to end with exact values set the target rotation fix when done
        transformToMove.position = targetPosition;
    }
}

#endregion