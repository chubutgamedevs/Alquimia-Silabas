using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalabrasManager : MonoBehaviour
{
    #region eventos
    void OnEnable()
    {
        EventManager.onPalabraFormada += handlePalabraCorrectaFormada;
    }

    void OnDisable()
    {
        EventManager.onPalabraFormada -= handlePalabraCorrectaFormada;
    }

    #endregion

    #region ciclo de vida
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    public void handlePalabraCorrectaFormada(PalabraController palabraController, string palabra)
    {
        palabraController.handleFormacion();
    }

}
