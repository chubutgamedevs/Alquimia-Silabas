using System.Collections.Generic;
using UnityEngine;

public class SilabasManager : MonoBehaviour
{
    public GameObject sampleSilaba;

    public GameManager gameManager;


    #region eventos
    void OnEnable()
    {
        EventManager.silabaEsClickeada += manejarClickASilaba;
        EventManager.silabasColisionan += UnirSilabas;
        EventManager.activarConectoresDespuesDe += activarConectoresDespuesDe;
    }

    void OnDisable()
    {
        EventManager.silabaEsClickeada -= manejarClickASilaba;
        EventManager.silabasColisionan -= UnirSilabas;
        EventManager.activarConectoresDespuesDe -= activarConectoresDespuesDe;
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

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    #endregion

    #region metodos
    void UnirSilabas(SilabaController silaba, SilabaController otraSilaba)
    {   //la primer silaba siempre es la que se est� moviendo (checkear eventos)
        PalabraController pController1 = silaba.getPalabraController();
        PalabraController pController2 = otraSilaba.getPalabraController();
    
        silaba.disableDrag();

        float deltaX = pController1.transform.position.x - pController2.transform.position.x;

        float signoDistanciaSilabas = Mathf.Sign(deltaX);

        bool otraSilabaEstaALaIzquierda = signoDistanciaSilabas > 0;

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

        List<SilabaController> todasLasSilabas = izquierda.getSilabasPalabra();
        todasLasSilabas.AddRange(derecha.getSilabasPalabra());

        if (otraSilabaEstaALaIzquierda)
        {
            pController1.setSilabas(todasLasSilabas);
        }
        else
        {
            pController1.setSilabas(todasLasSilabas);
        }

        Destroy(pController2.gameObject);

        izquierda.silabaDerecha = derecha;
        derecha.silabaIzquierda = izquierda;

        izquierda.enableDrag();
        derecha.enableDrag();

        pController1.acomodarSilabasEnElEspacio();

        EventManager.onSilabasUnidas(izquierda, derecha);
    }

    void activarConectoresDespuesDe(List<SilabaController> silabasAux,float t)
    {
        foreach(SilabaController silAux in silabasAux)
        {
            silAux.restablecerConectoresDespuesDe(t);
        }
    }

    void unirPalabra(List<SilabaController> silabasPalabra)
    {
        GameObject palabraObj = gameManager.nuevaPalabraVacia();
        PalabraController palabraController = palabraObj.GetComponent<PalabraController>();

        palabraController.setSilabas(silabasPalabra);


    }

    void manejarClickASilaba(SilabaController silaba)
    {
        if (gameManager.modoRomper)
        {
            silaba.getPalabraController().romperEnSilabasYColocarEnPantalla();
            EventManager.onModoRomperDesactivado();
        }
    }

    #endregion
}
