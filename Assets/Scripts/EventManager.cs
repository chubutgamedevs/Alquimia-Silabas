using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    #region eventos de silabas (plural)
    public static event System.Action<SilabaController, SilabaController> silabasUnidas = delegate { };
    public static event System.Action<SilabaController, SilabaController> silabasSeparadas = delegate { };
    public static event System.Action<SilabaController, SilabaController> silabasColisionan = delegate { };

    #endregion

    #region eventos de silaba (singular)
    public static event System.Action<SilabaController> silabaEsClickeada = delegate { };
    public static event System.Action<SilabaController> silabaEsBajada = delegate { };
    public static event System.Action<SilabaController> silabaSeparadaDeSilaba = delegate { };
    #endregion

    #region eventos de palabras (plural)
    public static event System.Action<PalabraController, PalabraController> palabrasUnidas = delegate { };
    #endregion


    #region eventos de juego general
    public static event System.Action modoRomperActivado = delegate { };
    public static event System.Action modoRomperDesActivado = delegate { };

    #endregion


    #region notificacion de eventos
    public static void onSilabasUnidas(SilabaController s1, SilabaController s2)
    {
        Debug.Log("onSilabasUnidas");
        silabasUnidas(s1, s2);
    }

    public static void onSilabasSeparadas(SilabaController s1, SilabaController s2)
    {
        Debug.Log("onSilabasSeparadas");
        silabasSeparadas(s1, s2);
    }

    public static void onSilabasColisionan(SilabaController s1, SilabaController s2)
    {
        Debug.Log("onSilabasColisionan");
        //el primer argumento es la silaba que se está moviend
        silabasColisionan(s1, s2);
    }

    public static void onSilabaEsClickeada(SilabaController silaba)
    {
        Debug.Log("onSilabaEsClickeada");
        silabaEsClickeada(silaba);
    }

    public static void onSilabaEsBajada(SilabaController silaba)
    {
        Debug.Log("onSilabaEsBajada");
        silabaEsBajada(silaba);
    }

    public static void onModoRomperActivado()
    {
        Debug.Log("onModoRomperActivado");
        modoRomperActivado();
    }
    public static void onModoRomperDesactivado()
    {
        Debug.Log("onModoRomperDesactivado");
        modoRomperDesActivado();
    }

    internal static void onSilabaSeparadaDeSilaba(SilabaController silabaSeparada)
    {
        Debug.Log("onSilabaSeparadaDeSilaba");
        silabaSeparadaDeSilaba(silabaSeparada);
    }

    #endregion
}
