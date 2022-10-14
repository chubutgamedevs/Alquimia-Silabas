using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants
{
    public int tres = 3;
}

enum Modo
{
    Secuencial = 0,
    Pool = 1,
}

public class Palabra
{
    public string palabra = "";
    public List<String> silabas = new List<string>();

    public Palabra(string v, List<string> list)
    {
        this.palabra = v;
        this.silabas = list;
    }
}

public class GameManager : MonoBehaviour
{
    private Modo modo = Modo.Pool;

    public bool modoRomper = false;
    private bool _modoRomper = false;

    public GameObject palabraPrefab;
    public GameObject silabaPrefab;

    public List<Palabra> palabrasTarget;
    public List<string> poolDeSilabas;

    private GameObject _juego;

    private string palabraActual = "";


    //sacados de palabras.json
    public Dictionary<string, List<string>> palabrasYSilabas;
    //lista de palabras
    List<string> wordList;

    #region ciclo de vida

    // Start is called before the first frame update
    void Start()
    {
        _juego = getJuegoGameObject();

        switch (modo)
        {
            case Modo.Secuencial:
                break;
            case Modo.Pool:
                startGameConPool();
                break;
            default:
                Console.WriteLine("GAMEMANAGER.START El modo no existe");
                break;
        }

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

    }

    void OnDisable()
    {
        EventManager.silabasUnidas -= comprobarPalabraFormada;
        EventManager.palabraFormada -= handlePalabraFormada;
    }


    void handlePalabraFormada(PalabraController palabraController, string palabra)
    {
        if(modo == Modo.Pool)
        {
            //separar palabra y agitarla
        }
    }
    #endregion

    #region metodos

    public void startGameConPool()
    {
        palabrasTarget = generarPalabrasTargetRandomConSilabas(3,2);
        poolDeSilabas = generarPoolDeSilabas(palabrasTarget);
        anunciarPalabrasTarget();
        colocarEnPantallaSilabas();
        desordenarPalabras();
    }


    List<string> generarPoolDeSilabas(List<Palabra> palabras)
    {
        List<string> pool = new List<string>();

        foreach(Palabra palabra in palabras)
        {
            pool.AddRange(palabra.silabas);
        }


        var silabasSinRepeticion = new HashSet<string>(pool);

        return new List<string>(silabasSinRepeticion);
    }

    void colocarEnPantallaSilabas()
    {
        if(modo == Modo.Pool)
        {
            foreach (string silaba in poolDeSilabas)
            {

                PalabraController palabraAuxController = nuevaPalabra(silaba);
                palabraAuxController.sacudirYColocarEnPantalla();
            }
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

        palabrasYSilabas = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json.text);
        wordList = new List<string>(palabrasYSilabas.Keys);
    }
    #endregion

    public void comprobarPalabraFormada(SilabaController silaba, SilabaController otraSilaba)
    {
        PalabraController palabraFormada = silaba.getPalabraController();
        string palabraAux = palabraFormada.getPalabraString().ToUpper();

        Debug.Log("Palabra formada: ");
        Debug.Log(palabraAux);

        if(modo == Modo.Pool)
        {
            foreach (Palabra palabra in this.palabrasTarget)
            {
                if (palabra.palabra.ToUpper() == palabraAux)
                {
                    EventManager.onPalabraFormada(palabraFormada, palabraAux);
                    return;
                }
            }

        }
        else if(modo == Modo.Secuencial)
        {
            if (palabraActual.ToUpper() == palabraAux)
            {
                Invoke("continuarConSiguientePalabra", 0.5f);

                EventManager.onPalabraFormada(palabraFormada, palabraAux);
                return;
            }
        }

    }

    private void continuarConSiguientePalabra()
    {
        eliminarTodasLasPalabrasEnPantalla();
        if(palabrasTarget.Count > 0)
        {
            palabrasTarget.RemoveAt(0);
            colocarEnPantallaPalabra(palabrasTarget[0]);
            desordenarPalabras();
        }
    }

    internal GameObject nuevaPalabraVacia()
    {
        GameObject palabraObj = Instantiate(palabraPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        return palabraObj;
    }

    internal PalabraController nuevaPalabra(string silaba)
    {
        GameObject palabraObj = nuevaPalabraVacia();
        PalabraController controller = palabraObj.GetComponent<PalabraController>();

        controller.setSilaba(silaba);

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

    private List<Palabra> generarPalabrasTargetRandomConSilabas(int cantPalabras, int cantSilabas)
    {
        List<Palabra> palabrasAux = new List<Palabra>();

        for (int i = 0; i < cantPalabras; i++)
        {
            palabrasAux.Add(nuevaPalabraRandomConSilabas(cantSilabas));
        }

        return palabrasAux;
    }

    

    private void colocarEnPantallaPalabra(Palabra palabra)
    {
        nuevaPalabraActual(palabra);
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

    public Palabra nuevaPalabraRandomConSilabas(int cantSilabas)
    {
        int randomIndex = UnityEngine.Random.Range(0, wordList.Count);
        while (palabrasYSilabas[wordList[randomIndex]].Count != cantSilabas)
        {
            randomIndex = UnityEngine.Random.Range(0, wordList.Count);
        }
        return new Palabra(wordList[randomIndex], palabrasYSilabas[wordList[randomIndex]]);
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

    internal PalabraController nuevaPalabraActual(Palabra palabra)
    {
        palabraActual = palabra.palabra;

        GameObject palabraAux = nuevaPalabraVacia();
        PalabraController palabraAuxController = palabraAux.GetComponent<PalabraController>();


        foreach (string silaba in palabra.silabas) {
            SilabaController silabaAux = nuevaSilaba(silaba);
            silabaAux.setPalabra(palabraAux);
            palabraAuxController.nuevaSilabaAlFinal(silabaAux);
        }

        return palabraAuxController;
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

    public void desordenarPalabras()
    {
        List<PalabraController> palabras = new List<PalabraController>();

        foreach(PalabraController palabra in palabras)
        {   
            //rompemos todas
            //palabra.romperEnSilabasYColocarEnPantalla();
        }

        foreach (Transform hijo in _juego.transform)
        {   //conseguimos las palabras hijo
            GameObject hijoAux = hijo.gameObject;
            if (hijoAux.CompareTag("Palabra"))
            {
                palabras.Add(hijoAux.GetComponent<PalabraController>());
            }
        }

        this.activarConectoresDespuesDe1Seg();

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
    internal SilabaController nuevaSilaba(string silaba)
    {
        GameObject silabaObj = Instantiate(silabaPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        SilabaController silabaCont = silabaObj.GetComponent<SilabaController>();

        silabaCont.setSilaba(silaba);

        return silabaCont;
    }

    public GameObject nuevaPalabra(List<SilabaController> silabas)
    {
        GameObject palabraObj = this.nuevaPalabraVacia();
        PalabraController palabraController = palabraObj.GetComponent<PalabraController>();

        palabraController.setSilabas(silabas);

        return palabraObj;
    }

    public GameObject nuevaPalabra(SilabaController silaba)
    {
        List<SilabaController> aux = new List<SilabaController>();
        aux.Add(silaba);

        return nuevaPalabra(aux);
    }


    #endregion
}
