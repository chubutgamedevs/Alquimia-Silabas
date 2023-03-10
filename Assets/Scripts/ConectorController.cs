using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConectorController : MonoBehaviour
{
    
    public SilabaController silabaController;
    bool activo = true;

    public void desActivarConector()
    {
        if (gameObject)
        {
            gameObject.SetActive(false);
            activo = false;
        }
    }
    public void activarConector()
    {
        gameObject.SetActive(true);
        activo = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!this.activo)
        {
            return;
        }

        ConectorController otroConector = other.gameObject.GetComponent<ConectorController>();

        if(this.gameObject.tag != other.gameObject.tag)
        {
            //de esta manera evitamos que el evento se lance 2 veces
            if (silabaController.getPalabraController().moviendose){

                //el primer argumento es la silaba que se est� moviendo
                EventManager.SilabasColisionan(silabaController, otroConector.silabaController);

                //desactivamos conectores para no conectar en el medio una vez unidas
                this.desActivarConector();
                otroConector.desActivarConector();
            }
        }
        
    }
}

