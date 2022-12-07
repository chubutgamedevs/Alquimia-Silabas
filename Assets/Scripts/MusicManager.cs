using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource MusicSource;
    // Start is called before the first frame update
    void Start()
    {
        MusicSource.Play();
    }

    // Update is called once per fram
}
