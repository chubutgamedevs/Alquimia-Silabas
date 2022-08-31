using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilabasManager : MonoBehaviour
{
    private float anchoSilaba = 1;
    public GameObject sampleSilaba;

    public GameManager gameManager;

    // eventos
    void OnEnable()
    {
        EventManager.SilabaEsClickeada += manejarClickASilaba;
        EventManager.SilabasColisionan += UnirSilabas;
    }

    void OnDisable()
    {
        EventManager.SilabaEsClickeada -= manejarClickASilaba;
        EventManager.SilabasColisionan -= UnirSilabas;
    }


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
            anchoSilaba = sampleSilaba.GetComponent<BoxCollider>().bounds.size.x;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UnirSilabas(SilabaController silaba, SilabaController otraSilaba)
    {   //la primer silaba siempre es la que se está moviendo (checkear eventos)
 
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

        //unirPalabras(izquierda.palabra(), derecha.palabra());

        izquierda.silabaDerecha = derecha;
        derecha.silabaIzquierda = izquierda;

        //se envia primero silaba para asegurarnos de que enviamos la que se movio primero (por convencion)
        EventManager.onSilabasUnidas(silaba, otraSilaba);
    }

    void manejarClickASilaba(SilabaController silaba)
    {
        if (gameManager.modoRomper)
        {
            silaba.separarSilaba();
        }
        else
        {
            //mover palabra
        }
    }
}
