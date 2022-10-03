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

    public GameObject silabasOcultadas;

    public List<(string,List<string>)> palabrasTarget; //tupla go brrr (tupla es el tipo de dato (tipo a, tipo b, ...)
    public string palabraActual = "";

    private GameObject _juego;

    public Dictionary<string, List<string>> palabrasYSilabas;
    List<string> wordList;

    #region ciclo de vida

    // Start is called before the first frame update
    void Start()
    {
        _juego = getJuegoGameObject();
        palabrasTarget = generarPalabrasTargetRandom(4);
        generarSilabasOcultadas();
    }

    private void generarSilabasOcultadas()
    {
        ////destruimos las que haya antes
        //foreach(Transform palOcultada in silabasOcultadas.transform)
        //{
        //    Destroy(palOcultada);
        //}

        ////a�adimos cada palabra target como hijo de silabasOcultadas
        //foreach((string, List<string>) palabraTarget in palabrasTarget)
        //{
        //    //silabasOcultadas.transform.
        //}
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
        colocarEnPantallaPalabra(palabrasTarget[0]);
        desordenarPalabras();
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

    private void cargarPalabrasYSilabas()
    {
        TextAsset json = Resources.Load<TextAsset>("silabas");

        palabrasYSilabas = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json.text);
        wordList = new List<string>(palabrasYSilabas.Keys);
    }

    public void comprobarPalabraFormada(SilabaController silaba, SilabaController otraSilaba)
    {
        string palabraAux = silaba.getPalabraString().ToUpper();

        Debug.Log("Palabra formada: ");
        Debug.Log(palabraAux);

        Debug.Log("Palabra target: ");
        Debug.Log(this.palabraActual);

        if(palabraActual.ToUpper() == palabraAux)
        {
            Debug.Log("Palabra formada correctamente");

            Invoke("continuarConSiguientePalabra", 0.5f);
        }

    }

    private void continuarConSiguientePalabra()
    {
         eliminarTodasLasPalabras();
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

    public void eliminarTodasLasPalabras()
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
