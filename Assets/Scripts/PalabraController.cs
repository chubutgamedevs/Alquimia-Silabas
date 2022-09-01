using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PalabraController : MonoBehaviour
{
    List<SilabaController> silabas;

    // Start is called before the first frame update
    void Awake()
    {
        silabas = new List<SilabaController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setSilabas(List<SilabaController> ls)
    {
        if(ls.Count == 0) { return; }

        GameObject padreDeSilabas = ls[0].getPalabra().transform.parent.gameObject;
        this.transform.SetParent(padreDeSilabas.transform);
        foreach(SilabaController sil in ls)
        {
            sil.getPalabraController().flagForDestruction();
            nuevaSilabaAlFinal(sil);
        }
    }

    public void nuevaSilabaAlFinal(SilabaController silaba)
    {
        silaba.transform.SetParent(this.gameObject.transform);
        silabas.Add(silaba);
    }

    public void nuevaSilabaAlPrincipio(SilabaController silaba)
    {
        silaba.setPalabra(this.gameObject);
        silabas.Insert(0,silaba);
    }

    private void imprimirPalabra()
    {
        Debug.Log("palabra formada: " + getPalabraString());
    }

    public string getPalabraString()
    {
        return silabas.Select(x => x.silaba).ToString();
    }

    public void flagForDestruction()
    {
        Invoke("tryToDestroy", 5f);
    }

    private void tryToDestroy()
    {
        if (silabas.Count == 0)
        {
            Destroy(this.gameObject);
        }
    }

}
