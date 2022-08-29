using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConectorController : MonoBehaviour
{
    
    private SilabaController silabaController;
    public GameObject silaba;

    float anchoSilaba;

    private void Awake()
    {
        if (!silaba)
        {
            silaba = transform.parent.transform.parent.gameObject;
        }
        silabaController = silaba.GetComponent<SilabaController>();        
    }

    public void desActivarConector()
    {
        gameObject.SetActive(false);
    }
    public void activarConector()
    {
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("conector"))
        {
            ConectorController otroConector = other.gameObject.GetComponent<ConectorController>();

            //de esta manera evitamos que el evento se lance 2 veces
            if (silabaController.moviendose){ 
                //desactivamos conectores para no conectar en el medio
                gameObject.SetActive(false);
                other.gameObject.SetActive(false);

                //el primer argumento es la silaba que se está moviendo
                EventManager.onSilabasColisionan(silabaController, otroConector.silabaController);
            }
        }
    }
}

