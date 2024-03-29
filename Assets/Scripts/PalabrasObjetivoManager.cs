using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class PalabrasObjetivoManager : MonoBehaviour
{
    List<PalabraObjetivoController> palabrasObjetivo;
    public GameObject palabraObjetivoPrefab;

    public List<Sprite> imageBuffer = new List<Sprite>();

    private void Awake() {
        //Cargar todo archivo Texture2D en Resources/Imagenes/imagenesPalabras
        Sprite[] textures = Resources.LoadAll<Sprite>("imagenes/imagenesPalabras");
        for (int i = 0; i < textures.Length; i++)
        {
            imageBuffer.Add(textures[i]);
        } 
    }


    #region eventos
    void OnEnable()
    {
        EventManager.onPalabrasSeleccionadasParaJuego += settearPalabrasObjetivo;
        EventManager.onPalabraFormada += esclarecerPalabra;
        EventManager.onLimpiarPalabrasObjetivo += limpiarTodo;

    }

    void OnDisable()
    {
        EventManager.onPalabrasSeleccionadasParaJuego -= settearPalabrasObjetivo;
        EventManager.onPalabraFormada -= esclarecerPalabra;
        EventManager.onLimpiarPalabrasObjetivo -= limpiarTodo;
    }
    #endregion

    #region ciclo de vida

    private void Start()
    {
        palabrasObjetivo = new List<PalabraObjetivoController>();
    }

    #endregion

    #region setters & getters
    List<PalabraObjetivoController> obtenerPalabrasObjetivoHijas()
    {
        List<PalabraObjetivoController> palabrasObjetivoAux = new List<PalabraObjetivoController>();
        foreach (Transform child in transform)
        {
            PalabraObjetivoController aux = child.gameObject.GetComponent<PalabraObjetivoController>();
            palabrasObjetivoAux.Add(aux);
        }

        return palabrasObjetivoAux;
    }

    public void settearPalabrasObjetivo(List<PalabraSilabas> palabras)
    {
        StartCoroutine(IEcolocarNuevasPalabrasObjetivo(palabras));
    }
    #endregion


    #region metodos

    void limpiarTodo()
    {
        this.palabrasObjetivo = new List<PalabraObjetivoController>();
        GameObject[] palabrasObjetivo = GameObject.FindGameObjectsWithTag("PalabraObjetivo");

        foreach(GameObject pal in palabrasObjetivo)
        {
            Destroy(pal);
        }
    }

    public void esclarecerPalabras()
    {
        foreach (PalabraObjetivoController pal in palabrasObjetivo)
        {
            pal.esclarecerSilabas();
        }
    }
    public void esclarecerPalabra(PalabraController palabra, string palabraAComparar)
    {
        PalabraObjetivoController pal = palabrasObjetivo.Find(x => x.palabra.ToLower() == palabraAComparar.ToLower());

        if (pal)
        {
            pal.esclarecerSilabas();
            palabrasObjetivo.Remove(pal);
            Debug.Log("Palabra correcta");
        }
    }
    public void oscurecerPalabras()
    {
        foreach (PalabraObjetivoController pal in palabrasObjetivo)
        {
            pal.oscurecerSilabas();
        }
    }


    IEnumerator IEcolocarNuevasPalabrasObjetivo(List<PalabraSilabas> palabrasNuevas)
    {
        if(palabrasNuevas != null)
        {
            if(palabrasNuevas.Count != 0)
            {
                GameObject[] palabrasObjetivoActuales = GameObject.FindGameObjectsWithTag(Constants.tagPalabraObjetivo);

                if (palabrasObjetivoActuales.Length > 0)
                {

                    foreach (GameObject palabraOculta in palabrasObjetivoActuales)
                    {
                        Vector3 posNueva = palabraOculta.transform.position;
                        posNueva += new Vector3(500, 0, 0);

                        if (palabraOculta.transform != null)
                        {

                            palabraOculta.transform.DOMove(posNueva, Constants.tiempoAnimacionSalidaPalabraObjetivo);
                        }
                    }

                    yield return new WaitForSeconds(Constants.tiempoAnimacionSalidaPalabraObjetivo);

                    foreach (GameObject palabraOculta in palabrasObjetivoActuales)
                    {
                        Destroy(palabraOculta);
                    }

                    transform.DetachChildren();
                }

                int indicePalabra = 0;

                foreach (PalabraSilabas palabra in palabrasNuevas)
                {
                    colocarNuevaPalabraObjetivo(palabra.palabra, palabra.silabas, indicePalabra);

                    indicePalabra++;
                }
            }
        }
    }

    internal GameObject nuevaPalabraObjetivoVacia()
    {
        GameObject palabraObj = Instantiate(palabraObjetivoPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        
        return palabraObj;
    }


    internal void colocarNuevaPalabraObjetivo(string palabra, List<string> silabas, int indicePalabra)
    {
        GameObject palabraObject = nuevaPalabraObjetivoVacia();
        palabraObject.transform.SetParent(this.transform);

        PalabraObjetivoController palabraObj = palabraObject.GetComponent<PalabraObjetivoController>();
        palabraObj.settearPalabraObjetivo(palabra, silabas);
        palabraObj.ubicarPalabra(indicePalabra);
        palabraObj.settearImagen(imageBuffer.Find(x => x.name.ToLower() == palabra.ToLower()));

        palabrasObjetivo.Add(palabraObj);

    }
    #endregion

}