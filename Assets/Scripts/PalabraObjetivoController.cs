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

        settearSilabasControllers();
    }

    void settearSilabasControllers()
    {
        eliminarSilabasHijasTransform();

        int indiceSilaba = 0;

        foreach(string silaba in silabas)
        {
            GameObject silAuxObj = nuevaSilabaObjetivoVacia();
            
            SilabaObjetivoController silAux = silAuxObj.GetComponent<SilabaObjetivoController>();
            silAux.transform.SetParent(this.transform);
            silAux.settearSilaba(silaba);
            silAux.ubicarSilaba(indiceSilaba);

            indiceSilaba++;

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
