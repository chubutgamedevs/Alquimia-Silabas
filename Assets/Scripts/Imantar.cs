using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Imantar : MonoBehaviour
{
    public GameObject arrastrado;
    private Vector3 distancia;
    private Vector3 aproximado;

    // Start is called before the first frame update
    void Start()
    {
        distancia =  new Vector3(1, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        aproximado = gameObject.transform.position - arrastrado.transform.position;
        if (aproximado.x < distancia.x && aproximado.y < distancia.y)
        {
            arrastrado.transform.position = gameObject.transform.position;
        }
    }
}
