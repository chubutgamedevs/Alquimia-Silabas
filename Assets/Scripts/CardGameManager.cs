using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardGameManager : MonoBehaviour
{
    private CardController[] ParDeCartas = new CardController[2];
    private Sprite[] Sprites;
    private CardController[] TodasLasCartas;
    private int numClicks = 0;
    private int conteoClicksTotal = 0;

    
    void OnEnable()
    {
        EventManager.SeDescubreCarta += CartaClickeada;
        EventManager.IniciaJuego += BarajarTablero;
    }

    void OnDisable()
    {
        EventManager.SeDescubreCarta -= CartaClickeada;
        EventManager.IniciaJuego -= BarajarTablero;
    }


    void InicializarCartas(string nivel)
    {
        // InicializarCartas(nivel, Globales.SabeLeer);
        BarajarTablero(nivel);
    }


    void BarajarTablero(string nivel)
    {
        // Recupera la lista de imagenes
        CargarSprites(nivel);
        List<Sprite> sprites = new List<Sprite>(Sprites);

        // Recuperar la lista de cartas en blanco
        TodasLasCartas = new CardController[transform.childCount];
        List<CardController> cartas = new List<CardController>();
        for (int i = 0; i < transform.childCount; i++)
        {
            cartas.Add(transform.GetChild(i).gameObject.GetComponent<CardController>());
        }

        // Baraja la lista de cartas
        System.Random rnd = new System.Random();
        cartas = cartas.OrderBy(x => rnd.Next()).ToList<CardController>(); 


        // Asigna el sprite
        for (int i = 0; i < transform.childCount; i++)
        {
            CardController c = cartas[i];

            // Si es par pone la imagen
            if (i % 2 == 0)
            {
                c.SetearImagenAdelante(sprites.First());
            }

            // Si es impar evalua si pone el texto.
            if (i % 2 > 0)
            {
                if (Globales.SabeLeer)
                {
                    c.SetearTextoAdelante(sprites.First().name);
                } else
                {
                    c.SetearImagenAdelante(sprites.First());
                }
                sprites.RemoveAt(0);
            }
        }       


    }

    void InicializarCartas(string nivel, bool habilitarTexto)
    {

        CargarSprites(nivel);

        TodasLasCartas = new CardController[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            TodasLasCartas[i] = transform.GetChild(i).gameObject.GetComponent<CardController>();
        }

        if (Sprites.Length < TodasLasCartas.Length / 2)
        {
            Debug.Log("Error, no nos alcanzan los sprites");
        }


        // Se barajan las cartas
        System.Random rnd = new System.Random();
        CardController[] TodasLasCartasRandom = TodasLasCartas.OrderBy(x => rnd.Next()).ToArray();

        //asignando imagen por par
        int lastIndex = TodasLasCartas.Length - 1;
        for (int i = 0; i < TodasLasCartas.Length / 2; i++)
        {
            // los pares no se repiten, por lo que la cantidad de pares es cantidad de cartas /2
            int indiceImagen = Random.Range(i, Sprites.Length);

            TodasLasCartasRandom[i].SetearImagenAdelante(Sprites[indiceImagen]);
            if (habilitarTexto)
            {
                TodasLasCartasRandom[lastIndex - i].SetearTextoAdelante(Sprites[indiceImagen].name);
            }
            else
            {
                TodasLasCartasRandom[lastIndex - i].SetearImagenAdelante(Sprites[indiceImagen]);
            }
        }

    }

    void CargarSprites(string nivel)
    {
        Sprites = Resources.LoadAll<Sprite>("Sprites/" + nivel);
    }

    //Compara si las cartas seleccionadas son iguales o distintas
    public void CartaClickeada(CardController Carta)
    {
        numClicks += 1;
        conteoClicksTotal += 1;
        Debug.Log(conteoClicksTotal);

        Puntaje();

        ParDeCartas[numClicks - 1] = Carta;

        if (numClicks == 2)
        {
            numClicks = 0;

            if (ParDeCartas[0].transform.position.Equals(ParDeCartas[1].transform.position))
            {
                ParDeCartas[0] = null;
                ParDeCartas[1] = null;
                return;
            }

            EventManager.OnParSeleccionado();
            //Cartas iguales
            if (ParDeCartas[0].GetName() == ParDeCartas[1].GetName())
            {
                ParDeCartas[0].destruirse();
                ParDeCartas[1].destruirse();

                EventManager.OnParIgual();

                ComprobarSiGanamos();
            }
            //Cartas distintas
            else
            {
                ParDeCartas[0].Ocultar();
                ParDeCartas[1].Ocultar();
            }

            NotificarParDesSeleccionado();
        }
    }

    public void NotificarParDesSeleccionado()
    {   //hay que esperar a que el par deje de mostrarse, pero solo queremos notificar del evento una sola vez, por esto no se hace dentro de cardcontroller
        StartCoroutine(NotificarQueSeDesSeleccionoPar());
    }
    private IEnumerator NotificarQueSeDesSeleccionoPar()
    {
        yield return new WaitForSeconds(Globales.TiempoDeMuestraDeCartas);
        EventManager.OnParDesSeleccionado();
    }

    public void ComprobarSiGanamos()
    {
        StartCoroutine(ComprobarSiGanamosElJuego());
    }

    private IEnumerator ComprobarSiGanamosElJuego()
    {
        // hay que esperar un tiempo porque hacer esto con eventos no sirve, unity tarda en destruir los objetos
        yield return new WaitForSeconds(Globales.TiempoDeMuestraDeCartas * 1.5f);

        if (transform.childCount == 0)
        {
            EventManager.OnGanamosElJuego(this.Puntaje());
        }

    }

    private int Puntaje()
    {
        int baseMuyBueno = 3;
        int baseBueno = 4;
        int baseMalo = 6;

        int pares = TodasLasCartas.Length / 2;          
        int factor = pares - 0;

        int MuyBuenoAjustado = baseMuyBueno * factor;
        int BuenoAjustado = baseBueno * factor;
        int MaloAjustado = baseMalo * factor;

        int resultado = 0;



        if (conteoClicksTotal == pares * 2)
        {
            Debug.Log("Perfecto");
            resultado = 4;
        }
        else if (conteoClicksTotal <= MuyBuenoAjustado)
        {
            Debug.Log("MuyBueno");
            resultado = 3;
        }
        else if (conteoClicksTotal <= BuenoAjustado)
        {
            Debug.Log("Bueno");
            resultado = 2;
        }
        else if (conteoClicksTotal <= MaloAjustado)
        {
            Debug.Log("Malo");
            resultado = 1;
        }
        else
        {
            resultado = 0;
            Debug.Log("Desinstala el juego");
        }

        return resultado;
    }
}
