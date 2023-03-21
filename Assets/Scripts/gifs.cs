using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gifs : MonoBehaviour
{
    public Sprite[] frames;
    private int framesPerSecond = 10;

    void Update(){
    
        int index = ((int)((Time.time * framesPerSecond) % frames.Length));
        GetComponent<SpriteRenderer>().sprite = frames[index];  
        if (frames[index] = frames[0]){
            Destroy(this);
            Debug.Log("romper");
        }
    }
}
