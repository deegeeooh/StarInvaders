using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplay_GameOverScreen : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI highscoreText;
    [SerializeField] TextMeshProUGUI numberOfKillsText;
    [SerializeField] TextMeshProUGUI numberOfShotsText;
    [SerializeField] TextMeshProUGUI numberOfHitsText;
    [SerializeField] TextMeshProUGUI accuracyText;

    // init variables

    [SerializeField] int highScore;
    [SerializeField] int currentScore;
    [SerializeField] int currentLevel;
    [SerializeField] int numberOfKills;
    [SerializeField] int numberOfShotsFired;
    [SerializeField] int numberOfHits;


    int counter;

    // cache references
       
    GameSession gameSession;
    GameObject player;
    GameObject music;


    // Start is called before the first frame update
    void Start()
    {
        
        if (FindObjectsOfType<Player>().Length == 1)             // if there's a player left, destroy it
        {

            Debug.Log("PLayer found");
            player = GameObject.FindWithTag("Player");
            Destroy(player);
        }

        if (FindObjectsOfType<MusicPlayerLevel>().Length == 1)
        {
            music = GameObject.Find("MusicPlayerLevel");
            Destroy(music);

        }


        gameSession = FindObjectOfType<GameSession>();
        if (gameSession.CheckHighscore())
        {
            Debug.Log("You have a highscore!!!!!");
            highscoreText.text = gameSession.GetHighScore().ToString();
        } 
        else
        {
            highscoreText.text = gameSession.GetHighScore().ToString();
        }

        scoreText.text = gameSession.GetScore().ToString();
        levelText.text = gameSession.GetCurrentLevel().ToString();
        numberOfKillsText.text = gameSession.GetNumberOfKills().ToString();
        numberOfShotsText.text = gameSession.GetNumberOfShotsFired().ToString();
        numberOfHitsText.text = gameSession.GetNumberOfHits().ToString();
        int a = gameSession.GetNumberOfHits();
        float b = gameSession.GetNumberOfShotsFired();
        float accuracy = (a / b)*100;
        Debug.Log(accuracy);
        accuracyText.text = accuracy.ToString("000")+"%";


    }

    // Update is called once per frame
    void Update()
    {
               
        PrintScore();
        PrintLevel();

        
    }

    public void PrintScore()
    {
        scoreText.text = gameSession.GetScore().ToString();
    }

   
    public void PrintLevel()
    {
        
    }


}
