using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConectorController : MonoBehaviour
{
    
    private SilabaController silabaController;

    float anchoSilaba;

    public GameObject silaba;

    private void Awake()
    {
        if (!silaba)
        {
            silaba = transform.parent.transform.parent.gameObject;
        }
        silabaController = silaba.GetComponent<SilabaController>();
    }

    private void Start()
    {

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("conector"))
        {
            ConectorController otroConector = other.gameObject.GetComponent<ConectorController>();

            if (silabaController.moviendose)
                //desactivamos conectores para no conectar en el medio
                gameObject.SetActive(false);
                other.gameObject.SetActive(false);

            {   //el primer argumento es la silaba que se está moviendo
                EventManager.onSilabasColisionan(silabaController, otroConector.silabaController);

            }
        }
    }
}

