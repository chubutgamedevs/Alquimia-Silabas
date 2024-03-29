using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;


public class SilabaController : MonoBehaviour
{
    public SilabaController silabaIzquierda;
    public SilabaController silabaDerecha;

    private ConectoresManager conectoresManager;
    
    private GameObject palabraParent;
    private PalabraController palabraController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("colisionador")) {
            palabraController.romperEnSilabasYColocarEnPantalla();
        }
    }

    public String silaba = "CIS";

    public TMPro.TextMeshPro texto;

    private Drag drag;

    private BoxCollider2D boxCollider;

    public Animator animadorSilaba;

    internal Vector3 puntoInicial = new Vector3(0,0,0);
    private Sprite ficha;
    [SerializeField] Sprite[] sprites;
    public GameObject fichin, gif;




    #region eventos
    void OnEnable()
    {
        EventManager.onModoRomperDesActivado += handleModoRomperDesactivado;
        EventManager.onModoRomperActivado += handleModoRomperActivado;
    }

    void OnDisable()
    {
        EventManager.onModoRomperDesActivado -= handleModoRomperDesactivado;
        EventManager.onModoRomperActivado -= handleModoRomperActivado;
    }

    public void disableDrag()
    {
        this.drag.disableDrag();
    }

    public void disableDragPorSegundos(float segundos)
    {
        disableDrag();
        Invoke("enableDrag", segundos);
    }

    public void enableDrag()
    {
        if (this)
        {
            this.drag.enableDrag();
        }
    }

    private void OnMouseDown()
    {
        EventManager.SilabaEsClickeada(this);
    }

    void OnMouseDrag()
    {
        if (drag.dragEnabled)
        {
            this.palabraController.moviendose = true;
        }
    }


    internal void dejarQuietaDespuesDe(float segundos)
    {
        Invoke("dejarQuieta", segundos);
    }

    internal void dejarQuietaDespuesDeRandom(float segundos)
    {
        Invoke("dejarQuieta", Random.Range(segundos, segundos*2));
    }



    void OnMouseUp()
    {
        EventManager.ComprobarBounds();
    }

    #endregion eventos

    #region ciclo de vida
    private void Awake()
    {
        drag = gameObject.GetComponent<Drag>();

        if (!texto)
        {
            texto = getTextMeshPro();
        }

        conectoresManager = getConectoresManager();
        boxCollider = GetComponent<BoxCollider2D>();
    }


    // Start is called before the first frame update
    void Start()
    {
        silabaIzquierda = null;
        silabaDerecha = null;

        texto.text = silaba;

        setPalabra(this.transform.parent.gameObject, this.transform.parent.gameObject.GetComponent<PalabraController>());
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

    public float getAlto()
    {
        return boxCollider.bounds.size.y;
    }


    public void setPalabra(GameObject palabraAux, PalabraController controller)
    {
        this.palabraParent = palabraAux;
        this.transform.SetParent(controller.transform);

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
  
    public void handleModoRomperActivado()
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
    }


    public void eliminarLuegoDeFormacion()
    {
        StartCoroutine(IEeliminarluegoDeFormacion());
    }

    IEnumerator IEeliminarluegoDeFormacion()
    {
        if(this.transform != null)
        {
            transform.DOScale(new Vector3(0, 0, 0), Constants.tiempoAnimacionDestruccionSilaba);
        }
        yield return new WaitForSeconds(Constants.tiempoAnimacionDestruccion*1.1f);
        Destroy(this.gameObject);
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

            EventManager.SilabaSeparadaDeSilaba(this);
        }
    }

    public void separarFuerteDeOtrasSilabas()
    {
        this.silabaIzquierda = null;
        this.silabaDerecha = null;
        this.SpriteDesconectado();
    }


    public void restablecerConectoresDespuesDe(float t)
    {
        Invoke("restablecerConectores", t);
    }

    public void restablecerConectores()
    {
        if (!conectoresManager)
        {
            return;
        }

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
        if (conectoresManager)
        {
            conectoresManager.desActivarConectores();
        }
    }

    public void desactivarConectoresPor(float tiempo)
    {
        conectoresManager.gameObject.SetActive(true);
        desactivarConectores();
        restablecerConectoresDespuesDe(tiempo);
    }

    public void desactivarConectoresIndefinidamente()
    {
        if (conectoresManager)
        {
            conectoresManager.desActivarConectores();
            conectoresManager.gameObject.SetActive(false);
        }
    }



    #endregion


    #region animaciones
    public void playAnimacionSilabaCorrecta()
    {
        animadorSilaba.Play("silabaGreenearOK");
    }

    public void playAnimacionSilabaIncorrecta()
    {
        animadorSilaba.Play("silabaRojearOK");
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

    #region CambioSprites

    public void SpriteConectadoIzq()
    {
        if (this.silabaDerecha != null){
            SpriteConectadoFull();
            return;    
        }
        ficha = sprites[1];
        fichin.GetComponent<SpriteRenderer>().sprite = ficha;
        Instantiate(gif, (transform.position + new Vector3 (0,0,-2)), Quaternion.identity);
    }
    public void SpriteConectadoDer(){

        if (this.silabaIzquierda != null){
            SpriteConectadoFull();
            return;    
        }
        ficha= sprites[2];
        fichin.GetComponent<SpriteRenderer>().sprite = ficha;
        Instantiate(gif, (transform.position + new Vector3 (0,0,-2)), Quaternion.identity);
    }

    public void SpriteConectadoFull(){
        ficha = sprites[3];
        fichin.GetComponent<SpriteRenderer>().sprite = ficha;
    }

    public void SpriteDesconectado(){
        ficha = sprites[0];
        fichin.GetComponent<SpriteRenderer>().sprite = ficha;
    }

    #endregion

}

