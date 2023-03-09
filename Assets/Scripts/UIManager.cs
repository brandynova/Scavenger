using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] WaveManager waveManager;
    [SerializeField] MineralManager mineralManager;

    [SerializeField] public GameObject endWaveScreen;
    [SerializeField] public GameObject statusScreen;

    [SerializeField] GameObject titleScreen;
    [SerializeField] GameObject shipyardScreen;
    [SerializeField] GameObject pauseScreen;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject creditsScreen;
    
    [SerializeField] TextMeshProUGUI shipyardCreditsText;
    [SerializeField] TextMeshProUGUI shipyardMaxShieldsText;
    [SerializeField] TextMeshProUGUI shipyardCurrShieldsText;
    [SerializeField] TextMeshProUGUI repairText;
    [SerializeField] TextMeshProUGUI enhanceText;

    [SerializeField] public Slider shieldSlider;
    [SerializeField] public Slider shipyardSlider;
    [SerializeField] Button repairButton;
    [SerializeField] Button enhanceButton;

    [SerializeField] TextMeshProUGUI creditsText;
    [SerializeField] TextMeshProUGUI shieldsText;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI greenText;
    [SerializeField] TextMeshProUGUI purpleText;
    [SerializeField] TextMeshProUGUI blueText;
    [SerializeField] TextMeshProUGUI redText;
    [SerializeField] TextMeshProUGUI yellowText;
    [SerializeField] TextMeshProUGUI endWaveText;
    [SerializeField] TextMeshProUGUI finalScoreText;

    private CanvasGroup screenCanvas;

    private float currentAlpha;
    private float desiredAlpha;
    private float fadeTime = 1f;

    private int difficulty;

    private bool isScreenTransition;


    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        waveManager = GameObject.Find("Wave Manager").GetComponent<WaveManager>();
        mineralManager = GameObject.Find("Mineral Manager").GetComponent<MineralManager>();


        titleScreen.gameObject.SetActive(true);
        statusScreen.gameObject.SetActive(false);
        endWaveScreen.gameObject.SetActive(false);
        gameOverScreen.gameObject.SetActive(false);
        creditsScreen.gameObject.SetActive(false);

        screenCanvas = titleScreen.GetComponent<CanvasGroup>();

        shipyardScreen.gameObject.SetActive(true);
        repairButton = GameObject.Find("Repair Button").GetComponent<Button>();
        enhanceButton = GameObject.Find("Enhance Button").GetComponent<Button>();
        shipyardScreen.gameObject.SetActive(false);
        

        isScreenTransition = false;
    }

    // Update is called once per frame
    void Update()
    {
         // Check if the title screen is being faded out (start of game only)
         if (isScreenTransition)
         {
            TransitionScreen();
         }
    }

    
    // Called from DifficultyControl, once the player selects a difficulty level
    public void StartScreenTransition(int playerDifficulty)
    {
        difficulty = playerDifficulty;
        isScreenTransition = true;
        currentAlpha = 1f;
        desiredAlpha = 0f;
        Time.timeScale = 1;
    }
    
    void TransitionScreen()
    {
        currentAlpha = Mathf.MoveTowards(currentAlpha, desiredAlpha, fadeTime * Time.deltaTime);
        screenCanvas.alpha = currentAlpha;
        if (currentAlpha == desiredAlpha)
        { 
            isScreenTransition = false;  
            titleScreen.gameObject.SetActive(false);
            screenCanvas.alpha = 1f;
            gameManager.StartGame(difficulty);
        }
    }    
    
    
    // Called from GameManager 
    public void InitializeStatusScreen()
    {
        shieldSlider.maxValue = gameManager.shieldHealth;
        shieldSlider.fillRect.gameObject.SetActive(true);

        shipyardSlider.maxValue = gameManager.shieldHealth;
        shipyardSlider.fillRect.gameObject.SetActive(true);

        mineralManager.InitializeMinerals();
        DisplayCredits(0);
        DisplayShields();
        
        //creditsScreen.gameObject.SetActive(false);
        statusScreen.gameObject.SetActive(true);
    }
    
             

    // Called from GameManager and UIManager
    public void DisplayMinerals()
    {
        greenText.text = mineralManager.mineralName[0] + mineralManager.mineralCount[0];
        purpleText.text = mineralManager.mineralName[1] + mineralManager.mineralCount[1];
        blueText.text = mineralManager.mineralName[2] + mineralManager.mineralCount[2];
        redText.text = mineralManager.mineralName[3] + mineralManager.mineralCount[3];
        yellowText.text = mineralManager.mineralName[4] + mineralManager.mineralCount[4];
    }
    
    // Called from WaveManager
    public void DisplayWaveTimer(float waveTimer)
    {
        float minutes = Mathf.FloorToInt(waveTimer / 60);   
        float seconds = Mathf.FloorToInt(waveTimer % 60);
        
        timerText.text = "Wave " + waveManager.waveNumber + ":  " + minutes.ToString() + ":" + seconds.ToString().PadLeft(2,'0');
    }

    // Called from WaveManager
    public void DisplayWaveScreen()
    {
        endWaveText.text = "Wave " + (waveManager.waveNumber - 1);
        statusScreen.gameObject.SetActive(false);
        endWaveScreen.SetActive(true);
    }

    
    

    public void DisplayCredits(int credits)
    {
        creditsText.text = "Credits: " + credits;
    }

    public void DisplayShields()
    {
        shieldSlider.value = gameManager.shieldHealth;
        shipyardSlider.value = gameManager.shieldHealth;
        shieldsText.text = "Shields: " + gameManager.shieldHealth * 100;
    }
    
    // Called from Shipyard Manager
     public void DisplayShipyard(int purchaseRepairs, int purchaseEnhance)
    {
        shipyardCreditsText.text = "Available Credits: " + gameManager.credits;
        shipyardMaxShieldsText.text = "Maxium Level: " + shieldSlider.maxValue * 100;
        shipyardCurrShieldsText.text = "Current Level: " + gameManager.shieldHealth * 100;
        shipyardSlider.value = gameManager.shieldHealth;

        repairText.text = purchaseRepairs + " credits";
        enhanceText.text = purchaseEnhance + " credits";

        if (purchaseRepairs <= gameManager.credits && gameManager.shieldHealth < shieldSlider.maxValue)
        {
            repairButton.interactable = true;
            repairText.alpha = 1f;

        }
        else
        {
            repairButton.interactable = false;
            repairText.alpha = .4f;
        }
        
        if (purchaseEnhance <= gameManager.credits)
        {
            enhanceButton.interactable = true;
            enhanceText.alpha = 1f;
        }
        else
        {
            enhanceButton.interactable = false;
            enhanceText.alpha = .4f;
        }
        
        endWaveScreen.SetActive(false);
        shipyardScreen.SetActive(true);
    }
    
    // Called from Shipyard Manager
    public void UILeaveShipyard()
    {
        shipyardScreen.SetActive(false);
        statusScreen.gameObject.SetActive(true);
    }

    // Called from GameManager 
    public void DisplayGameOver()
    {
        int mineralScore = 0;
        int finalScore;

        for (int i = 0; i < mineralManager.mineralCount.Count; ++i)
        {
            mineralScore += mineralManager.mineralCount[i] * (i + 1);
        }
        gameOverScreen.gameObject.SetActive(true);  
        finalScore = (mineralScore + waveManager.waveNumber)*gameManager.difficulty + gameManager.credits;
        finalScoreText.text = "Final Score: " + finalScore;
    }
    
    public void TogglePauseScreen(bool toggle)
    {
        pauseScreen.SetActive(toggle);
    }

    public void DisplayCreditsScreen()
    {
        //if (player != null)
        //{
        //    player.SetActive(false);
        //}
        titleScreen.gameObject.SetActive(false);
        gameOverScreen.gameObject.SetActive(false);  
        statusScreen.gameObject.SetActive(false);
        creditsScreen.gameObject.SetActive(true);
    }
}
