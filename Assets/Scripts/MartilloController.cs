using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MartilloController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hola");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hallo");
    }
}
