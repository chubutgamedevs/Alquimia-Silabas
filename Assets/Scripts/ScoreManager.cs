using UnityEngine;
using DG.Tweening;

public class ScoreManager : MonoBehaviour
{
    static int puntajePorSilaba = 100;
    static int factor = 10;

    int puntajeActual = 0;

    [SerializeField] TMPro.TextMeshProUGUI textoScore;

    private void OnEnable()
    {
        EventManager.onPalabraFormada += handlePalabraFormada;
    }

    private void OnDisable()
    {
        EventManager.onPalabraFormada -= handlePalabraFormada;
    }

    private void Start()
    {
        addPuntaje(0);
    }

    void handlePalabraFormada(PalabraController pal, string _)
    {
        int silabas = pal.silabas.Count;
        int puntaje = silabas * puntajePorSilaba + silabas * puntajePorSilaba / factor;

        addPuntaje(puntaje);

        greenearTextoUnPoquito();
    }

    void addPuntaje(int puntaje)
    {
        puntajeActual += puntaje;

        textoScore.text = "Puntaje: " + puntajeActual.ToString();
    }

    void greenearTextoUnPoquito()
    {
        Color initialColor = textoScore.color;
        textoScore.DOBlendableColor(Color.green, 0.3f)
            .OnComplete(() =>
            {
                textoScore.DOBlendableColor(initialColor, 0.3f);
            }
        ).SetEase(Ease.InCirc);
    }
}
