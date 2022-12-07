using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GanasteMostradorController : MonoBehaviour
{
    public GameObject ga;
    public GameObject nas;
    public GameObject te;

    private Vector3 gaInitialPosition = new Vector3(-10, 0, 0);
    private Vector3 nasInitialPosition = new Vector3(10, 0, 0);
    private Vector3 teInitialPosition = new Vector3(20, 0, 0);

    private Vector3 gaFinalPosition = new Vector3(-1.5f, 0.25f, 0);
    private Vector3 nasFinalPosition = new Vector3(0, 0, 0);
    private Vector3 teFinalPosition = new Vector3(1.5f, -0.25f, 0);


    private List<Vector3> explosiones = new List<Vector3> {
        new Vector3(1.5f,1),
        new Vector3(3.5f,0.2f),
        new Vector3(6f,7f),
        new Vector3(2.5f,6f),
        new Vector3(10f,6f),
        new Vector3(10.5f,2.5f)
    };

    #region eventos
    void OnEnable()
    {
        EventManager.onGanaste += animarFinDelJuego;
    }

    void OnDisable()
    {
        EventManager.onGanaste -= animarFinDelJuego;
    }
    #endregion
    public void animarFinDelJuego()
    {
        StartCoroutine(IEanimarFinDelJuego(0.8f));
    }

    IEnumerator IEanimarFinDelJuego(float duracion)
    {
        yield return new WaitForSeconds(duracion);
        ga.transform.DOLocalMove(gaFinalPosition, duracion / 3).SetEase(Ease.OutElastic);
        nas.transform.DOLocalMove(nasFinalPosition, duracion / 2).SetEase(Ease.OutElastic);
        te.transform.DOLocalMove(teFinalPosition, duracion).SetEase(Ease.OutElastic)
            .OnComplete(() => {
                hacerQueExploteTodo();
            });
    }

    void hacerQueExploteTodo()
    {
        foreach(Vector3 punto in explosiones)
        {
            float tiempoExplosion = Random.Range(0.1f, 0.3f);
            Invoke("explotar", tiempoExplosion);
        }
    }

    void explotar()
    {
        int index = Random.Range(0, explosiones.Count);
        EventManager.ExplotarFinJuego(explosiones[index]);
        explosiones.RemoveAt(index);
    }

    void quitarGanasteDePantalla(float duracion)
    {
        ga.transform.DOLocalMove(gaInitialPosition, duracion).SetEase(Ease.OutElastic);
        nas.transform.DOLocalMove(nasInitialPosition, duracion).SetEase(Ease.OutElastic);
        te.transform.DOLocalMove(teInitialPosition, duracion).SetEase(Ease.OutElastic);
    }


}
