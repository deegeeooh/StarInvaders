using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Levels : MonoBehaviour
{

    [SerializeField] float delayInSeconds = 4f;

   
    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
        FindObjectOfType<GameSession>().AddToLevel();
        
    }

    public void LoadStartScene()

    {
        SceneManager.LoadScene(0);
        FindObjectOfType<GameSession>().ResetGame();                    // Destroy singletonwith scores progres

    }

    public void LoadGameOverScene()

    {
        StartCoroutine(WaitSeconds());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public IEnumerator WaitSeconds()
    {
        yield return new WaitForSeconds(delayInSeconds);
        SceneManager.LoadScene("GameOver");
    }

    public string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }


}
