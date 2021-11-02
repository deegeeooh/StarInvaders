using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
public class GameSession : MonoBehaviour
{
      
    [SerializeField] int highScore;
    [SerializeField] int currentScore = 0;
    [SerializeField] int currentLevel = 0;
    [SerializeField] int numberOfKills = 0;
    [SerializeField] int healthRemaining = 600;
    [SerializeField] int numberOfShotsFired = 0;
    [SerializeField] int numberOfHits = 0;
    [SerializeField] int pointsPerHit = 5;
    [SerializeField] int escapedEnemies = 0;
    [SerializeField] int goldValue = 0;
    [Header("Settings")]
    [SerializeField] bool music = true;
    [SerializeField] bool playerInvulnarable = false;
    [Header("Loop Game")]
    [SerializeField] bool loopGame = false;
    [SerializeField] float gameSpeedFactor = 0.01f;
    [SerializeField] float movementspeedFactor = 0.1f;
    [SerializeField] float shootingspeedFactor = 0.1f;
    [SerializeField] float scoringMultiplier = 0.1f;
    [SerializeField] float multiplier = 2f;
    [SerializeField] GameObject gameLoopSprite;



    //init variables
    int currentGameLoop = 1;
    float setGameSpeed;
    float setMovementSpeed;
    float setShootingSpeed;
    float setScoringMultiplier;
    int enemiesSpawnedInCurrentLevel;
    int enemiesKilledInCurrentLevel;
    int spawnersActiveInLevel;
    bool isEnemiesRemainingInCurrentLvl = true;
    bool isSpawnersRemaininginCurrentLvl = true;
    DateTime startTime;
    
    


    private void Awake()
    {
        CheckSingleton();
    }

