using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Levels : MonoBehaviour
{
    // initialise variables
    [SerializeField] float delayInSeconds = 4.5f;
  
    //init
    int currentGameLoop;
    bool numeratingNextLevel = false;
    bool goingtoNextGameLoop = false;
    

    // cache references
    GameSession gamesession;



    
    private void Start()
    {
        gamesession = FindObjectOfType<GameSession>();
        currentGameLoop = gamesession.GetCurrentGameLoop();
        

        //Debug.Log("SceneLoader gameloop:  " + currentGameLoop);
    }


    public void LoadNextScene()
    {
        currentGameLoop = gamesession.GetCurrentGameLoop();

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        //Debug.Log("Levels: currentSceneIndex: " + currentSceneIndex);

        if (currentSceneIndex + 1 == SceneManager.sceneCountInBuildSettings - 1) // We are entering the GameOver Screen but haven't died
        {
            if (gamesession.Loopgame() && currentGameLoop < 3)
            {
                if (goingtoNextGameLoop == false)           // so CheckRemainingEnemies won't keep executing this while in Coroutine
                {
                    goingtoNextGameLoop = true;
                    StartCoroutine(StartNewGameLoop(1));
                    ////StartCoroutine(WaitSecondsAndLoadScene(1));
                    //goingtoNextGameLoop = false;
                }
                

            }
            else
            {
                if (numeratingNextLevel == false)
                {
                    numeratingNextLevel = true;
                    StartCoroutine(WaitSecondsAndLoadScene(currentSceneIndex + 1));
                    
                }
            }
        }
        else
        {
            if (numeratingNextLevel == false)
            {
                numeratingNextLevel = true;
                StartCoroutine(WaitSecondsAndLoadScene(currentSceneIndex + 1));
                
            }
                
        }
        
    }

    public void LoadStartScene()

    {
        FindObjectOfType<GameSession>().UnLockMouseCursor();
        FindObjectOfType<GameSession>().ResetGame();                    // Destroy singletonwith scores progres
        FindObjectOfType<GoldPot>().ResetGoldPot();
        FindObjectOfType<RandomPot>().ResetRandomPot();
        FindObjectOfType<BackgroundScroller>().Reset();                 // Destroy singleton 

        SceneManager.LoadScene(0);
    }

    public void LoadGameOverScene()

    {
        Debug.Log("SceneCount "+SceneManager.sceneCountInBuildSettings);
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
        if (scene < SceneManager.sceneCountInBuildSettings - 1)         // if not GameOver Screen
        {
            FindObjectOfType<GameSession>().AddToLevel();               // increase level
        }
        gamesession.ResetEnemiesAndSpawnersInLevel();
        SceneManager.LoadScene(scene);
    }

    public string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    public void LoadFirstLevel()
    {
        FindObjectOfType<GameSession>().LockMouseCursor();
        FindObjectOfType<GameSession>().SetLevel(1);
        gamesession.ResetEnemiesAndSpawnersInLevel();
        SceneManager.LoadScene(1);
    }

    public IEnumerator StartNewGameLoop(int num)            // just for loading new gameloop, no addlevel> gamesession.addtogameloop setlevel(1)
    {
        yield return new WaitForSeconds(delayInSeconds);
        gamesession.ResetEnemiesAndSpawnersInLevel();
        gamesession.AddToGameLoop();
        goingtoNextGameLoop = false;
        SceneManager.LoadScene(num);
    }



}
