using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalabrasObjetivoManager : MonoBehaviour
{
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
    public void settearPalabrasObjetivo(List<(string, List<string>)> palabras)
    {
        Debug.Log("hola");
    }
}
