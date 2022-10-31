using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalabrasManager : MonoBehaviour
{

    List<PalabraSilabas> poolDeSilabas;

    #region eventos
    void OnEnable()
    {
        EventManager.palabraFormada += handlePalabraCorrectaFormada;
        EventManager.palabrasSeleccionadasParaJuego += handlePalabrasSeleccionadasParaJuego;
    }

    void OnDisable()
    {
        EventManager.palabraFormada -= handlePalabraCorrectaFormada;
        EventManager.palabrasSeleccionadasParaJuego -= handlePalabrasSeleccionadasParaJuego;
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

    void handlePalabraCorrectaFormada(PalabraController palabraController, string palabra)
    {
        palabraController.desactivarConectores();
        palabraController.disableDrag();
        palabraController.playAnimacionPalabraCorrecta();

        PalabraSilabas found  = poolDeSilabas.Find(x => x.palabra == palabraController.getPalabraString());
        poolDeSilabas.Remove(found);

        List<SilabaController> silabasAEliminar = new List<SilabaController>();

        foreach(SilabaController sil in palabraController.silabas)
        {
            bool silabaEstaEnOtraPalabra = false;
            foreach(PalabraSilabas pal in poolDeSilabas)
            {
                silabaEstaEnOtraPalabra = pal.contieneSilaba(sil.silaba);
                if (silabaEstaEnOtraPalabra)
                {
                    break;
                }
            }

            if (!silabaEstaEnOtraPalabra)
            {
                silabasAEliminar.Add(sil);
            }
        }

        palabraController.eliminarSilabasLuegoDeFormacion(silabasAEliminar);
    }

    void handlePalabrasSeleccionadasParaJuego(List<PalabraSilabas> target)
    {
        this.poolDeSilabas = target;
    }
}
