
using UnityEngine;
using DG.Tweening;

public class ScoreManager : MonoBehaviour
{
    static int puntajePorSilaba = 100;
    static int factor = 10;

    int puntajeActual = 0;

    int puntajeGolpeMartillo = 20;

    bool descontandoPuntos = false;

    Color initialColor;

    [SerializeField] TMPro.TextMeshProUGUI textoScore;

    private void OnEnable()
    {
        EventManager.onPalabraFormada += handlePalabraFormada;
        EventManager.onMartilloGolpea += handleMartilloGolpea;
    }

    private void OnDisable()
    {
        EventManager.onPalabraFormada -= handlePalabraFormada;
        EventManager.onMartilloGolpea -= handleMartilloGolpea;
    }

    private void Start()
    {
        initialColor = textoScore.color;

        addPuntaje(0);
    }

    void handlePalabraFormada(PalabraController pal, string _)
    {
        int silabas = pal.silabas.Count;
        int puntaje = silabas * puntajePorSilaba + silabas * puntajePorSilaba / factor;

        addPuntaje(puntaje);

        greenearTextoUnPoquito();

        iniciarDescuentoDePuntos();
    }

    void handleMartilloGolpea(Vector3 _)
    {
        reducirPuntaje(puntajeGolpeMartillo);
    }


    void iniciarDescuentoDePuntos()
    {
        if (!descontandoPuntos) {
            descontandoPuntos = true;
            InvokeRepeating("reducirPuntajeUnPunto", 1f,1f);
        }
    }

    void reducirPuntajeUnPunto()
    {
        addPuntaje(-1);
        reddearTextoUnPoquititito();
    }

    void reducirPuntaje(int puntos)
    {
        addPuntaje(-puntos);
        reddearTextoUnPoquito();
    }

    void addPuntaje(int puntaje)
    {
        puntajeActual += puntaje;

        textoScore.text = "Puntaje: " + puntajeActual.ToString();
    }


    void reddearTextoUnPoquititito()
    {
        textoScore.DOBlendableColor(new Color(0.8f,0.5f,0.5f) , 0.05f)
            .OnComplete(() =>
            {
                textoScore.DOBlendableColor(initialColor, 0.05f);
            }
        ).SetEase(Ease.InCirc);
    }

    void reddearTextoUnPoquito()
    {
        textoScore.DOBlendableColor(Color.red, 0.3f)
            .OnComplete(() =>
            {
                textoScore.DOBlendableColor(initialColor, 0.3f);
            }
        ).SetEase(Ease.InCirc);
    }
    void greenearTextoUnPoquito()
    {
        textoScore.DOBlendableColor(Color.green, 0.3f)
            .OnComplete(() =>
            {
                textoScore.DOBlendableColor(initialColor, 0.3f);
            }
        ).SetEase(Ease.InCirc);
    }
}