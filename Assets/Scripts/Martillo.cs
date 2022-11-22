using UnityEngine;
using DG.Tweening;

public class Martillo : MonoBehaviour
{
    private float anguloRotacion = 40f;
    private float tiempoRotacion = 0.17f;
    private float tiempoVolver = .1f;


    private Vector2 initialPos;


    private BoxCollider2D colisionadorBC;
    #region eventos
    #endregion

    #region ciclo de vida
    private void Awake()
    {
        colisionadorBC = GameObject.FindGameObjectWithTag("colisionador").GetComponent<BoxCollider2D>();
        initialPos = transform.position;
    }
    #endregion

    #region metodos

    private void OnMouseDown()
    {
        EventManager.onModoRomperActivado();    
    }

    private void OnMouseUp()
    {
        martillar();
        EventManager.onModoRomperDesactivado();
    }

    void martillar()
    {
        float tiempoVolverActual = tiempoVolver * Vector2.Distance(initialPos, transform.position) * 0.5f;

            if (!(this.transform == null))
            {
            transform.DOLocalRotate(new Vector3(0f, 0f, anguloRotacion), tiempoRotacion, RotateMode.Fast)
                .SetEase(Ease.InExpo)
                .OnComplete(
                    () =>
                    {
                        EventManager.onMartilloGolpea(colisionadorBC.transform.position);

                        colisionadorBC.isTrigger = true;
                        Invoke("quitarColisionador", tiempoRotacion / 2);

                        transform.DOLocalRotate(new Vector3(0f, 0f, -anguloRotacion), tiempoRotacion, RotateMode.Fast);
                        transform.DOMove(initialPos, tiempoVolverActual).SetEase(Ease.InBack);
                    });
                
            }
    }

    void quitarColisionador()
    {
        colisionadorBC.isTrigger = false;
    }
    #endregion
}