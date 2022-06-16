using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject PanelAdelante;
    private GameObject Cruz;
    private GameObject Tilde;

    private GameObject GameObjectTexto;
    private UnityEngine.UI.Text Texto;
    private string _texto;

    private Sprite imagenAdelante;


    void OnEnable()
    {
        EventManager.SeSeleccionaPar += ConGelarClickTemporalmente;
    }

    void OnDisable()
    {
        EventManager.SeSeleccionaPar -= ConGelarClickTemporalmente;
    }
    private void Start()
    {
        PanelAdelante = gameObject.transform.GetChild(1).gameObject;
        Tilde = PanelAdelante.transform.GetChild(1).gameObject;
        Cruz = PanelAdelante.transform.GetChild(0).gameObject;
        GameObjectTexto = ObtenerCanvas().transform.GetChild(0).gameObject;
        Texto = GameObjectTexto.GetComponent<UnityEngine.UI.Text>();
    }
    private GameObject ObtenerCanvas()
    {
        return PanelAdelante.transform.GetChild(2).gameObject;
    }
    private void OnMouseDown()
    {
        if (this.tag != "Deshabilitado")
        {
            CongelarClickIndefinidamente();
            Rotar();
        }
    }

    void ConGelarClickTemporalmente()
    {
        // congelamos click
        this.tag = "Deshabilitado";
        // habilitamosClick
        Invoke("HabilitarClick", Globales.TiempoDeMuestraDeCartas);
    }

    void CongelarClickIndefinidamente()
    {
        // congelamos click
        this.tag = "Deshabilitado";
    }

    void HabilitarClick()
    {
        this.tag = "Habilitado";
    }


    public Sprite getImagenAdelante()
    {
        return PanelAdelante.GetComponent<SpriteRenderer>().sprite;
    }

    public void SetearImagenAdelante(Sprite sprite)
    {
        PanelAdelante.GetComponent<SpriteRenderer>().sprite = sprite;
        _texto = sprite.name;
    }

    public void SetearTextoAdelante(string text)
    {
        GameObjectTexto.SetActive(true);
        Texto.text = text;
        _texto = text;
    }

    public string GetName()
    {
        return _texto;
    }
    private GameObject getCarta()
    {
        return gameObject.transform.parent.gameObject;
    }

    private Rotate ScriptDeRotacion()
    {
        return gameObject.GetComponent<Rotate>();
    }

    public void Rotar()
    {
        StartCoroutine(RotarAntesDeTiempo());
    }

    public void Ocultar()
    {
        StartCoroutine(RotarDespuesDeTiempo());
    }

    private IEnumerator RotarAntesDeTiempo()
    {
        EventManager.OnCartaComienzaADescubrirse(this);
        ScriptDeRotacion().StartRotating(Globales.TiempoDeRotacion);
        yield return new WaitForSeconds(Globales.TiempoDeRotacion);
        EventManager.OnCartaDescubierta(this);
        yield return new WaitForSeconds(Globales.TiempoDeMuestraDeCartas - Globales.TiempoDeRotacion);

    }

    private IEnumerator RotarDespuesDeTiempo()
    {
        yield return new WaitForSeconds(Globales.TiempoDeMuestraDeCartas * 3 / 4);
        Cruz.SetActive(true);
        Handheld.Vibrate();
        yield return new WaitForSeconds(Globales.TiempoDeMuestraDeCartas / 4);

        EventManager.OnCartaOcultada(this);
        ScriptDeRotacion().StartRotatingBackwards(Globales.TiempoDeRotacion);
        yield return new WaitForSeconds(Globales.TiempoDeRotacion);
        Cruz.SetActive(false);
        this.HabilitarClick();
    }

    public void destruirse()
    {
        StartCoroutine(DestruirDespuesDeTiempo());
    }

    private IEnumerator DestruirDespuesDeTiempo()
    {
        Tilde.SetActive(true);
        yield return new WaitForSeconds(Globales.TiempoDeMuestraDeCartas);
        Destroy(gameObject);
        EventManager.OnCartaDestruida();
    }


}
