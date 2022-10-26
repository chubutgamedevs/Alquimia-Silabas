using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public static class Constants
{
    public static float tiempoHastaDejarQuieta = 2;
    public static float tiempoDeAnimacionPalabraCorrecta = 4f;
    public static float tiempoHastaIrAlPunto = 0.5f;

    public static float maxY = 4;
    public static float minY = -4;

    public static float maxX = 4;
    public static float minX = -10;

    public static float anchoSilaba = 1;
}

public class GameManager : MonoBehaviour
{

    public bool modoRomper = false;

    public GameObject palabraPrefab;
    public GameObject silabaPrefab;

    public List<PalabraSilabas> palabrasTarget;
    public List<string> poolDeSilabas;

    private GameObject _juego;

    private Ubicador ubicador;

    //sacados de palabras.json
    public Dictionary<string, List<string>> palabrasYSilabas;
    //lista de palabras
    List<string> wordList;

    #region ciclo de vida

    // Start is called before the first frame update
    void Start()
    {
        _juego = getJuegoGameObject();
        ubicador = new Ubicador(Screen.width, Screen.height);

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
        cargarPalabrasYSilabas();

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
        EventManager.modoRomperDesActivado += desActivarModoRomper;

    }

    void OnDisable()
    {
        EventManager.silabasUnidas -= comprobarPalabraFormada;
        EventManager.palabraFormada -= handlePalabraFormada;
        EventManager.modoRomperActivado -= activarModoRomper;
        EventManager.modoRomperDesActivado -= desActivarModoRomper;
    }


    void handlePalabraFormada(PalabraController palabraController, string palabra)
    {
        StartCoroutine(handlePalabraFormadaRutina(palabraController,palabra));
    }
    IEnumerator handlePalabraFormadaRutina(PalabraController palabraController,string palabra)
    {
        palabraController.playAnimacionPalabraCorrecta();
        yield return new WaitForSeconds(Constants.tiempoDeAnimacionPalabraCorrecta);
    }
    #endregion

    #region metodos

    #region modo romper
    public void startGameConPool()
    {
        palabrasTarget = generarPalabrasTargetRandomConSilabas(2,3);
        poolDeSilabas = generarPoolDeSilabas(palabrasTarget);
        anunciarPalabrasTarget();
        colocarEnPantallaSilabas();
        Invoke("desordenarPalabras",0.1f);
    }

    public void recargarEscena()
    {
        this.startGameConPool();
    }

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

