using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SilabaObjetivoController : MonoBehaviour
{
    string silaba = "si";
    float anchoSilaba = 60;

    public Animator miAnimator;

    public TMPro.TextMeshProUGUI texto;
    public GameObject fondoObj;
    Image fondo;

    #region ciclo de vida

    void Awake()
    {
        RectTransform rectTransform = this.gameObject.GetComponent<RectTransform>();
        anchoSilaba = rectTransform.rect.width * rectTransform.localScale.x ;        
    }

    private void Start()
    {
        oscurecerFondo();
    }


    #endregion

    #region getters & setters


    internal void settearSilaba(string silaba)
    {
        this.silaba = silaba;
        texto.text = this.silaba;
    }

    public void setFontSize(float size)
    {
        this.texto.fontSizeMax = size;
        this.texto.fontSizeMin = size;
    }
    public float getFontSize()
    {
        return this.texto.fontSize;
    }


    #endregion

    #region metodos


    public void oscurecerFondo()
    {
        miAnimator.Play("oscurecer");
    }
    public void esclarecerFondo()
    {
        miAnimator.Play("esclarecer");
    }



    public void ubicarSilaba(int numSilaba)
    {
        Vector3 position = gameObject.transform.position;
        position.x += numSilaba * anchoSilaba;

        gameObject.transform.position = position;
    }


    #endregion




}
