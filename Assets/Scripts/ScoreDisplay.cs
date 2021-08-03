using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI shipsRemainingText;

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

        
    }

    public void PrintScore()
    {
        scoreText.text = gameSession.GetScore().ToString();
    }

    public void PrintLivesRemaining()
    {
        shipsRemainingText.text = gameSession.GetLivesRemaining().ToString();
    }

    public void PrintLevel()
    {
        levelText.text = gameSession.GetCurrentLevel().ToString();
    }


}
