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
    [SerializeField] int shipsRemaining = 3;
    [SerializeField] int numberOfShotsFired = 0;
    [SerializeField] int numberOfHits = 0;
    [SerializeField] int pointsPerHit = 5;
    //[SerializeField] int numberOfMissed = 0;
    

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
        UpdateLivesRemaining(0);
        
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
        
    }
    public void AddToscore(int score)
    {
        currentScore += score;
        
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

    //public void AddToNumberOfMissed()
    //{
    //    numberOfMissed++;
    //    // Debug.Log(numberOfKills);
    //}

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
    
    public void UpdateLivesRemaining(int lives)
    {
        shipsRemaining += lives;

    }

    public int GetScore() { return currentScore; }

    public int GetShipsRemaining () { return shipsRemaining; }

    public int GetHighScore () { return highScore; }

    public int GetLivesRemaining () { return shipsRemaining; }
    
    public int GetNumberOfShotsFired () { return numberOfShotsFired; }

    public int GetNumberOfKills() { return numberOfKills; }

    public int GetCurrentLevel() { return currentLevel; }

    public int GetNumberOfHits() { return numberOfHits; }



}
