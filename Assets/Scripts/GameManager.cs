using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public static class Constants
{
    public static float tiempoHastaDejarQuieta = 2;
    public static float tiempoDeAnimacionPalabraCorrecta = 4f;
    public static float tiempoHastaIrAlPunto = 0.5f;
    public static float tiempoAnimacionDestruccion = 1f;

    public static float maxY = 6;
    public static float minY = 0;

    public static float maxX = 15;
    public static float minX = 0;

    public static float factorAgrandarGrid = 1.4f;

    public static float anchoSilaba = 1;
}

public class GameManager : MonoBehaviour
{

    public bool modoRomper = false;

    public List<PalabraSilabas> palabrasTarget;
    public List<string> poolDeSilabas;

    private GameObject _juego;

    private PalabrasDeserializer palabrasDeserializer;

    public GameObject palabraPrefab;
    public GameObject silabaPrefab;

    private List<Vector3> puntos;

    #region ciclo de vida

    // Start is called before the first frame update
    void Start()
    {
        _juego = getJuegoGameObject();
        palabrasDeserializer = new PalabrasDeserializer("silabas");
        puntos = PoissonDiscSampling.generatePoints();

        startGameConPool();
    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion

    #region singleton
    //singleton
    private static GameManager instance;
    public static GameManager GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    #endregion singleton

    #region eventos
    void OnEnable()
    {
        EventManager.silabasUnidas += comprobarPalabraFormada;
        EventManager.palabraFormada += handlePalabraFormada;
        EventManager.modoRomperActivado += activarModoRomper;
        EventManager.modoRomperActivado += desactivarConectoresPor1Seg;
        EventManager.modoRomperDesActivado += desActivarModoRomper;
        EventManager.ganaste += handleGanaste;

    }

    void OnDisable()
    {
        EventManager.silabasUnidas -= comprobarPalabraFormada;
        EventManager.palabraFormada -= handlePalabraFormada;
        EventManager.modoRomperActivado -= activarModoRomper;
        EventManager.modoRomperActivado -= desactivarConectoresPor1Seg;
        EventManager.modoRomperDesActivado -= desActivarModoRomper;
        EventManager.ganaste -= handleGanaste;
    }


    void handlePalabraFormada(PalabraController palabraController, string palabra)
    {
        StartCoroutine(handlePalabraFormadaRutina(palabraController, palabra));
    }
    IEnumerator handlePalabraFormadaRutina(PalabraController palabraController, string palabra)
    {
        palabraController.playAnimacionPalabraCorrecta();
        yield return new WaitForSeconds(Constants.tiempoDeAnimacionPalabraCorrecta);
    }
    #endregion

    #region metodos

    public void handleGanaste()
    {
        eliminarTodasLasPalabrasEnPantallalFinDelJuego();
    }

    public void startGameConPool()
    {
        palabrasTarget = palabrasDeserializer.generarPalabrasTargetRandomConSilabas(3, 3);
        poolDeSilabas = generarPoolDeSilabas(palabrasTarget);
        anunciarPalabrasTarget();
        colocarEnPantallaSilabas();
        Invoke("desordenarPalabras", 0.01f);
    }

    #region modo romper
    public void toggleModoRomper()
    {
        modoRomper = !modoRomper;

        if (modoRomper)
        {
            EventManager.onModoRomperActivado();
        }
        else
        {
            EventManager.onModoRomperDesactivado();
        }
    }

    public void activarModoRomper()
    {
        modoRomper = true;
    }

    public void desActivarModoRomper()
    {
        modoRomper = false;
    }

    #endregion
    List<string> generarPoolDeSilabas(List<PalabraSilabas> palabras)
    {
        List<string> pool = new List<string>();

        foreach (PalabraSilabas palabra in palabras)
        {
            pool.AddRange(palabra.silabas);
        }

        var silabasSinRepeticion = new HashSet<string>(pool);

        return new List<string>(silabasSinRepeticion);
    }

    void colocarEnPantallaSilabas()
    {
        foreach (string silaba in poolDeSilabas)
        {
            PalabraController palabraAuxController = this.nuevaPalabra(silaba, getRandomPunto());
        }
    }

    public Vector3 getRandomPunto()
    {
        int randomIndex = Random.Range(0, puntos.Count);
        Vector3 punto = puntos[randomIndex];

        puntos.Remove(punto);

        return punto;
    }

    void anunciarPalabrasTarget()
    {
        //larga vida a las tuplas
        EventManager.onPalabrasSeleccionadasParaJuego(palabrasTarget);
    }


    public void comprobarPalabraFormada(SilabaController silaba, SilabaController otraSilaba)
    {
        PalabraController palabraFormada = silaba.getPalabraController();
        string palabraAux = palabraFormada.getPalabraString().ToUpper();

        Debug.Log("Palabra formada: ");
        Debug.Log(palabraAux);

        foreach (PalabraSilabas palabra in this.palabrasTarget)
        {
            if (palabra.palabra.ToUpper() == palabraAux)
            {
                EventManager.onPalabraFormada(palabraFormada, palabraAux);
                return;
            }
        }
    }

    public void eliminarTodasLasPalabrasEnPantallalFinDelJuego()
    {
        foreach (Transform hijo in _juego.transform)
        {   //conseguimos las palabras hijo y las matamos a todas
            GameObject hijoAux = hijo.gameObject;
            if (hijoAux.CompareTag("Palabra")) 
            {
                hijoAux.GetComponent<PalabraController>().iniciarDestruccionFinDelJuego() ;
            }
        }
    }

    public void ordenarPalabras()
    {
        EventManager.onOrdenarPalabras();
    }

    public void desordenarPalabras()
    {
        List<PalabraController> palabras = new List<PalabraController>();

        foreach (Transform hijo in _juego.transform)
        {   //conseguimos las palabras hijo
            if (hijo.gameObject.CompareTag("Palabra"))
            {
                palabras.Add(hijo.gameObject.GetComponent<PalabraController>());
            }
        }

        foreach (PalabraController palabra in palabras)
        {
            //rompemos todas
            palabra.romperEnSilabasYColocarEnPantalla();
        }
    }

    public void activarConectoresDespuesDe1Seg()
    {
        foreach (Transform hijo in _juego.transform)
        {   //conseguimos las palabras hijo de vuelta, porque ahora son palabras de una sola silaba
            PalabraController palabraAux = hijo.gameObject.GetComponent<PalabraController>();
            palabraAux.activarConectoresDespuesDe(1f);
        }
    }

    public void desactivarConectoresPor1Seg()
    {
        foreach (Transform hijo in _juego.transform)
        {   //conseguimos las palabras hijo de vuelta, porque ahora son palabras de una sola silaba
            PalabraController palabraAux = hijo.gameObject.GetComponent<PalabraController>();
            palabraAux.desactivarConectores();
            palabraAux.activarConectoresDespuesDe(1f);
        }
    }
    public GameObject getJuegoGameObject()
    {
        if(!_juego)
        {               
            _juego = GameObject.FindGameObjectWithTag("juego").gameObject;
        }
        return _juego;
    }

    #endregion
}

public static class Instantiator
{
    internal static SilabaController nuevaSilaba(this GameManager gm, string silaba, Vector3 punto)
    {
        GameObject silabaObj = GameManager.Instantiate(gm.silabaPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        SilabaController silabaCont = silabaObj.GetComponent<SilabaController>();
        silabaCont.setPunto(punto);
        silabaCont.setSilaba(silaba);

        return silabaCont;
    }

    internal static GameObject nuevaPalabraVacia(this GameManager gm)
    {
        GameObject palabraObj = GameManager.Instantiate(gm.palabraPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        return palabraObj;
    }

    internal static PalabraController nuevaPalabra(this GameManager gm,string silaba, Vector3 punto)
    {
        GameObject palabraObj = gm.nuevaPalabraVacia();
        PalabraController controller = palabraObj.GetComponent<PalabraController>();

        controller.setSilaba(silaba, punto);

        return controller;
    }

    public static PalabraController nuevaPalabra(this GameManager gm,List<SilabaController> silabas)
    {
        GameObject palabraObj = gm.nuevaPalabraVacia();
        PalabraController palabraController = palabraObj.GetComponent<PalabraController>();

        palabraController.setSilabas(silabas);

        return palabraController;
    }

    public static PalabraController nuevaPalabra(this GameManager gm,SilabaController silaba)
    {
        List<SilabaController> aux = new List<SilabaController>();
        aux.Add(silaba);

        return gm.nuevaPalabra(aux);
    }


}
