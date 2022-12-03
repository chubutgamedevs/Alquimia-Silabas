using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UINiveles : MonoBehaviour
{
    // Start is called before the first frame update
    public void goBackToMenu()
    {
        SceneManager.LoadSceneAsync(0);
        GameManager.GetInstance().cleanUp();
    }

    public void nextLevel()
    {
        GameManager.GetInstance().cleanUp();
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
