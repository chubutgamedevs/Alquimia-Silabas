using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    #region eventos de silabas (plural)
    public static event System.Action<SilabaController, SilabaController> onSilabasUnidas = delegate { };
    public static event System.Action<SilabaController, SilabaController> onSilabasSeparadas = delegate { };
    public static event System.Action<SilabaController, SilabaController> onSilabasColisionan = delegate { };
    public static event System.Action<List<SilabaController>, float> onActivarConectoresDespuesDe = delegate { };
    

    #endregion

    #region eventos de silaba (singular)
    public static event System.Action<SilabaController> onSilabaEsClickeada = delegate { };
    public static event System.Action<SilabaController> onSilabaEsBajada = delegate { };
    public static event System.Action<SilabaController> onSilabaSeparadaDeSilaba = delegate { };
    public static event System.Action comprobarBounds = delegate { };

    #endregion

    #region eventos de palabras (plural)
    public static event System.Action<PalabraController, PalabraController> onPalabrasUnidas = delegate { };
    public static event System.Action<List<PalabraSilabas>> onPalabrasSeleccionadasParaJuego = delegate { };
    public static event System.Action onOrdenarPalabras = delegate { };
    public static event System.Action onNosQuedamosSinPalabras = delegate { };

    public static event System.Action onLimpiarPalabrasObjetivo = delegate { };
    #endregion

    #region eventos de palabra (singular)
    public static event System.Action<string, List<string>> onPalabraSeleccionadaParaJuego = delegate { };
    public static event System.Action<PalabraController, string> onPalabraFormada = delegate { };
    #endregion


    #region eventos de explosiones
    public static event System.Action<Vector3> onMartilloGolpea = delegate { };
    public static event System.Action<Vector3> onExplosionFinJuego = delegate { };
    #endregion

    #region eventos de juego general
    public static event System.Action onModoRomperActivado = delegate { };
    public static event System.Action onModoRomperDesActivado = delegate { };
    public static event System.Action onGanaste = delegate { };

    #endregion

    #region eventos de puntos
    public static event System.Action<Vector3> onPuntoDevuelto = delegate { };
    #endregion



    #region notificacion de eventos
    public static void SilabasUnidas(SilabaController s1, SilabaController s2)
    {
        onSilabasUnidas(s1, s2);
    }

    public static void SilabasSeparadas(SilabaController s1, SilabaController s2)
    {
        onSilabasSeparadas(s1, s2);
    }

    public static void SilabasColisionan(SilabaController s1, SilabaController s2)
    {
        //el primer argumento es la silaba que se est� moviend
        onSilabasColisionan(s1, s2);
    }

    public static void SilabaEsClickeada(SilabaController silaba)
    {
        onSilabaEsClickeada(silaba);
    }

    public static void SilabaEsBajada(SilabaController silaba)
    {
            onSilabaEsBajada(silaba);
    }

    public static void ModoRomperActivado()
    {
        Debug.Log("onModoRomperActivado");
        onModoRomperActivado();
    }
    public static void ModoRomperDesactivado()
    {
        Debug.Log("onModoRomperDesactivado");
        onModoRomperDesActivado();
    }

    public static void SilabaSeparadaDeSilaba(SilabaController silabaSeparada)
    {
        onSilabaSeparadaDeSilaba(silabaSeparada);
    }

    public static void PalabraFormada(PalabraController palabra, string palabraString)
    {
        onPalabraFormada(palabra, palabraString);
    }

    public static void PalabraSeleccionadaParaJuego(string palabra, List<string> silabas)
    {
        onPalabraSeleccionadaParaJuego(palabra, silabas);
    }

    public static void PalabrasSeleccionadasParaJuego(List<PalabraSilabas> palabras)
    {
        onPalabrasSeleccionadasParaJuego(palabras);
    }

    public static void ActivarConectoresDespuesDe(List<SilabaController> silabas, float tiempo)
    {
        onActivarConectoresDespuesDe(silabas, tiempo);
    }

    public static  void OrdenarPalabras()
    {
        onOrdenarPalabras();
    }

    public static void ComprobarBounds()
    {
        comprobarBounds();
    }

    public static void Ganaste()
    {
        onGanaste();
    }

    public static void NosQuedamosSinPalabras()
    {
        onNosQuedamosSinPalabras();
    }

    public static void MartilloGolpea(Vector3 pos)
    {
        onMartilloGolpea(pos);
    }

    public static void ExplotarFinJuego(Vector3 pos)
    {
        onExplosionFinJuego(pos);
    }
    

    public static void PuntoDevuelto(Vector3 punto)
    {
        onPuntoDevuelto(punto);
    }

    public static void LimpiarPalabrasObjetivo()
    {
        onLimpiarPalabrasObjetivo();
    }
    #endregion
}
