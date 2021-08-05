using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    

    // state variables

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
        CheckHighscore();
        AddToscore(0);
        AddToHealthRemaining(0);
        
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
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))            // reset highscore
        {
            PlayerPrefs.SetInt("highScore", 0);
            highScore = 0;
        }
    }
    public void AddToscore(int score)
    {
        currentScore += score;
        
    }

    public void AddToGold(int value)
    {
        goldValue += value;

    }


    public void ResetGame()                                 //  reset gameStatus by destroying this object
    {
        Destroy(gameObject);
        
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

    public void AddToNumberOfShots()
    {
        numberOfShotsFired ++;
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


}
