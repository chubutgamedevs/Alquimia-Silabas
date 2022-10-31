using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    #region eventos de silabas (plural)
    public static event System.Action<SilabaController, SilabaController> silabasUnidas = delegate { };
    public static event System.Action<SilabaController, SilabaController> silabasSeparadas = delegate { };
    public static event System.Action<SilabaController, SilabaController> silabasColisionan = delegate { };
    public static event System.Action<List<SilabaController>, float> activarConectoresDespuesDe = delegate { };
    

    #endregion

    #region eventos de silaba (singular)
    public static event System.Action<SilabaController> silabaEsClickeada = delegate { };
    public static event System.Action<SilabaController> silabaEsBajada = delegate { };
    public static event System.Action<SilabaController> silabaSeparadaDeSilaba = delegate { };
    public static event System.Action comprobarBounds = delegate { };

    #endregion

    #region eventos de palabras (plural)
    public static event System.Action<PalabraController, PalabraController> palabrasUnidas = delegate { };
    public static event System.Action<List<PalabraSilabas>> palabrasSeleccionadasParaJuego = delegate { };
    public static event System.Action ordenarPalabras = delegate { };
    public static event System.Action nosQuedamosSinPalabras = delegate { };
    

    #endregion

    #region eventos de palabra (singular)
    public static event System.Action<string, List<string>> palabraSeleccionadaParaJuego = delegate { };
    public static event System.Action<PalabraController, string> palabraFormada = delegate { };
    #endregion


    #region eventos de juego general
    public static event System.Action modoRomperActivado = delegate { };
    public static event System.Action modoRomperDesActivado = delegate { };
    public static event System.Action ganaste = delegate { };

    #endregion


    #region notificacion de eventos
    public static void onSilabasUnidas(SilabaController s1, SilabaController s2)
    {
        silabasUnidas(s1, s2);
    }

    public static void onSilabasSeparadas(SilabaController s1, SilabaController s2)
    {
        silabasSeparadas(s1, s2);
    }

    public static void onSilabasColisionan(SilabaController s1, SilabaController s2)
    {
        //el primer argumento es la silaba que se est� moviend
        silabasColisionan(s1, s2);
    }

    public static void onSilabaEsClickeada(SilabaController silaba)
    {
        silabaEsClickeada(silaba);
    }

    public static void onSilabaEsBajada(SilabaController silaba)
    {
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

    public static void onSilabaSeparadaDeSilaba(SilabaController silabaSeparada)
    {
        silabaSeparadaDeSilaba(silabaSeparada);
    }

    public static void onPalabraFormada(PalabraController palabra, string palabraString)
    {
        palabraFormada(palabra, palabraString);
    }

    public static void onPalabraSeleccionadaParaJuego(string palabra, List<string> silabas)
    {
        palabraSeleccionadaParaJuego(palabra, silabas);
    }

    public static void onPalabrasSeleccionadasParaJuego(List<PalabraSilabas> palabras)
    {
        palabrasSeleccionadasParaJuego(palabras);
    }

    public static void onActivarConectoresDespuesDe(List<SilabaController> silabas, float tiempo)
    {
        activarConectoresDespuesDe(silabas, tiempo);
    }

    public static  void onOrdenarPalabras()
    {
        ordenarPalabras();
    }

    public static void onComprobarBounds()
    {
        comprobarBounds();
    }

    public static void onGanaste()
    {
        ganaste();
    }

    public static void onNosQuedamosSinPalabras()
    {
        nosQuedamosSinPalabras();
    }
    #endregion
}
