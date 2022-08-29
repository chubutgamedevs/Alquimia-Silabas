using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilabasController : MonoBehaviour
{
    private float anchoSilaba = 1;
    public GameObject sampleSilaba;


    private void Awake()
    {
        EventManager.SilabasColisionan += UnirSilabas;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (sampleSilaba)
        {
            anchoSilaba = sampleSilaba.GetComponent<BoxCollider>().bounds.size.x;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UnirSilabas(SilabaController s1, SilabaController s2)
    {
        //s1 siempre es la que se está moviendo
        GameObject silaba = s1.gameObject;
        GameObject otraSilaba = s2.gameObject;

        float signoDistanciaSilabas = Mathf.Sign(silaba.transform.position.x - otraSilaba.transform.position.x);

        bool s2EstaALaIzquierda = Mathf.Sign(silaba.transform.position.x - otraSilaba.transform.position.x) > 0;

        float xOffset = otraSilaba.transform.position.x + (anchoSilaba * signoDistanciaSilabas); ;
        silaba.transform.position = new Vector3(xOffset, otraSilaba.transform.position.y, otraSilaba.transform.position.z);

        //quitamos el control al usuario
        s1.dejarQuietaYQuitarControlDeMouse();
        s2.dejarQuietaYQuitarControlDeMouse();

        if (s2EstaALaIzquierda)
        {
            s2.silabaDerecha = s1;
            s1.silabaIzquierda = s2;
        }
        else
        {
            s2.silabaIzquierda = s1;
            s1.silabaDerecha = s2;
        }

        EventManager.onSilabasUnidas(s1, s2);
    }
}
