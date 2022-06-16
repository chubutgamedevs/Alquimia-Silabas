using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    public static event Action<CardController> SeDescubreCarta = delegate { };
    public static event Action<CardController> CartaComienzaADescubrirse = delegate { };
    public static event Action<CardController> SeOcultaCarta = delegate { };

    public static event Action SeDestruyeCarta = delegate { };

    public static event Action<string> IniciaJuego = delegate { };

    public static event Action SeSeleccionaPar = delegate { };
    public static event Action SeDesSeleccionaPar = delegate { };
    public static event Action<int> JuegoGanado = delegate { };
    public static event Action ParIgual = delegate { };

    // eventos de interfaz
    public static event Action SeClickeaBoton = delegate { };


    public static void OnCartaDestruida()
    {
        Debug.Log("CartaDestruida");
        SeDestruyeCarta();
    }
    public static void OnCartaDescubierta(CardController carta)
    {
        SeDescubreCarta(carta);
    }

    public static void OnCartaComienzaADescubrirse(CardController carta){
        CartaComienzaADescubrirse(carta);
    }

    public static void OnCartaOcultada(CardController carta)
    {
        SeOcultaCarta(carta);
    }

    public static void OnGameStarted(string nivel)
    {
        IniciaJuego(nivel);
    }
    public static void OnParSeleccionado()
    {
        SeSeleccionaPar();
    }

    public static void OnParDesSeleccionado()
    {
        SeDesSeleccionaPar();
    }

    public static void OnGanamosElJuego(int Puntaje)
    {
        Debug.Log("Ganamos el juego!");
        JuegoGanado(Puntaje);
    }

    public static void OnButtonClickeado()
    {
        SeClickeaBoton();
    }

    public static void OnParIgual()
    {
        ParIgual();
    }

}
