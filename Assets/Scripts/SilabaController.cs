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
        drag.enableDrag();
    }
    void OnMouseDrag(){
        moviendose = true;
    }

    void OnMouseUp() {
        moviendose = false;
    }

    public void dejarQuietaYQuitarControlDeMouse()
    {
        moviendose = false;
        drag.disableDrag();
    }

}
