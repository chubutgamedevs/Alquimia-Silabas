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

    }

    void OnDisable()
    {
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
        conectorHembra.activarConector();
        conectorMacho.activarConector();
    }

    internal void activarConectorDerecho()
    {
        conectorMacho.activarConector();
    }

    internal void activarConectorIzquierdo()
    {
        conectorHembra.activarConector();
    }
}
