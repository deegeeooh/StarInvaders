using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI scoreGameLoopMultiplierText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI shipsRemainingText;
    [SerializeField] TextMeshProUGUI goldText;

    // init variables

    int currentGameLoop = 1;
    string scoringMultiplier;
    int counter;

    // cache references
       
    GameSession gameSession;


    // Start is called before the first frame update
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        scoringMultiplier = gameSession.GetLoopScoringMultiplier().ToString();
        PrintGameLoopMultiplier();

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

    public void PrintGameLoopMultiplier()
    {
        scoreGameLoopMultiplierText.text = scoringMultiplier + " x";
    }


}
