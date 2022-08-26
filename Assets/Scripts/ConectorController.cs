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
        //anchoSilaba = silaba.GetComponent<BoxCollider>().bounds.size.x;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("conector"))
        {
            ConectorController otroConector = other.gameObject.GetComponent<ConectorController>();

            if (silabaController.moviendose)
            {   //el primer argumento es la silaba que se está moviendo
                EventManager.onSilabasColisionan(silabaController, otroConector.silabaController);
            }
        }
    }
}

