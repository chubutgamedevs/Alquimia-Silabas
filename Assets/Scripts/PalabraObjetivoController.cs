using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalabraObjetivoController : MonoBehaviour
{
    string palabra = "";

    public GameObject SilabaObjetivoPrefab;

    List<string> silabas;
    List<SilabaObjetivoController> silabasControllers;
    public void settearPalabraObjetivo(string palabra, List<string> silabas)
    {
        this.palabra = palabra;
        this.silabas = silabas;
    }

    void settearSilabasControllers()
    {
        eliminarSilabasHijasTransform();

        foreach(string silaba in silabas)
        {
            SilabaObjetivoController silAux = nuevaSilabaObjetivoVacia().GetComponent<SilabaObjetivoController>();
            silAux.settearSilaba(silaba);
            silabasControllers.Add(silAux);
        }
    }

    void eliminarSilabasHijasTransform()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        silabasControllers = new List<SilabaObjetivoController>();
    }

    internal GameObject nuevaSilabaObjetivoVacia()
    {
        GameObject SilabaObj = Instantiate(SilabaObjetivoPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        return SilabaObj;
    }
}
