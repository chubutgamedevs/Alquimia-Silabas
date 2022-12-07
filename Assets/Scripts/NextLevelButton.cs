using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        if(SceneManager.sceneCountInBuildSettings < SceneManager.GetActiveScene().buildIndex + 1)
        {
            Destroy(this.gameObject);
        }
    }

}
