using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class PalabraObjetivoController : MonoBehaviour
{
    public string palabra = "";

    float anchoPalabra = -35;
    float altoPalabra = 30;

    float baseline = 20;

    [SerializeField] float minFontSize = 20;

    public GameObject SilabaObjetivoPrefab;
    [SerializeField] private Image imagen;

    List<string> silabas;
    List<SilabaObjetivoController> silabasControllers;

    private RectTransform rectTransform;

    #region ciclo de vida

    private void Start()
    {
        //this.imagen = gameObject.transform.GetChild(0);
    }
    private void Awake()
    {
        if (!rectTransform)
        {
            rectTransform = GetComponent<RectTransform>();
        }

        anchoPalabra = rectTransform.rect.width * rectTransform.localScale.x;
        altoPalabra = rectTransform.rect.height;

        settearPosicionInicial();
    }


    #endregion

    #region getters & setters

    void settearPosicionInicial()
    {
        rectTransform.anchoredPosition = new Vector2(0, 0);
    }

    public void settearPalabraObjetivo(string palabra, List<string> silabas)
    {
        this.palabra = palabra;
        this.silabas = silabas;

        settearSilabasControllers();
    }

     public void settearImagen(Sprite imagenRecibida){
        Debug.Log(imagenRecibida.name);
        imagen.sprite = imagenRecibida;
     }


    void settearSilabasControllers()
    {
        eliminarSilabasHijasTransform();

        int indiceSilaba = 0;

        foreach (string silaba in silabas)
        {
            GameObject silAuxObj = nuevaSilabaObjetivoVacia();

            SilabaObjetivoController silAux = silAuxObj.GetComponent<SilabaObjetivoController>();
            silAux.transform.SetParent(this.transform);
            silAux.settearSilaba(silaba);
            silAux.ubicarSilaba(indiceSilaba);

            indiceSilaba++;

            silabasControllers.Add(silAux);
        }

        //hay que esperar a que se acomode la interfaz
        Invoke("settearTamanioFuenteDeTodasLasSilabas", 0.1f);
    }
    void settearTamanioFuenteDeTodasLasSilabas()
    {
        float tamanioFuenteAux = minFontSize;
        foreach (SilabaObjetivoController sil in silabasControllers)
        {
            tamanioFuenteAux = sil.getFontSize();
            if (tamanioFuenteAux < minFontSize)
            {
                minFontSize = tamanioFuenteAux;
            }
        }

        foreach (SilabaObjetivoController sil in silabasControllers)
        {
            sil.setFontSize(minFontSize);
        }
    }

    #endregion

    #region metodos
    void eliminarSilabasHijasTransform()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        silabasControllers = new List<SilabaObjetivoController>();
    }


    #region animaciones
    public void esclarecerSilabas()
    {
        foreach (SilabaObjetivoController sil in silabasControllers)
        {
            sil.esclarecerFondo();
        }
    }

    public void oscurecerSilabas()
    {
        foreach (SilabaObjetivoController sil in silabasControllers)
        {
            sil.oscurecerFondo();
        }
    }


    #endregion

    internal GameObject nuevaSilabaObjetivoVacia()
    {
        GameObject SilabaObj = Instantiate(SilabaObjetivoPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        return SilabaObj;
    }

    public void ubicarPalabra(int numPalabra)
    {
        Vector2 startingPosition = rectTransform.rect.position + new Vector2(0,400);
        Vector2 endPosition = startingPosition;

        endPosition.y = -numPalabra * altoPalabra;
        endPosition.y -= baseline;

        rectTransform.anchoredPosition = startingPosition;
        rectTransform.DOAnchorPos(endPosition, Constants.tiempoAnimacionEntradaPalabraObjetivo).SetEase(Ease.OutExpo) ;
    }
}

#endregion
