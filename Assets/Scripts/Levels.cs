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
    }

    public void LoadStartScene()

    {
        FindObjectOfType<GameSession>().UnLockMouseCursor();
        FindObjectOfType<GameSession>().ResetGame();                    // Destroy singletonwith scores progres
        SceneManager.LoadScene(0);
    }

    public void LoadGameOverScene()

    {

        Debug.Log("SceneCount"+SceneManager.sceneCountInBuildSettings);
        StartCoroutine(WaitSecondsAndLoadScene(SceneManager.sceneCountInBuildSettings - 1));
        
    }

    public void QuitGame()
    {
        FindObjectOfType<GameSession>().UnLockMouseCursor();
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
        // AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
        // Wait until the asynchronous scene fully loads
        //while (!asyncLoad.isDone)
        //{
        //    yield return null;
        //}

    }

    public string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    public void LoadFirstLevel()
    {
        FindObjectOfType<GameSession>().LockMouseCursor();
        SceneManager.LoadScene(1);
        FindObjectOfType<GameSession>().AddToLevel();

    }
}
