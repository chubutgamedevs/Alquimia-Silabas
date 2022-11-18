using UnityEngine;
using DG.Tweening;

public class Explosion : MonoBehaviour
{

    private float eSize = 0.5f;
    private Vector2 explosionSize;
    private float explosionTime = 0.5f;
    // Start is called before the first frame update

    private void Awake()
    {
        explosionSize = new Vector2(eSize, eSize);
    }
    void Start()
    {
        explotar();
    }

    void explotar()
    {
        transform.DOScale(explosionSize, explosionTime / 2).SetEase(Ease.InCubic)
            .OnComplete(() => transform.DOScale(new Vector2(0,0), explosionTime/2).SetEase(Ease.OutBack))
            .OnComplete(() => Destroy(this.gameObject));
    }
}
