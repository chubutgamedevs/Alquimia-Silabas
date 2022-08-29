using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConectoresManager : MonoBehaviour
{
    public ConectorController conectorHembra; 
    public ConectorController conectorMacho;

    private void Awake()
    {
        if (!conectorHembra)
        {
            conectorHembra = gameObject.transform.GetChild(0).GetComponent<ConectorController>();
        }

        if (!conectorMacho)
        {
            conectorMacho = gameObject.transform.GetChild(1).GetComponent<ConectorController>();
        }

    }



    // eventos
    void OnEnable()
    {
        EventManager.ModoRomperActivado += activarConectores;
        EventManager.ModoRomperDesActivado += desActivarConectores;
    }

    void OnDisable()
    {
        EventManager.ModoRomperActivado -= activarConectores;
        EventManager.ModoRomperDesActivado -= desActivarConectores;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {

    }

    internal void desActivarConectores()
    {
        conectorHembra.desActivarConector();
        conectorMacho.desActivarConector();
    }

    internal void activarConectores()
    {
        conectorHembra.desActivarConector();
        conectorMacho.desActivarConector();
    }
    
}
