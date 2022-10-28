using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConectorController : MonoBehaviour
{
    
    public SilabaController silabaController;

    public void desActivarConector()
    {
        if (gameObject)
        {
            gameObject.SetActive(false);
        }
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

            if(this.gameObject.name != other.gameObject.name)
            {
                //de esta manera evitamos que el evento se lance 2 veces
                if (silabaController.getPalabraController().moviendose){

                    //el primer argumento es la silaba que se está moviendo
                    EventManager.onSilabasColisionan(silabaController, otroConector.silabaController);
                
                    //desactivamos conectores para no conectar en el medio una vez unidas
                    gameObject.SetActive(false);
                    other.gameObject.SetActive(false);
                }
            }
        }
    }
}

