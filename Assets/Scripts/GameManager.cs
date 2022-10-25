using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Constants
{
    public static float tiempoHastaDejarQuieta = 2;
}

enum Modo
{
    Secuencial = 0,
    Pool = 1,
}

public class GameManager : MonoBehaviour
{
    private Modo modo = Modo.Pool;

    public bool modoRomper = false;
    private bool _modoRomper = false;

    public GameObject palabraPrefab;
    public GameObject silabaPrefab;

    public List<PalabraSilabas> palabrasTarget;
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
        if(modo == Modo.Pool)
        {
            foreach (string silaba in poolDeSilabas)
            {
                PalabraController palabraAuxController = nuevaPalabra(silaba);
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

        if(modo == Modo.Pool)
        {
            foreach (PalabraSilabas palabra in this.palabrasTarget)
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

    private List<PalabraSilabas> generarPalabrasTargetRandomConSilabas(int cantPalabras, int cantSilabas)
    {
        List<PalabraSilabas> palabrasAux = new List<PalabraSilabas>();

        for (int i = 0; i < cantPalabras; i++)
        {
            palabrasAux.Add(nuevaPalabraRandomConSilabas(cantSilabas));
        }

        return palabrasAux;
    }

    

    private void colocarEnPantallaPalabra(PalabraSilabas palabra)
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

    internal PalabraController nuevaPalabraActual(PalabraSilabas palabra)
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