        foreach(PalabraSilabas palabra in palabras)
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
                PalabraController palabraAuxController = nuevaPalabra(silaba, Ubicador.nuevoPunto());
            }
    }

    void anunciarPalabrasTarget()
    {
        //larga vida a las tuplas
        EventManager.onPalabrasSeleccionadasParaJuego(palabrasTarget);
    }

    #region json handling
    private void cargarPalabrasYSilabas()
    {
        TextAsset json = Resources.Load<TextAsset>("silabas");
        Palabras palabras = JsonUtility.FromJson<Palabras>(json.ToString());
        palabrasYSilabas = palabras.palabras.ToDictionary(x => x.palabra, x => x.silabas);
        wordList = new List<string>(palabrasYSilabas.Keys);
    }
    #endregion

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


    internal GameObject nuevaPalabraVacia()
    {
        GameObject palabraObj = Instantiate(palabraPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        return palabraObj;
    }

    internal PalabraController nuevaPalabra(string silaba, Vector3 punto)
    {
        GameObject palabraObj = nuevaPalabraVacia();
        PalabraController controller = palabraObj.GetComponent<PalabraController>();

        controller.setSilaba(silaba,punto);

        return controller;
    }

    private List<(string, List<string>)> generarPalabrasTargetRandom(int v)
    {
        List<(string, List<string>)> palabrasAux = new List<(string, List<string>)>();

        for (int i = 0; i < v; i++)
        {
            palabrasAux.Add(nuevaPalabraRandom());
        }

        return palabrasAux;
    }

    private List<PalabraSilabas> generarPalabrasTargetRandomConSilabas(int cantPalabras, int cantSilabas)
    {
        List<PalabraSilabas> palabrasAux = new List<PalabraSilabas>();

        for (int i = 0; i < cantPalabras; i++)
        {
            palabrasAux.Add(nuevaPalabraRandomConSilabas(cantSilabas));
        }

        return palabrasAux;
    }

    public (string, List<string>) nuevaPalabraRandom() //retorna tupla, tupla go brr
    {
            
        int randomIndex = UnityEngine.Random.Range(0, wordList.Count);
        while (palabrasYSilabas[wordList[randomIndex]].Count == 1)
        {
            randomIndex = UnityEngine.Random.Range(0, wordList.Count);
        }
        return (wordList[randomIndex], palabrasYSilabas[wordList[randomIndex]]);        
    }

    public PalabraSilabas nuevaPalabraRandomConSilabas(int cantSilabas)
    {
        int randomIndex = UnityEngine.Random.Range(0, wordList.Count);
        while (palabrasYSilabas[wordList[randomIndex]].Count != cantSilabas)
        {
            randomIndex = UnityEngine.Random.Range(0, wordList.Count);
        }
        return new PalabraSilabas(wordList[randomIndex], palabrasYSilabas[wordList[randomIndex]]);
    }

    public (string, List<string>) nuevaPalabraRandomMaxSilabas(int maxSilabas)
    {
        int randomIndex = UnityEngine.Random.Range(0, wordList.Count);
        while (palabrasYSilabas[wordList[randomIndex]].Count < maxSilabas)
        {
            randomIndex = UnityEngine.Random.Range(0, wordList.Count);
        }
        return (wordList[randomIndex], palabrasYSilabas[wordList[randomIndex]]);
    }

    public void eliminarTodasLasPalabrasEnPantalla()
    {
        foreach (Transform hijo in _juego.transform)
        {   //conseguimos las palabras hijo y las matamos a todas
            GameObject hijoAux = hijo.gameObject;
            if (hijoAux.CompareTag("Palabra")) 
            {
                hijoAux.tag = "PalabraDestruida";
                GameObject.Destroy(hijoAux);
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

    public GameObject getJuegoGameObject()
    {
        if(!_juego)
        {               
            _juego = GameObject.FindGameObjectWithTag("juego").gameObject;
        }
        return _juego;
    }
    internal SilabaController nuevaSilaba(string silaba, Vector3 punto)
    {
        GameObject silabaObj = Instantiate(silabaPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        SilabaController silabaCont = silabaObj.GetComponent<SilabaController>();
        silabaCont.setPunto(punto);
        silabaCont.setSilaba(silaba);

        return silabaCont;
    }

    public PalabraController nuevaPalabra(List<SilabaController> silabas)
    {
        GameObject palabraObj = this.nuevaPalabraVacia();
        PalabraController palabraController = palabraObj.GetComponent<PalabraController>();

        palabraController.setSilabas(silabas);

        return palabraController;
    }

    public PalabraController nuevaPalabra(SilabaController silaba)
    {
        List<SilabaController> aux = new List<SilabaController>();
        aux.Add(silaba);

        return nuevaPalabra(aux);
    }


    #endregion
}

public class Ubicador
{
    private float w;
    private float h;

    private static List<Vector3> puntos = new List<Vector3>();

    public Ubicador(float w,float h)
    {
        puntos = new List<Vector3>();
        this.w = w;
        this.h = h;
    }

    public static Vector3 nuevoPunto()
    {
        Vector3 nuevoPunto = new Vector3(Random.Range(-4, 4), Random.Range(-4, 4),0);
        while (puntos.Contains(nuevoPunto))
        {
            nuevoPunto = new Vector3(Random.Range(-4, 4), Random.Range(-4, 4), 0);
        }

        puntos.Add(nuevoPunto);

        return nuevoPunto;
    }

    public static bool estaDentroDelJuego(Vector3 punto)
    {
        return (punto.x > Constants.minX && punto.x < Constants.maxX && punto.y > Constants.minY && punto.y < Constants.maxY);
    }
}
