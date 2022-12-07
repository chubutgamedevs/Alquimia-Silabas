using UnityEngine;
using DG.Tweening;
using System.Collections;

public class AnimacionSilabasBackground : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("animarSilabas", 1f);
    }

    void animarSilabas()
    {
        foreach (GameObject silaba in GameObject.FindGameObjectsWithTag("silaba"))
        {
            Transform silabaT = silaba.transform;
            float tiempoAnim = Random.Range(10, 100)/10;
            Vector3 randomForce = Random.insideUnitCircle * Random.Range(50, 100)/10;

            silabaT.DOMove(silabaT.position + randomForce, tiempoAnim).SetEase(Ease.InCirc).SetLoops(10000, LoopType.Yoyo);

        }
    }


}
