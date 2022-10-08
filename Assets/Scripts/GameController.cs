using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController singleton;
    [SerializeField] GUIController guiController;

    public static GameStates CurrentGameState;

    private void Awake()
    {
        singleton = this;
        ChangeGameState(GameStates.MainMenu);
    }

    public static void ChangeGameState(GameStates state)
    {
        CurrentGameState = state;
    }

    public void SetScore(float score)
    {
        if (PlayerPrefs.HasKey("HighScore"))
        {
            float highestScore = PlayerPrefs.GetFloat("HighScore");
            if (score > highestScore)
            {
                PlayerPrefs.SetFloat("HighScore", score);
                guiController.highscoreText.text = "HighestScore: " + score.ToString("0.00");
            }
            else
            {
                guiController.highscoreText.text = "HighestScore: " + highestScore.ToString("0.00");
            }
        }
        else
        {
            PlayerPrefs.SetFloat("HighScore", score);
            guiController.highscoreText.text = "HighestScore: " + score.ToString("0.00");
        }
        guiController.gameOverPanel.SetActive(true);
        guiController.scoreText.text = score.ToString("0.00");

    }
}



