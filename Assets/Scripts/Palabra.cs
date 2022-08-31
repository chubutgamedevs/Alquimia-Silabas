using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Palabra : MonoBehaviour
{
    List<SilabaController> silabas;
    // Start is called before the first frame update
    void Start()
    {
        silabas = new List<SilabaController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void nuevaSilabaAlFinal(SilabaController silaba)
    {
        silabas.Add(silaba);
    }

    public void nuevaSilabaAlPrincipio(SilabaController silaba)
    {
        silabas.Insert(0,silaba);
    }


}
