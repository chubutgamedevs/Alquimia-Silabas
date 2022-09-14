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

    public string palabraActual = "";

    private GameObject _juego;

    public Dictionary<string, List<string>> palabrasYSilabas;
    List<string> wordList;

    #region ciclo de vida

    // Start is called before the first frame update
    void Start()
    {

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
        string palabraA = silaba.getPalabraString();
        string palabraB = otraSilaba.getPalabraString();

        Debug.Log("Palabra formada: ");
        Debug.Log(palabraB);
    }

    internal GameObject nuevaPalabra()
    {
        GameObject palabraObj = Instantiate(palabraPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        return palabraObj;
    }

    public void nuevaPalabraRandom()
    {
        int randomIndex = UnityEngine.Random.Range(0, wordList.Count);
        nuevaPalabraActual(wordList[randomIndex], palabrasYSilabas[wordList[randomIndex]]);
        desordenarPalabras();
        
    }

    internal PalabraController nuevaPalabraActual(string palabra, List<string> silabas)
    {
        palabraActual = palabra;

        GameObject palabraAux = nuevaPalabra();
        PalabraController palabraAuxController = palabraAux.GetComponent<PalabraController>();

        palabraAuxController.encontrarPadre();

        foreach (string silaba in silabas) {
            SilabaController silabaAux = nuevaSilaba(silaba);
            silabaAux.setPalabra(palabraAux);
            palabraAuxController.nuevaSilabaAlFinal(silabaAux);
        }


        return palabraAuxController;
    }

    public void desordenarPalabras()
    {
        List<PalabraController> palabras = new List<PalabraController>();

        foreach(Transform hijo in _juego.transform)
        {   //conseguimos las palabras hijo
            palabras.Add(hijo.gameObject.GetComponent<PalabraController>());
        }

        foreach(PalabraController palabra in palabras)
        {   
            //rompemos todas
            palabra.romperEnSilabasYColocarEnPantalla();
        }

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
            _juego = GameObject.FindGameObjectWithTag("juego").transform.gameObject;
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
