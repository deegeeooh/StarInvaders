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
    }

    public void LoadStartScene()

    {
        SceneManager.LoadScene(0);

    }

    public void LoadGameOverScene()

    {
        StartCoroutine(Wacht());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public IEnumerator Wacht()
    {
        yield return new WaitForSeconds(delayInSeconds);
        SceneManager.LoadScene("GameOver");
    }


}
