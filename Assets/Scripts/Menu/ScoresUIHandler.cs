using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[DefaultExecutionOrder(1200)]
public class ScoresUIHandler : MonoBehaviour
{
    [SerializeField] GameObject highScoresScreen;
    [SerializeField] GameObject clearHighScoresDialog;
    [SerializeField] GameObject highScoreTable;
    [SerializeField] GameObject noHighScoreText;
    
    [SerializeField] Button clearButton;
    [SerializeField] TextMeshProUGUI clearButtonText;

    [SerializeField] TextMeshProUGUI nameText1;
    [SerializeField] TextMeshProUGUI scoreText1;
    [SerializeField] TextMeshProUGUI waveText1;
    [SerializeField] TextMeshProUGUI difficultyText1;
    
    [SerializeField] TextMeshProUGUI nameText2;
    [SerializeField] TextMeshProUGUI waveText2;
    [SerializeField] TextMeshProUGUI scoreText2;
    [SerializeField] TextMeshProUGUI difficultyText2;
    
    [SerializeField] TextMeshProUGUI nameText3;
    [SerializeField] TextMeshProUGUI waveText3;
    [SerializeField] TextMeshProUGUI scoreText3;
    [SerializeField] TextMeshProUGUI difficultyText3;
    
    private float alphaMax = 1f;
    private float alphaFade = .3f;
    private bool isHighScores;

    // Start is called before the first frame update
    void Awake()
    {
        DisplayHighScores();
    }

    public void DisplayHighScores()
    {
        clearHighScoresDialog.SetActive(false);
        highScoresScreen.gameObject.SetActive(true);
        BuildHighScoreTable();
    }
    
    // Called from DisplayHighScores and from Main Manager after clearing the high score fields
    public void BuildHighScoreTable()
    {
        isHighScores = false;
        ManageClearButton(); // Make the clear button inactive - if there are any high scores, then activate

        //If the player name is null, then there is no high score recorded yet
        if(string.IsNullOrEmpty(MainManager.Instance.highScoreName1) && 
        string.IsNullOrEmpty(MainManager.Instance.highScoreName2) && 
        string.IsNullOrEmpty(MainManager.Instance.highScoreName3))
        {
            noHighScoreText.SetActive(true);
            highScoreTable.SetActive(false);
            return;
        }
        
        highScoreTable.SetActive(true);
        noHighScoreText.SetActive(false);

        Debug.Log("Build table:");
        Debug.Log("  Score1: " + MainManager.Instance.highScoreName1);
        Debug.Log("  Score2: " + MainManager.Instance.highScoreName2);
        Debug.Log("  Score3: " + MainManager.Instance.highScoreName3);

        // *** EASY ***
        if(string.IsNullOrEmpty(MainManager.Instance.highScoreName1))
        {
            nameText1.text = "No high score";
            difficultyText1.text = "Easy";
            waveText1.text = "";
            scoreText1.text = "";
        }
        else
        {
            isHighScores = true;
            string formattedScore = MainManager.Instance.FormatScore(MainManager.Instance.highScore1);
            //Debug.Log("temp1 string with parsing format: " + formattedScore);

            nameText1.text = MainManager.Instance.highScoreName1;
            difficultyText1.text = "Easy";
            waveText1.text = MainManager.Instance.highScoreWave1.ToString();
            scoreText1.text = formattedScore;
        }

        // *** NORMAL ***
        if(string.IsNullOrEmpty(MainManager.Instance.highScoreName2))
        {
            nameText2.text = "No high score";
            difficultyText2.text = "Normal";
            waveText2.text = "";
            scoreText2.text = "";
        }
        else
        {
            isHighScores = true;
            string formattedScore = MainManager.Instance.FormatScore(MainManager.Instance.highScore2);

            nameText2.text = MainManager.Instance.highScoreName2;
            difficultyText2.text = "Normal";
            waveText2.text = MainManager.Instance.highScoreWave2.ToString();
            scoreText2.text = formattedScore;
        }

        // *** HARD ***
        if(string.IsNullOrEmpty(MainManager.Instance.highScoreName3))
        {
            nameText3.text = "No high score";
            difficultyText3.text = "Hard";
            waveText3.text = "";
            scoreText3.text = "";
        }
        else
        {
            isHighScores = true;
            string formattedScore = MainManager.Instance.FormatScore(MainManager.Instance.highScore3);

            nameText3.text = MainManager.Instance.highScoreName3;
            difficultyText3.text = "Hard";
            waveText3.text = MainManager.Instance.highScoreWave3.ToString();
            scoreText3.text = formattedScore;
        }

        ManageClearButton(); // Make the clear button inactive if there are not any high scores yet
    }

    //Check if there are any high scores. If not, the Clear button should be inactive
    private void ManageClearButton()
    {
        if(isHighScores)
        {
            clearButton.interactable = true;
            clearButtonText.alpha = alphaMax;
        }
        else  
        {
            clearButton.interactable = false;
            clearButtonText.alpha = alphaFade;
        }
    }
    // Check if the player really wants to clear all high scores
    public void DisplayConfirmClearHighScores()
    {
        clearHighScoresDialog.SetActive(true);
    }

    public void CloseConfirmClearHighScores()
    {
        clearHighScoresDialog.SetActive(false);
    }

    public void ClearHighScores()
    {
        MainManager.Instance.ClearHighScores();
        clearHighScoresDialog.SetActive(false);
        BuildHighScoreTable();
    }
}
