using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI shipsRemainingText;
    [SerializeField] TextMeshProUGUI goldText;

    // init variables

    //[SerializeField] int currentScore;
    //[SerializeField] int currentLevel;
    //[SerializeField] int shipsRemaining;

    int counter;

    // cache references
       
    GameSession gameSession;


    // Start is called before the first frame update
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();

    }

    // Update is called once per frame
    void Update()
    {
        //counter++;
        //var fps = 1 / Time.deltaTime;

        //Debug.Log(counter +" delta" + fps);
       
        PrintScore();
        PrintLevel();
        PrintLivesRemaining();
        PrintGold();

        
    }

    private void PrintGold()
    {
        goldText.text = gameSession.GetTotalGold().ToString();
    }

    public void PrintScore()
    {
        scoreText.text = gameSession.GetScore().ToString();
    }

    public void PrintLivesRemaining()
    {
        shipsRemainingText.text = gameSession.GetHealthRemaining().ToString();
    }

    public void PrintLevel()
    {
        levelText.text = gameSession.GetCurrentLevel().ToString();
    }


}
