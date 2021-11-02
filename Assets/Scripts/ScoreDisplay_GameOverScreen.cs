using System;
using TMPro;
using UnityEngine;

public class ScoreDisplay_GameOverScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI goldBonus;
    [SerializeField] private TextMeshProUGUI accuracyBonus;
    [SerializeField] private TextMeshProUGUI healthBonus;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI highscoreText;
    [SerializeField] private TextMeshProUGUI numberOfKillsText;
    [SerializeField] private TextMeshProUGUI numberOfEnemiesEscaped;
    [SerializeField] private TextMeshProUGUI numberOfShotsText;
    [SerializeField] private TextMeshProUGUI numberOfHitsText;
    [SerializeField] private TextMeshProUGUI accuracyText;
    [SerializeField] private TextMeshProUGUI totalTimePlayedText;
    [SerializeField] private TextMeshProUGUI gamesPlayedText;

    // init variables

    [SerializeField] private int highScore;
    [SerializeField] private int currentScore;
    [SerializeField] private int currentLevel;
    [SerializeField] private int numberOfKills;
    [SerializeField] private int numberOfShotsFired;
    [SerializeField] private int numberOfHits;

    private int counter;
    private int currentGameLoop;
    private DateTime endTime;
    private int gamesPlayed;
    private int gamesCompleted;
    //private string totalTimePlayed;

    // cache references

    private GameSession gameSession;
    private GameObject player;
    private GameObject music;

    // Start is called before the first frame update
    private void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        currentGameLoop = gameSession.GetCurrentGameLoop();
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

        endTime = DateTime.Now;
        var timespan = endTime - gameSession.GetStarTime();
        UpdatePlayerPrefs();
        var timePassedString = timespan.Minutes.ToString().Trim() + ":" + timespan.Seconds.ToString().Trim();
        totalTimePlayedText.text = timePassedString;
        gamesPlayedText.text = gamesPlayed.ToString();
        

        healthText.text = gameSession.GetHealthRemaining().ToString();
        goldText.text = gameSession.GetTotalGold().ToString();                  // text under Goldpot
        goldBonus.text = (gameSession.GetTotalGold()).ToString();               // text in score summary
        levelText.text = gameSession.GetCurrentLevel().ToString();
        numberOfKillsText.text = gameSession.GetNumberOfKills().ToString();
        numberOfEnemiesEscaped.text = gameSession.GetEnemiesEscaped().ToString();
        numberOfShotsText.text = gameSession.GetNumberOfShotsFired().ToString();
        numberOfHitsText.text = gameSession.GetNumberOfHits().ToString();
        scoreText.text = gameSession.GetScore().ToString();

        float accuracy;
        int a = gameSession.GetNumberOfHits();
        float b = gameSession.GetNumberOfShotsFired();
        if (b != 0)
        {
            accuracy = ((a / b) * 100);
        }
        else
        {
            accuracy = 0;
        }
        accuracyText.text = accuracy.ToString("000") + "%";
        accuracyBonus.text = Mathf.RoundToInt((accuracy * (gameSession.GetCurrentLevel() + (currentGameLoop - 1) * 5))  
                                                * gameSession.GetNumberOfKills() / 10 ).ToString();                    //TODO: make better score bonuses

        healthBonus.text = (gameSession.GetHealthRemaining() * 20 * currentGameLoop).ToString();

        var total = (gameSession.GetScore() +
                         gameSession.GetTotalGold() +
                         gameSession.GetHealthRemaining() * 20 * currentGameLoop +
                         Mathf.RoundToInt(accuracy * 25 + gameSession.GetCurrentLevel() * 250 * currentGameLoop));

        Debug.Log(gameSession.GetScore() + " " +
                         gameSession.GetTotalGold() + " " +
                         gameSession.GetHealthRemaining() * 20 * currentGameLoop + " " +
                         Mathf.RoundToInt(accuracy * 25 + gameSession.GetCurrentLevel() * 250 * currentGameLoop));

        finalScoreText.text = total.ToString();

        gameSession.SetScore(total);

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

    private void UpdatePlayerPrefs()
    {
        gamesPlayed = PlayerPrefs.GetInt("gameplayed", gamesPlayed);
        gamesPlayed++;
        PlayerPrefs.SetInt("gameplayed", gamesPlayed);
        gamesCompleted = PlayerPrefs.GetInt("gamesCompleted", gamesCompleted);
        if (gameSession.GetHealthRemaining() > 0)
        {
            gamesCompleted++;
            PlayerPrefs.GetInt("gamesCompleted", gamesCompleted);
        }
    }

    private void Update()
    {
    }
}