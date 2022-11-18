using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MartilloUITween : MonoBehaviour
{
    private IEnumerator coroutine;
    private float anguloRotacion = 20f;
    private float tiempoRotacion = 0.6f;

    #region eventos
    void OnEnable()
    {
        EventManager.modoRomperActivado += handleModoRomperActivado;
        EventManager.modoRomperDesActivado += handleModoRomperDesactivado;
    }

    void OnDisable()
    {
        EventManager.modoRomperActivado -= handleModoRomperActivado;
        EventManager.modoRomperDesActivado -= handleModoRomperDesactivado;
    }
    #endregion
    private void Awake()
    {
        coroutine = animarMartillo();
    }

    #region metodos
    private void handleModoRomperActivado()
    {
        StartCoroutine(coroutine);
    }

    private void handleModoRomperDesactivado()
    {
        StopCoroutine(coroutine);

        if(this.transform == null)
        {
            return;
        }
        Vector3 vectorRotacion = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);

        transform.DOLocalRotate(-vectorRotacion, tiempoRotacion, RotateMode.Fast)
                .SetEase(Ease.Linear);
    }

    IEnumerator animarMartillo()
    {
        while (true)
        {
            if (!(this.transform == null))
            {

            transform.DOLocalRotate(new Vector3(0f, 0f, anguloRotacion), tiempoRotacion, RotateMode.Fast)
                .SetEase(Ease.InExpo);

            yield return new WaitForSeconds(tiempoRotacion);

            transform.DOLocalRotate(new Vector3(0f, 0f, -anguloRotacion), tiempoRotacion, RotateMode.Fast);

            yield return new WaitForSeconds(tiempoRotacion);
            }

        }

    }


    #endregion
}
