using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalabrasObjetivoManager : MonoBehaviour
{
    List<PalabraObjetivoController> palabrasObjetivo;
    public GameObject palabraObjetivoPrefab;

    private void Start()
    {
        palabrasObjetivo = obtenerPalabrasObjetivoHijas();
        destruirPalabrasObjetivoHijas();
        testSettearPalabraObjetivo();
    }


    public void esclarecerPalabras()
    {
        foreach(PalabraObjetivoController pal in palabrasObjetivo)
        {
            pal.esclarecerSilabas();
        }
    }

    public void oscurecerPalabras()
    {
        foreach (PalabraObjetivoController pal in palabrasObjetivo)
        {
            pal.oscurecerSilabas();
        }
    }


    List<PalabraObjetivoController> obtenerPalabrasObjetivoHijas()
    {
        List<PalabraObjetivoController> palabrasObjetivoAux = new List<PalabraObjetivoController>();
        foreach(Transform child in transform)
        {
            PalabraObjetivoController aux = child.gameObject.GetComponent<PalabraObjetivoController>();
            palabrasObjetivoAux.Add(aux);
        }

        return palabrasObjetivoAux;
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

    public void settearPalabrasObjetivo(List<(string, List<string>)> palabras)
    {
        destruirPalabrasObjetivoHijas();

        int indicePalabra = 0;

        foreach ((string, List<string>) palabra in palabras)
        {
            GameObject palabraObject = nuevaPalabraObjetivoVacia();
            palabraObject.transform.SetParent(this.transform);

            PalabraObjetivoController palabraObj = palabraObject.GetComponent<PalabraObjetivoController>();
            palabraObj.settearPalabraObjetivo(palabra.Item1,palabra.Item2);
            palabraObj.ubicarPalabra(indicePalabra);

            palabrasObjetivo.Add(palabraObj);

            indicePalabra++;
        }
    }


    internal GameObject nuevaPalabraObjetivoVacia()
    {
        GameObject palabraObj = Instantiate(palabraObjetivoPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        return palabraObj;
    }

    public void testSettearPalabraObjetivo()
    {
        string palabra = "mañana";
        List<string> silabas = new List<string>();
        silabas.Add("ma");
        silabas.Add("ña");
        silabas.Add("na");

        string palabra1 = "silabas";
        List<string> silabas1 = new List<string>();
        silabas1.Add("si");
        silabas1.Add("la");
        silabas1.Add("bas");

        List<(string, List<string>)> palabrasObj = new List<(string, List<string>)>();

        palabrasObj.Add((palabra, silabas));
        palabrasObj.Add((palabra1, silabas1));

        settearPalabrasObjetivo(palabrasObj);
    }
}
