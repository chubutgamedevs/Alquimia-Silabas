using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuntajeManager : MonoBehaviour
{
    private GameObject[] estrellas;

    // Start is called before the first frame update
    void Start()
    {
        IniciarEstrellas();
        StartCoroutine(MostrarEstrellasDespues());

    }

    public void iniciarAnimacionIrseDeNivel()
    {
        Animator animator = gameObject.GetComponent<Animator>();
        animator.Play("irse de escena");
    }
    public void MostrarEstrellas()
    {
        int puntaje = gamemanager.puntaje;

        for (int i = 0; i < puntaje; i++)
        {
            StartCoroutine(ActivarEstrella(i));
        }
    }

    private IEnumerator MostrarEstrellasDespues()
    {
        yield return new WaitForSeconds(2);
        MostrarEstrellas();

    }

    private IEnumerator ActivarEstrella(int numEstrella)
    {
        yield return new WaitForSeconds(numEstrella);
        estrellas[numEstrella].SetActive(true);
    }

    void IniciarEstrellas()
    {
        estrellas = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            estrellas[i] = transform.GetChild(i).gameObject;
        }
    }

}
