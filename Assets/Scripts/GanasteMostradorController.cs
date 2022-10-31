using System.Collections;
using UnityEngine;
using DG.Tweening;

public class GanasteMostradorController : MonoBehaviour
{
    public GameObject ga;
    public GameObject nas;
    public GameObject te;

    public Vector3 gaInitialPosition = new Vector3(-10, 0, 0);
    public Vector3 nasInitialPosition = new Vector3(10, 0, 0);
    public Vector3 teInitialPosition = new Vector3(20, 0, 0);

    public Vector3 gaFinalPosition = new Vector3(-1, 0.25f, 0);
    public Vector3 nasFinalPosition = new Vector3(0, 0, 0);
    public Vector3 teFinalPosition = new Vector3(1, -0.25f, 0);


    #region eventos
    void OnEnable()
    {
        EventManager.ganaste += animarFinDelJuego;
    }

    void OnDisable()
    {
        EventManager.ganaste -= animarFinDelJuego;
    }
    #endregion
    public void animarFinDelJuego()
    {
        StartCoroutine(IEanimarFinDelJuego(3f));
    }

    IEnumerator IEanimarFinDelJuego(float duracion)
    {
        yield return new WaitForSeconds(duracion/2);
        ga.transform.DOLocalMove(gaFinalPosition, duracion).SetEase(Ease.OutElastic);
        nas.transform.DOLocalMove(nasFinalPosition, duracion).SetEase(Ease.OutElastic);
        te.transform.DOLocalMove(teFinalPosition, duracion).SetEase(Ease.OutElastic);

        yield return new WaitForSeconds(duracion);
    }

    IEnumerator IEquitarGanasteDePantalla(float duracion)
    {

        yield return new WaitForSeconds(duracion);
    }


}
