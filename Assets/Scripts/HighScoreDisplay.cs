using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI highscoreText;

    // init variables

    //[SerializeField] int highScore;

    // cache references
        
    GameSession gameSession;

    // Start is called before the first frame update
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        gameSession.CheckHighscore();
        highscoreText.text = gameSession.GetHighScore().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
