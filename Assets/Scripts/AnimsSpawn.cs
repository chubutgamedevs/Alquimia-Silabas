using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class AnimsSpawn : MonoBehaviour
{
    private string Anim = "Tween";
    private int[] Animacion;

    void Spawn(){
        Anim = "Tween" + (SceneManager.GetActiveScene().buildIndex-1);
        StartCoroutine(Anim);
    }

    void Tween1(){

    }

    void Tween2(){
        
    }

    void Tween3(){
        
    }

    void Tween4(){
        
    }

    void Tween5(){
        
    }
} 
