using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalabraObjetivoController : MonoBehaviour
{
    string palabra = "";
    List<string> silabas;
    public void settearPalabraObjetivo(string palabra, List<string> silabas)
    {
        this.palabra = palabra;
        this.silabas = silabas;

        foreach(string silaba in silabas)
        {
            Debug.Log("silaba " + silaba);
        }
    }
}
