using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilabasController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UnirSilabas(SilabaController s1, SilabaController s2)
    {
        //s1 siempre es la que se está moviendo

        //float xOffset = otraSilaba.transform.position.x + (anchoSilaba * Mathf.Sign(silaba.transform.position.x - otraSilaba.transform.position.x)); ;
        //silaba.transform.position = new Vector3(xOffset, otraSilaba.transform.position.y, otraSilaba.transform.position.z);

        ////quitamos el control al usuario
        //silabaController.dejarQuietaYQuitarControlDeMouse();

        ////desactivamos conectores para no conectar en el medio
        //gameObject.SetActive(false);
        //other.gameObject.SetActive(false);
    }
}
