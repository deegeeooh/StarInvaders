using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Levels : MonoBehaviour
{

    [SerializeField] float delayInSeconds = 4.5f;

   
    public void LoadNextScene()
    {


        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(WaitSecondsAndLoadScene(currentSceneIndex + 1));
        ;
        
            
        
    }

    public void LoadStartScene()

    {
        SceneManager.LoadScene(0);
        FindObjectOfType<GameSession>().ResetGame();                    // Destroy singletonwith scores progres

    }

    public void LoadGameOverScene()

    {

        Debug.Log("SceneCount"+SceneManager.sceneCountInBuildSettings);
        StartCoroutine(WaitSecondsAndLoadScene(SceneManager.sceneCountInBuildSettings - 1));
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public IEnumerator WaitSecondsAndLoadScene(int scene)
    {
        yield return new WaitForSeconds(delayInSeconds);
        SceneManager.LoadScene(scene);
        if (scene < SceneManager.sceneCountInBuildSettings - 1)         // if not GameOver Screen
        {
            FindObjectOfType<GameSession>().AddToLevel();                               // increase level
        }

    }

    public string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    public void LoadFirstLevel()
    {
        SceneManager.LoadScene(1);
        FindObjectOfType<GameSession>().AddToLevel();

    }
}
