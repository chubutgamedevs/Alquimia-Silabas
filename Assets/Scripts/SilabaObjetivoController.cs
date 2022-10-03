using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilabaObjetivoController : MonoBehaviour
{
    string silaba = "si";
    int anchoSilaba = 30;


    public TMPro.TextMeshPro texto;
    void Start()
    {
        if (!texto)
        {
            texto = getTextMeshPro();
        }
    }
    TMPro.TextMeshPro getTextMeshPro()
    {
        return gameObject.transform.GetChild(1).gameObject.GetComponent<TMPro.TextMeshPro>();
    }

    internal void settearSilaba(string silaba)
    {
        this.silaba = silaba;
        texto.text = this.silaba;
    }

    public void ubicarSilaba(int numSilaba)
    {
        
    }
}
