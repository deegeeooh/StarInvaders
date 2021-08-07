using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplay_GameOverScreen : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI finalScoreText;
    [SerializeField] TextMeshProUGUI goldBonus;
    [SerializeField] TextMeshProUGUI accuracyBonus;
    [SerializeField] TextMeshProUGUI healthBonus;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI highscoreText;
    [SerializeField] TextMeshProUGUI numberOfKillsText;
    [SerializeField] TextMeshProUGUI numberOfEnemiesEscaped;
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
        FindObjectOfType<GameSession>().UnLockMouseCursor();

        gameSession = FindObjectOfType<GameSession>();


        healthText.text = gameSession.GetHealthRemaining().ToString();
        goldBonus.text = (gameSession.GetTotalGold()).ToString();
        levelText.text = gameSession.GetCurrentLevel().ToString();
        numberOfKillsText.text = gameSession.GetNumberOfKills().ToString();
        numberOfEnemiesEscaped.text = gameSession.GetEnemiesEscaped().ToString();
        numberOfShotsText.text = gameSession.GetNumberOfShotsFired().ToString();
        numberOfHitsText.text = gameSession.GetNumberOfHits().ToString();
        scoreText.text = gameSession.GetScore().ToString();

        
        int a = gameSession.GetNumberOfHits();
        float b = gameSession.GetNumberOfShotsFired();
        float accuracy = ((a / b) * 100);
        accuracyText.text = accuracy.ToString("000")+"%";
        
        
        accuracyBonus.text = Mathf.RoundToInt(accuracy * 25+gameSession.GetCurrentLevel()*250).ToString();                    //TODO: make better score bonuses
        healthBonus.text = (gameSession.GetHealthRemaining() * 20).ToString();
        var total = (gameSession.GetScore() +
                         gameSession.GetTotalGold() +
                         gameSession.GetHealthRemaining() * 20 +
                         Mathf.RoundToInt(accuracy * 25));

        Debug.Log(gameSession.GetScore() +" "+
                         gameSession.GetTotalGold() +" "+
                         gameSession.GetHealthRemaining() * 20 +" "+
                         Mathf.RoundToInt(accuracy * 25));
        
        finalScoreText.text = total.ToString();

        gameSession.AddToscore(total - gameSession.GetScore());

        if (gameSession.CheckHighscore())
        {
            Debug.Log("You have a highscore!!!!!");
            highscoreText.text = gameSession.GetHighScore().ToString();
        }
        else
        {
            highscoreText.text = gameSession.GetHighScore().ToString();
        }

    }



}
