using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool modoRomper = false;
    private bool _modoRomper = false;

    public GameObject palabraPrefab;
    public GameObject silabaPrefab;

    public List<(string palabra,List<string> silabas)> palabrasTarget; //tupla go brrr (tupla es el tipo de dato (tipo a, tipo b, ...)
    public List<string> poolDeSilabas;
    public string palabraActual = "";

    private GameObject _juego;


    //sacados de palabras.json
    public Dictionary<string, List<string>> palabrasYSilabas;
    //lista de palabras
    List<string> wordList;

    #region ciclo de vida

    // Start is called before the first frame update
    void Start()
    {
        _juego = getJuegoGameObject();
        startGame();
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
        EventManager.modoRomperActivado += ActivarModoRomper;
        EventManager.modoRomperDesActivado += DesActivarModoRomper;

    }

    void OnDisable()
    {
        EventManager.silabasUnidas -= comprobarPalabraFormada;
        EventManager.modoRomperActivado -= ActivarModoRomper;
        EventManager.modoRomperDesActivado -= DesActivarModoRomper;
    }
    #endregion

    #region metodos

    public void startGame()
    {
        palabrasTarget = generarPalabrasTargetRandom(4);
        poolDeSilabas = generarPoolDeSilabas(palabrasTarget);
        anunciarPalabrasTarget();
        //colocarEnPantallaPalabra(palabrasTarget[0]);
        colocarEnPantallaSilabas();
        desordenarPalabras();
    }

    List<string> generarPoolDeSilabas(List<(string palabra, List<string> silabas)> palabras)
    {
        List<string> pool = new List<string>();

        foreach((string palabra, List<string> silabas) palabra in palabras)
        {
            pool.AddRange(palabra.silabas);
        }


        var silabasSinRepeticion = new HashSet<string>(pool);

        return new List<string>(silabasSinRepeticion);
    }

    void colocarEnPantallaSilabas()
    {
        foreach(string silaba in poolDeSilabas)
        {
            GameObject palabraAux = nuevaPalabraVacia();
            PalabraController palabraAuxController = palabraAux.GetComponent<PalabraController>();

            palabraAuxController.encontrarPadre();

            SilabaController silabaAux = nuevaSilaba(silaba);
            silabaAux.setPalabra(palabraAux);
            palabraAuxController.nuevaSilabaAlFinal(silabaAux);
        }
    }

    void anunciarPalabrasTarget()
    {
        //larga vida a las tuplas
        EventManager.onPalabrasSeleccionadasParaJuego(palabrasTarget);
    }

    public void ActivarModoRomper()
    {
        modoRomper = true;
    }

    public void DesActivarModoRomper()
    {
        modoRomper = false;
    }

    public void ToggleModoRomper()
    {
        if (_modoRomper)
        {
            EventManager.onModoRomperDesactivado();
        }
        else
        {
            EventManager.onModoRomperActivado();
        }

        _modoRomper = !_modoRomper;

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
        string palabraAux = silaba.getPalabraString().ToUpper();
        PalabraController palabraFormada = silaba.getPalabraController();

        Debug.Log("Palabra formada: ");
        Debug.Log(palabraAux);

        Debug.Log("Palabra target: ");
        Debug.Log(this.palabraActual);

        if(palabraActual.ToUpper() == palabraAux)
        {
            Debug.Log("Palabra formada correctamente");
            Invoke("continuarConSiguientePalabra", 0.5f);

            EventManager.onPalabraFormada(palabraFormada, palabraAux);
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

    private List<(string, List<string>)> generarPalabrasTargetRandom(int v)
    {
        List<(string, List<string>)> palabrasAux = new List<(string, List<string>)>();

        for (int i = 0; i < v; i++)
        {
            palabrasAux.Add(nuevaPalabraRandom());
        }

        return palabrasAux;
    }

    private void colocarEnPantallaPalabra((string,List<string>) palabraTupla)
    {
        nuevaPalabraActual(palabraTupla);
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

    internal PalabraController nuevaPalabraActual((string palabra, List<string> silabas) palabra)
    {
        palabraActual = palabra.palabra;

        GameObject palabraAux = nuevaPalabraVacia();
        PalabraController palabraAuxController = palabraAux.GetComponent<PalabraController>();

        palabraAuxController.encontrarPadre();

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
            if (hijo.gameObject.CompareTag("Palabra")) 
            {
                hijo.gameObject.tag = "PalabraDestruida";
                GameObject.Destroy(hijo.gameObject);
            }
        }
    }

    public void desordenarPalabras()
    {
        List<PalabraController> palabras = new List<PalabraController>();

        foreach(Transform hijo in _juego.transform)
        {   //conseguimos las palabras hijo
            if (hijo.gameObject.CompareTag("Palabra"))
            {
                palabras.Add(hijo.gameObject.GetComponent<PalabraController>());
            }
        }

        foreach(PalabraController palabra in palabras)
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

    #endregion
}
