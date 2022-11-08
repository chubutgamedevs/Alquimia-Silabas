using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class Constants
{
    public static string tagPalabraObjetivo = "PalabraObjetivo";

    public static float tiempoHastaDejarQuieta = 2;
    public static float tiempoDeAnimacionPalabraCorrecta = 2f;
    public static float tiempoHastaIrAlPunto = 0.5f;
    public static float tiempoAnimacionDestruccion = 1f;
    public static float tiempoAnimacionDestruccionSilaba = 0.5f;
    public static float tiempoAnimacionSalidaPalabraObjetivo = 2f;
    public static float tiempoAnimacionEntradaPalabraObjetivo = 4f;
    public static float tiempoHastaRenovacionDePalabras = 2f;


    public static float maxY = 8;
    public static float minY = 0;

    public static float maxX = 11;
    public static float minX = 0;

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

    private string nivel = "json/niveles/nivel1";

    #region ciclo de vida

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        _juego = getJuegoGameObject();
        palabrasDeserializer = new PalabrasDeserializer(nivel);

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
        EventManager.modoRomperActivado += activarModoRomper;
        EventManager.modoRomperDesActivado += desActivarModoRomper;
        EventManager.nosQuedamosSinPalabras += nuevaTandaDePalabras;
    }

    void OnDisable()
    {
        EventManager.silabasUnidas -= comprobarPalabraFormada;
        EventManager.modoRomperActivado -= activarModoRomper;
        EventManager.modoRomperDesActivado -= desActivarModoRomper;
        EventManager.nosQuedamosSinPalabras -= nuevaTandaDePalabras;
    }


    #endregion

    #region metodos


    public void startGameConPool()
    {
        puntos = PoissonDiscSampling.generatePoints();
        palabrasTarget = palabrasDeserializer.generarPalabrasTargetEnOrden(2);
        poolDeSilabas = generarPoolDeSilabas(palabrasTarget);
        anunciarPalabrasTarget(palabrasTarget);
        colocarEnPantallaSilabas();
        Invoke("desordenarPalabras", 0.01f);
    }

    public void nuevaTandaDePalabras()
    {
        Invoke("limpiarPalabras", Constants.tiempoHastaRenovacionDePalabras - 0.3f);
        Invoke("startGameConPool",Constants.tiempoHastaRenovacionDePalabras);
    }

    public void limpiarPalabras()
    {
        foreach(GameObject palabra in GameObject.FindGameObjectsWithTag("Palabra"))
        {
            Destroy(palabra);
        }

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
    List<string> generarPoolDeSilabas(List<PalabraSilabas> palabrasRecibidas)
    {
        List<string> pool = new List<string>();

        foreach (PalabraSilabas palabra in palabrasRecibidas)
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
            if (modoRomper)
            {
                palabraAuxController.handleModoRomperActivado();
            }
        }
    }

    public Vector3 getRandomPunto()
    {
        int randomIndex = Random.Range(0, puntos.Count);
        Vector3 punto = puntos[randomIndex];

        puntos.Remove(punto);

        return punto;
    }

    void anunciarPalabrasTarget(List<PalabraSilabas>  target)
    {
        //larga vida a las tuplas
        EventManager.onPalabrasSeleccionadasParaJuego(target);
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

    public void desordenarPalabras()
    {
        GameObject[] palabras = GameObject.FindGameObjectsWithTag("Palabra");

        foreach (GameObject palabra in palabras)
        {
            //rompemos todas
            palabra.GetComponent<PalabraController>().romperEnSilabasYColocarEnPantalla();
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
