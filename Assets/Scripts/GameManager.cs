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

    public static float maxX = 10;
    public static float minX = 2;

    public static float anchoSilaba = 1;

    public static float radioUbicador = 3f;
}

public class GameManager : MonoBehaviour
{

    public bool modoRomper = false;

    public List<PalabraSilabas> palabrasTarget;
    
    private GameObject _juego;

    private PalabrasDeserializer palabrasDeserializer;

    public GameObject palabraPrefab;
    public GameObject silabaPrefab;

    private List<Vector3> puntos;

    private string nivel = "json/niveles/nivel0";

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
        EventManager.nosQuedamosSinPalabras += nuevaTandaDePalabras;
    }

    void OnDisable()
    {
        EventManager.silabasUnidas -= comprobarPalabraFormada;
        EventManager.nosQuedamosSinPalabras -= nuevaTandaDePalabras;
    }


    #endregion

    #region metodos


    public void startGameConPool()
    {
        palabrasTarget = palabrasDeserializer.getNuevasPalabrasTarget();

        anunciarPalabrasTarget(palabrasTarget);

        colocarEnPantallaSilabas(palabrasDeserializer.getPoolParcialActual(cantPalabras: 2));

        Invoke("desordenarPalabras",time: 0.01f);
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
            palabra.GetComponent<PalabraController>().iniciarDestruccionFinDelJuego();
        }

    }

    void colocarEnPantallaSilabas(List<string> silabas)
    {
        puntos = PoissonDiscSampling.generatePoints(); // EFECTO COLATERAL (DA�O COLATERAL)

        foreach (string silaba in silabas)
        {
            PalabraController palabraAuxController = this.nuevaPalabra(silaba, getRandomPunto());
        }
    }

    public Vector3 getRandomPunto()
    {
        if(puntos.Count == 0)
        {
            puntos = PoissonDiscSampling.generatePoints();
        }

        int randomIndex = Random.Range(0, puntos.Count);
        Vector3 punto = puntos[randomIndex];
        puntos.RemoveAt(randomIndex);

        return punto;
    }

    void anunciarPalabrasTarget(List<PalabraSilabas>  target)
    {
        EventManager.onPalabrasSeleccionadasParaJuego(target);
    }

    void handlePalabraFormada(PalabraController palabraFormada, string palabraAux)
    {
        EventManager.onPalabraFormada(palabraFormada, palabraAux);
        colocarEnPantallaSilabas(palabrasDeserializer.getPoolParcialActual(cantPalabras: 1));
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
                palabrasTarget.Remove(palabra);
                handlePalabraFormada(palabraFormada,palabraAux);
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
        GameObject palabraObj = GameManager.Instantiate(gm.palabraPrefab, new Vector3(0, 0, 0), Quaternion.identity,gm.getJuegoGameObject().transform);

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
