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

        foreach ((string, List<string>) palabra in palabras)
        {
            PalabraObjetivoController palabraObj = nuevaPalabraObjetivoVacia().GetComponent<PalabraObjetivoController>();
            palabraObj.settearPalabraObjetivo(palabra.Item1,palabra.Item2);

            palabrasObjetivo.Add(palabraObj);
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

        List<(string, List<string>)> palabrasObj = new List<(string, List<string>)>();
        palabrasObj.Add((palabra, silabas));

        settearPalabrasObjetivo(palabrasObj);
    }
}
