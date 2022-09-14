using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class SilabasManager : MonoBehaviour
{
    private float anchoSilaba = 1;
    public GameObject sampleSilaba;

    public GameManager gameManager;


    #region eventos
    void OnEnable()
    {
        EventManager.silabaEsClickeada += manejarClickASilaba;
        EventManager.silabasColisionan += UnirSilabas;
    }

    void OnDisable()
    {
        EventManager.silabaEsClickeada -= manejarClickASilaba;
        EventManager.silabasColisionan -= UnirSilabas;
    }

    #endregion

    #region ciclo de vida

    private void Awake()
    {
        if (!gameManager)
        {
            gameManager = GameManager.GetInstance();
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        if (sampleSilaba)
        {
            anchoSilaba = sampleSilaba.GetComponent<BoxCollider>().size.x;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    #endregion

    #region metodos
    void UnirSilabas(SilabaController silaba, SilabaController otraSilaba)
    {   //la primer silaba siempre es la que se está moviendo (checkear eventos)
        silaba.getPalabraController().dejarQuieta();
        otraSilaba.getPalabraController().dejarQuieta();

        float deltaX = silaba.transform.position.x - otraSilaba.transform.position.x;

        float signoDistanciaSilabas = Mathf.Sign(deltaX);

        bool otraSilabaEstaALaIzquierda = signoDistanciaSilabas > 0;

        float xOffset = otraSilaba.transform.position.x + (anchoSilaba * signoDistanciaSilabas); ;
        silaba.transform.position = new Vector3(xOffset, otraSilaba.transform.position.y, otraSilaba.transform.position.z);

        //quitamos el control al usuario
        silaba.dejarQuietaYQuitarControlDeMouse();
        otraSilaba.dejarQuietaYQuitarControlDeMouse();

        SilabaController izquierda = silaba;
        SilabaController derecha = otraSilaba;

        if (otraSilabaEstaALaIzquierda)
        {
            izquierda = otraSilaba;
            derecha = silaba;
        }

        //unimos palabra
        PalabraController palabra = izquierda.getPalabraController();
        palabra.aniadirPalabraAlFinal(derecha.getSilabasPalabra());
       
        izquierda.silabaDerecha = derecha;
        derecha.silabaIzquierda = izquierda;

        izquierda.enableDrag();
        derecha.enableDrag();

        EventManager.onSilabasUnidas(izquierda,derecha);
        
    }

    void unirPalabra(List<SilabaController> silabasPalabra)
    {
        GameObject palabraObj = gameManager.nuevaPalabra();
        PalabraController palabraController = palabraObj.GetComponent<PalabraController>();

        palabraController.setSilabas(silabasPalabra);


    }

    void manejarClickASilaba(SilabaController silaba)
    {
        if (gameManager.modoRomper)
        {
            silaba.separarSilabaDeOtrasSilabas();
        }
    }

    #endregion
}
