using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    //eventos de silabas
    public static event System.Action<SilabaController, SilabaController> SilabasUnidas = delegate { };
    public static event System.Action<SilabaController, SilabaController> SilabasSeparadas = delegate { };
    public static event System.Action<SilabaController, SilabaController> SilabasColisionan = delegate { };

    //eventos de juego general
    public static event System.Action ModoRomperActivado = delegate { };
    public static event System.Action ModoRomperDesActivado = delegate { };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void onSilabasUnidas(SilabaController s1, SilabaController s2)
    {
        Debug.Log("onSilabasUnidas");
        SilabasUnidas(s1, s2);
    }

    public static void onSilabasSeparadas(SilabaController s1, SilabaController s2)
    {
        Debug.Log("onSilabasSeparadas");
        SilabasSeparadas(s1, s2);
    }

    public static void onSilabasColisionan(SilabaController s1, SilabaController s2)
    {
        Debug.Log("onSilabasColisionan");
        //el primer argumento es la silaba que se está moviend
        SilabasColisionan(s1, s2);
    }



}
