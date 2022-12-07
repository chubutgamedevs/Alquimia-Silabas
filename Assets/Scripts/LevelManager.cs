using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        startCurrentLevel();
    }

    void startCurrentLevel()
    {
        GameManager gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();  

        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentLevelIndex > 0)
        {
            gameManager.startGameConPool(currentLevelIndex);
        }
    }

}
