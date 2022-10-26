using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalabrasObjetivoManager : MonoBehaviour
{
    List<PalabraObjetivoController> palabrasObjetivo;
    public GameObject palabraObjetivoPrefab;


    #region eventos
    void OnEnable()
    {
        EventManager.palabrasSeleccionadasParaJuego += settearPalabrasObjetivo;
        EventManager.palabraFormada += esclarecerPalabra;
    }

    void OnDisable()
    {
        EventManager.palabrasSeleccionadasParaJuego -= settearPalabrasObjetivo;
        EventManager.palabraFormada -= esclarecerPalabra;
    }
    #endregion

    #region ciclo de vida

    private void Start()
    {
        //palabrasObjetivo = obtenerPalabrasObjetivoHijas();
        //destruirPalabrasObjetivoHijas();
        //testSettearPalabraObjetivo(); //testing
    }

    #endregion

    #region setters & getters
    List<PalabraObjetivoController> obtenerPalabrasObjetivoHijas()
    {
        List<PalabraObjetivoController> palabrasObjetivoAux = new List<PalabraObjetivoController>();
        foreach (Transform child in transform)
        {
            PalabraObjetivoController aux = child.gameObject.GetComponent<PalabraObjetivoController>();
            palabrasObjetivoAux.Add(aux);
        }

        return palabrasObjetivoAux;
    }

    public void settearPalabrasObjetivo(List<PalabraSilabas> palabras)
    {
        destruirPalabrasObjetivoHijas();

        int indicePalabra = 0;

        foreach (PalabraSilabas palabra in palabras)
        {
            colocarNuevaPalabraObjetivo(palabra.palabra, palabra.silabas, indicePalabra);

            indicePalabra++;
        }
    }
    #endregion


    #region metodos
    public void esclarecerPalabras()
    {
        foreach (PalabraObjetivoController pal in palabrasObjetivo)
        {
            pal.esclarecerSilabas();
        }
    }

    public void esclarecerPalabra(PalabraController palabra, string palabraAComparar)
    {
        foreach (PalabraObjetivoController pal in palabrasObjetivo)
        {
            if (pal.palabra.ToLower() == palabraAComparar.ToLower())
            {
                pal.esclarecerSilabas();
                palabrasObjetivo.Remove(pal);
                if(palabrasObjetivo.Count == 0)
                {
                    EventManager.onGanaste();
                }
                return;
            }
        }
    }
    public void oscurecerPalabras()
    {
        foreach (PalabraObjetivoController pal in palabrasObjetivo)
        {
            pal.oscurecerSilabas();
        }
    }



    void destruirPalabrasObjetivoHijas()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        transform.DetachChildren();

        palabrasObjetivo = new List<PalabraObjetivoController>();
    }



    internal GameObject nuevaPalabraObjetivoVacia()
    {
        GameObject palabraObj = Instantiate(palabraObjetivoPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        return palabraObj;
    }


    internal void colocarNuevaPalabraObjetivo(string palabra, List<string> silabas, int indicePalabra)
    {
        GameObject palabraObject = nuevaPalabraObjetivoVacia();
        palabraObject.transform.SetParent(this.transform);

        PalabraObjetivoController palabraObj = palabraObject.GetComponent<PalabraObjetivoController>();
        palabraObj.settearPalabraObjetivo(palabra, silabas);
        palabraObj.ubicarPalabra(indicePalabra);

        palabrasObjetivo.Add(palabraObj);
    }

    #endregion

}