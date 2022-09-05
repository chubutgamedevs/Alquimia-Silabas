using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool modoRomper=false;
    private bool _modoRomper = false;

    public GameObject palabraPrefab;

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
        EventManager.modoRomperActivado += ActivarModoRomper;
        EventManager.modoRomperDesActivado+= DesActivarModoRomper;

    }

    void OnDisable()
    {
        EventManager.silabasUnidas-= comprobarPalabraFormada;
        EventManager.modoRomperActivado -= ActivarModoRomper;
        EventManager.modoRomperDesActivado -= DesActivarModoRomper;
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
    }

    internal GameObject nuevaPalabra()
    {
        GameObject palabraObj = Instantiate(palabraPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        return palabraObj;
    }
}