    private void CheckSingleton()
    {
        if (FindObjectsOfType<GameSession>().Length > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
        
    }

    private void Start()
    {
        setScoringMultiplier = 1 + ((currentGameLoop - 1) * scoringMultiplier);         // multiplier = 1 on first loop
        CheckHighscore();
        AddToscore(0);
        AddToHealthRemaining(0);
        startTime = DateTime.Now;

        //CheckIfLevelCompleted();


    }

    public void AddToGameLoop()
    {
        currentGameLoop++;
        setGameSpeed = currentGameLoop * gameSpeedFactor;
        setMovementSpeed = movementspeedFactor * (currentGameLoop * multiplier);
        setShootingSpeed = shootingspeedFactor * (currentGameLoop * multiplier);
        setScoringMultiplier = 1+ ((currentGameLoop-1) * scoringMultiplier * multiplier);

        Debug.Log(
            "CurrentGameLoop: " + currentGameLoop +
            "gameSpeedFactor: " + setGameSpeed +
            "movementspeedFactor: " + setMovementSpeed +
            "shootingspeedFactor: " + setShootingSpeed +
            "scoringMultiplier: " + setScoringMultiplier);

        FindObjectOfType<BackgroundScroller>().SetGameSpeed(setGameSpeed);
        SetLevel(1);

        if (currentGameLoop > 1)
        {
            GameObject sprite = Instantiate(gameLoopSprite, new Vector2(5.2f - ((currentGameLoop - 1) * 0.3f), -9.257f), Quaternion.identity);
            sprite.transform.parent = gameObject.transform;
        }

    }

    public void CheckIfLevelCompleted()                        // Did we kill everything in the current Scene?
    {
        
        Debug.Log(" EnemiesLeft ? " + isEnemiesRemainingInCurrentLvl + " Spawners? " + isSpawnersRemaininginCurrentLvl);

        if (spawnersActiveInLevel == 0)
            {
                SetSpawnersRemainingInLvl(false);
            }
        else
            {
                SetSpawnersRemainingInLvl(true);
            }


        if (enemiesSpawnedInCurrentLevel - enemiesKilledInCurrentLevel == 0)
            {

                Debug.Log("SETTING ENEMIES FALSE!");
                SetEnemiesRemainingInLvl(false);
            }
        else
            {
                SetEnemiesRemainingInLvl(true);
            }

        
        //Debug.Log("Spawners active "+ numberofSpawners+"enemies left: " +numberofEnemiesLeft);

        if (!isEnemiesRemainingInCurrentLvl && !isSpawnersRemaininginCurrentLvl)
            {
                FindObjectOfType<Levels>().LoadNextScene();
                ResetEnemiesAndSpawnersInLevel();           
            
            }

    }


    public void SetEnemiesRemainingInLvl(bool enemiesRemaining)
    {
        isEnemiesRemainingInCurrentLvl = enemiesRemaining;
    }

    public void SetSpawnersRemainingInLvl(bool spawnersRemaining)
    {
        isSpawnersRemaininginCurrentLvl = spawnersRemaining;
    }

    public void AddToEnemiesSpawnedInLevel()
    {
        Debug.Log("EnemyRem" + (enemiesSpawnedInCurrentLevel - enemiesKilledInCurrentLevel) + "EnemySpawned " + enemiesSpawnedInCurrentLevel + "Spawners: "+ spawnersActiveInLevel);
        Debug.Log("EnemiesLeft? " + isEnemiesRemainingInCurrentLvl + "Spawners? " + isSpawnersRemaininginCurrentLvl);
        enemiesSpawnedInCurrentLevel++;
    }
    
    public void AddToEnemiesKilledInLevel()
    {
        Debug.Log("EnemyRem" + (enemiesSpawnedInCurrentLevel - enemiesKilledInCurrentLevel) + "EnemySpawned " + enemiesSpawnedInCurrentLevel + "Spawners: " + spawnersActiveInLevel);
        Debug.Log("EnemiesLeft? " + isEnemiesRemainingInCurrentLvl + "Spawners? " + isSpawnersRemaininginCurrentLvl);
        enemiesKilledInCurrentLevel++;
    }


    public void AddToSpawnersInLevel(int spawners)
    {
        spawnersActiveInLevel += spawners;

    }

    public void ResetEnemiesAndSpawnersInLevel()
    {
        enemiesSpawnedInCurrentLevel = 0;
        enemiesKilledInCurrentLevel = 0;
        spawnersActiveInLevel = 0;
        SetSpawnersRemainingInLvl(false);
        SetEnemiesRemainingInLvl(false);

    }


    void Update()
     {
        if (Input.GetKeyDown(KeyCode.Home))            // reset highscore 
        {
            PlayerPrefs.SetInt("highScore", 0);
            PlayerPrefs.SetInt("gameplayed", 0);
            PlayerPrefs.SetInt("gamesCompleted", 0); 


            highScore = 0;
        }

        if (Input.GetKeyDown(KeyCode.PageDown))         // next level    
        {
            FindObjectOfType<Levels>().LoadNextScene();
        }

        if (Input.GetKeyDown(KeyCode.M))                   // toggle music
        {
            if (music)
            {
                music = false;
            }
            else if (!music) 
            {
                music = true;
            }
            if (FindObjectOfType<MusicPlayerLevel>() != null)
            {
                FindObjectOfType<MusicPlayerLevel>().GetComponent<AudioSource>().mute = music;
            }

            if (FindObjectOfType<MusicPlayer>() != null)
            {
                FindObjectOfType<MusicPlayer>().GetComponent<AudioSource>().mute = music;
            }

        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (playerInvulnarable)
            {
                playerInvulnarable = false;
            }
            else
            {
                playerInvulnarable = true;
            }
            FindObjectOfType<Player>().SetPlayerInvulnerable(playerInvulnarable);
        }

        if (Input.GetButtonDown("Start"))
        {
            if (currentLevel == 0)
            {
                FindObjectOfType<Levels>().LoadFirstLevel();
            }
            else if (FindObjectOfType<Levels>().GetCurrentSceneName() == "GameOver")
            {
                FindObjectOfType<Levels>().LoadStartScene();
            } 
            
        }


        //CheckIfLevelCompleted();

    }
        
       


    public bool CheckHighscore()
    {
        highScore = PlayerPrefs.GetInt("highScore", highScore);
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("highScore", highScore);
            return true;

        }
        else
        {
            return false;
        }
    }

    

    



