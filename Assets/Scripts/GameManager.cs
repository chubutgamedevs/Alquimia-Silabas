using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool modoRomper=false;
    private bool _modoRomper = false;


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
        EventManager.SilabasUnidas += comprobarPalabraFormada;
        EventManager.ModoRomperActivado += ActivarModoRomper;
        EventManager.ModoRomperDesActivado+= DesActivarModoRomper;

    }

    void OnDisable()
    {
        EventManager.SilabasUnidas-= comprobarPalabraFormada;
        EventManager.ModoRomperActivado -= ActivarModoRomper;
        EventManager.ModoRomperDesActivado -= DesActivarModoRomper;
    }
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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


    public void comprobarPalabraFormada(SilabaController silaba, SilabaController otraSilaba)
    {
        List<SilabaController> silabasAux = silaba.getSilabasPalabra();

        Debug.Log("Silabas de la nueva palabra: ");

        foreach (SilabaController sil in silabasAux)
            Debug.Log(sil.silaba);

    }
}
