using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UINiveles : MonoBehaviour
{
    [SerializeField] GameObject _nextLevelButton;

    // Start is called before the first frame update
    private void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex + 1 == SceneManager.sceneCountInBuildSettings)
        {
            _nextLevelButton.SetActive(false);
        }
    }
    public void goBackToMenu()
    {
        SceneManager.LoadSceneAsync(0);
        GameManager.GetInstance().cleanUp();
    }

    public void nextLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings)
        GameManager.GetInstance().cleanUp();
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