    // Update is called once per frame
    
    public void AddToscore(int score)
    {
        
        currentScore += Mathf.RoundToInt(score * setScoringMultiplier);
        
    }

    public void SetScore(int score)
    {
        currentScore = score;
    }

    public void AddToGold(int value)
    {
        goldValue += value;
    }


    public void ResetGame()                                 //  reset gameStatus by destroying this object
    {
        Destroy(gameObject);
        
    }

    public void SetLevel(int level)
    {
        currentLevel = level;
    }
    
    
    
    public void AddToLevel()
    {
        currentLevel++;
        //levelText.text = currentLevel.ToString();
    }

    public void AddToNumberOfKills()
    {
        numberOfKills++;
        // Debug.Log(numberOfKills);
    }

    public void AddToNumberOfEscaped()
    {
        escapedEnemies++;
        // Debug.Log(numberOfKills);
    }

    public void AddToNumberOfShots(int shots)
    {
        numberOfShotsFired += shots;
        // Debug.Log(numberOfShotsFired);
    }

    public void AddToNumberOfHits()
    {
        numberOfHits ++;
        AddToscore(pointsPerHit);

    }

    public void LockMouseCursor()
    {
        Cursor.visible = false;                               // un-hide mouse cursor 
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void UnLockMouseCursor()
    {
        Cursor.visible = true;                               // hide mouse cursor 
        Cursor.lockState = CursorLockMode.None;
    }
    
    public void AddToHealthRemaining(int health)
    {
        healthRemaining += health;

    }

    public void SetHealthRemaining(int health)
    {
        healthRemaining = health;

    }

    public void SetCurrentGameLoop(int loop)
    {
        currentGameLoop = loop;

    }

    //public SetTotalPlaytime(DateTime aDatetime)
    //{
    //    var timePlayedThisSession = (aDatetime - startTime);


    //    PlayerPrefs.GetString("totalTimePlayed", totalTimePlayed);

    //    if (DateTime.TryParse(totalTimePlayed, out DateTime aResult))
    //    {
    //        var newTotal = timePlayedThisSession.ToString("hh:ss") + aResult.ToString("hh:ss");
    //        PlayerPrefs.SetString(totalTimePlayed, newTotal);
    //    }
    //    else
    //    {
    //        var newTotal = timePlayedThisSession.ToString("c");
    //        PlayerPrefs.SetString(totalTimePlayed, newTotal);
    //    }

    //    return timePlayedThisSession.ToString();
    //}

    public DateTime GetStarTime() { return startTime; }

    public int GetScore() { return currentScore; }

    public int GetShipsRemaining () { return healthRemaining; }

    public int GetHighScore () { return highScore; }

    public int GetHealthRemaining () { return healthRemaining; }
    
    public int GetNumberOfShotsFired () { return numberOfShotsFired; }

    public int GetNumberOfKills() { return numberOfKills; }

    public int GetCurrentLevel() { return currentLevel; }

    public int GetNumberOfHits() { return numberOfHits; }

    public int GetTotalGold() { return goldValue; }

    public int GetEnemiesEscaped() { return escapedEnemies; }

    public bool MusicOn() { return music; }

    public bool IsPlayerInvulnarable() { return playerInvulnarable; }

    /// <summary>
    /// Game loop randomizer 
    /// </summary>

    public bool Loopgame() { return loopGame; }

    public float GetLoopGameSpeedFactor() { return setGameSpeed; }

    public float GetLoopMovementSpeedFactor() { return setMovementSpeed; }

    public float GetLoopShootingSpeedFactor() { return setShootingSpeed; }

    public float GetLoopMultiplier() { return multiplier; }

    public float GetLoopScoringMultiplier () { return setScoringMultiplier; }

    public int GetCurrentGameLoop () { return currentGameLoop; }


}
