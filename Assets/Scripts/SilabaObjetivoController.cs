using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilabaObjetivoController : MonoBehaviour
{
    string silaba = "si";
    int anchoSilaba = 30;


    public TMPro.TextMeshProUGUI texto;
    void Awake()
    {
        if (!texto)
        {
            texto = getTextMeshPro();
        }
    }
    TMPro.TextMeshProUGUI getTextMeshPro()
    {
        return gameObject.transform.GetChild(1).gameObject.GetComponent<TMPro.TextMeshProUGUI>();
    }

    internal void settearSilaba(string silaba)
    {
        this.silaba = silaba;
        texto.text = this.silaba;
    }

    public void ubicarSilaba(int numSilaba)
    {
        Vector3 position = gameObject.transform.position;
        position.x += numSilaba * anchoSilaba;

        gameObject.transform.position = position;
    }
}
