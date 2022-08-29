using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SilabaController : MonoBehaviour
{
    private SilabaController silabaIzquierda;
    private SilabaController silabaDerecha;

    private ConectoresManager conectoresManager;


    private Drag drag;
    public bool moviendose = false;
    
    private void Awake() {
        drag = gameObject.GetComponent<Drag>();
    }
    // Start is called before the first frame update
    void Start()
    {
        conectoresManager = getConectoresManager();

        silabaIzquierda = null;
        silabaDerecha = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    ConectoresManager getConectoresManager(){
        return gameObject.transform.GetChild(2).gameObject.GetComponent<ConectoresManager>();
    }


    private void OnMouseDown()
    {
        conectoresManager.desActivarConectores();
        acercarAPantalla();
    }
    void OnMouseDrag(){
        moviendose = true;
    }

    void OnMouseUp() {
        moviendose = false;
        conectoresManager.activarConectores();
        colocarEnPosicionNormalRelativaAPantalla();
    }

    private void acercarAPantalla()
    {
        //Vector3 posNueva = transform.position;
        //posNueva.z -= 1;
        //transform.position = posNueva;
    }

    private void colocarEnPosicionNormalRelativaAPantalla()
    {
        //Vector3 posNueva = transform.position;
        //posNueva.z = 0;
        //transform.position = posNueva;
    }


    public void dejarQuietaYQuitarControlDeMouse()
    {
        moviendose = false;
        drag.disableDrag();
    }

    public void disableDrag()
    {
        drag.enabled = false;
    }
    public void enableDrag()
    {
        drag.enabled = true;
    }

}
