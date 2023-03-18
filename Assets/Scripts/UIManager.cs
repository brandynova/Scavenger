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
    [SerializeField] PlayerController playerController;

    [SerializeField] public GameObject endWaveScreen;
    [SerializeField] public GameObject statusScreen;
    
    [SerializeField] GameObject titleScreen;
    [SerializeField] GameObject shipyardScreen;
    [SerializeField] GameObject pauseScreen;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject creditsScreen;
    [SerializeField] GameObject instructionsScreen;
    
    [SerializeField] public Slider shieldSlider;
    [SerializeField] public Slider shipyardSlider;
    [SerializeField] Button repairShieldsButton;
    [SerializeField] Button upgradeShieldsButton;
    [SerializeField] Button upgradeThrustButton;

    [SerializeField] TextMeshProUGUI creditsText;
    [SerializeField] TextMeshProUGUI shieldsText;
    [SerializeField] TextMeshProUGUI thrustText;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI greenText;
    [SerializeField] TextMeshProUGUI purpleText;
    [SerializeField] TextMeshProUGUI blueText;
    [SerializeField] TextMeshProUGUI redText;
    [SerializeField] TextMeshProUGUI yellowText;

    [SerializeField] TextMeshProUGUI shipyardCreditsText;
    [SerializeField] TextMeshProUGUI shipyardThrustText;
    [SerializeField] TextMeshProUGUI shipyardMaxShieldsText;
    [SerializeField] TextMeshProUGUI shipyardCurrShieldsText;
    [SerializeField] TextMeshProUGUI repairShieldsText;
    [SerializeField] TextMeshProUGUI upgradeShieldsText;
    [SerializeField] TextMeshProUGUI upgradeThrustText;

    [SerializeField] TextMeshProUGUI waveTextValue;
    [SerializeField] TextMeshProUGUI waveShadowValue;
    [SerializeField] TextMeshProUGUI ScoreValueText;
    [SerializeField] TextMeshProUGUI ScoreValueShadow;
    
    [SerializeField] GameObject player;

    private CanvasGroup screenCanvas;

    private float currentAlpha;
    private float desiredAlpha;
    private float fadeTime = 1f;
    private float alphaMax = 1f;
    private float alphaFade = .1f;
    private float alphaMin = 0f;

    private int difficulty;

    private bool isScreenTransition;


    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        waveManager = GameObject.Find("Wave Manager").GetComponent<WaveManager>();
        mineralManager = GameObject.Find("Mineral Manager").GetComponent<MineralManager>();

        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();

        screenCanvas = titleScreen.GetComponent<CanvasGroup>();

        shipyardScreen.gameObject.SetActive(true);
        repairShieldsButton = GameObject.Find("Repair Shields Button").GetComponent<Button>();
        upgradeShieldsButton = GameObject.Find("Upgrade Shields Button").GetComponent<Button>();
        upgradeThrustButton = GameObject.Find("Upgrade Thrust Button").GetComponent<Button>();
        shipyardScreen.gameObject.SetActive(false);

        isScreenTransition = false;

        DisplayTitleScreen(); // Display the Main Menu to get the game started
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

    
    // Called from DifficultyControl, once the player selects a difficulty level from the Main Menu
    public void StartScreenTransition(int playerDifficulty)
    {
        difficulty = playerDifficulty;
        isScreenTransition = true;
        currentAlpha = alphaMax;
        desiredAlpha = alphaMin;
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
            screenCanvas.alpha = alphaMax;
            gameManager.StartGame(difficulty);
        }
    }    
    
    
    // Called from GameManager 
    public void InitializeStatusScreen()
    {
        shieldSlider.maxValue = playerController.shieldHealth;
        shieldSlider.fillRect.gameObject.SetActive(true);

        shipyardSlider.maxValue = playerController.shieldHealth;
        shipyardSlider.fillRect.gameObject.SetActive(true);

        mineralManager.InitializeMinerals();
        DisplayCredits(0);
        DisplayThrust();
        DisplayShields();
        
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
        waveTextValue.text = "\n" + (waveManager.waveNumber - 1);
        waveShadowValue.text = "\n" + (waveManager.waveNumber - 1);
        statusScreen.gameObject.SetActive(false);
        endWaveScreen.SetActive(true);
    }

    public void DisplayCredits(int credits)
    {
        creditsText.text = "Credits: " + credits;
    }

    public void DisplayShields()
    {
        shieldsText.text = "Shields: " + playerController.shieldHealth * 100;
        shieldSlider.value = playerController.shieldHealth;
        shipyardSlider.value = playerController.shieldHealth;
    }

    public void DisplayThrust()
    {
        thrustText.text = "Thrust: " + playerController.horizontalThrust * 10;
        shipyardThrustText.text = "Thrust: " + playerController.horizontalThrust * 10;
    }
    
    // Called from Shipyard Manager
     public void DisplayShipyard(int purchaseRepairShields, int purchaseUpgradeShields, int purchaseUpgradeThrust)
    {
        shipyardCreditsText.text = "Available Credits: " + gameManager.credits;
        shipyardMaxShieldsText.text = "Maxium Level: " + shieldSlider.maxValue * 100;
        shipyardCurrShieldsText.text = "Current Level: " + playerController.shieldHealth * 100;
        shipyardThrustText.text = "Thrust: " + playerController.horizontalThrust * 10;
        shipyardSlider.value = playerController.shieldHealth;

        repairShieldsText.text = purchaseRepairShields + " credits";
        upgradeShieldsText.text = purchaseUpgradeShields + " credits";
        upgradeThrustText.text = purchaseUpgradeThrust + " credits";

        // Display Repair Shields if enough credits
        if (purchaseRepairShields <= gameManager.credits && playerController.shieldHealth < shieldSlider.maxValue)
        {
            repairShieldsButton.interactable = true;
            repairShieldsText.alpha = alphaMax;

        }
        else
        {
            repairShieldsButton.interactable = false;
            repairShieldsText.alpha = alphaFade;
        }
        
        // Display Upgrade Shields if enough credits 
        if (purchaseUpgradeShields <= gameManager.credits)
        {
            upgradeShieldsButton.interactable = true;
            upgradeShieldsText.alpha = alphaMax;
        }
        else
        {
            upgradeShieldsButton.interactable = false;
            upgradeShieldsText.alpha = alphaFade;
        }
        
        // Display Upgrade Thrusters if enough credits
        if (purchaseUpgradeThrust <= gameManager.credits)
        {
            upgradeThrustButton.interactable = true;
            upgradeThrustText.alpha = alphaMax;
        }
        else
        {
            upgradeThrustButton.interactable = false;
            upgradeThrustText.alpha = alphaFade;
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

    // Called from GameManager when player is killed
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
        ScoreValueText.text = "\n" + finalScore;
        ScoreValueShadow.text = "\n" + finalScore;
    }
    

    // Called from Game Manager to toggle the Pause screen on/off
    public void TogglePauseScreen(bool toggle)
    {
        pauseScreen.SetActive(toggle);
    }


    // Display the Main Menu (title screen)
    // Called from Start in UIManager and from the Back buttons on Credits and Instructions screens (off Main Menu)
    public void DisplayTitleScreen()
    {
        if (player != null)
        {
            player.SetActive(true);
        }
        statusScreen.gameObject.SetActive(false);
        endWaveScreen.gameObject.SetActive(false);
        gameOverScreen.gameObject.SetActive(false);
        creditsScreen.gameObject.SetActive(false);
        instructionsScreen.gameObject.SetActive(false);
        titleScreen.gameObject.SetActive(true);
    }

    // Called from Credits buttons on Main Menu (title screen)
    public void DisplayCreditsScreen()
    {
        if (player != null)
        {
            player.SetActive(false);
        }
        titleScreen.gameObject.SetActive(false);
        creditsScreen.gameObject.SetActive(true);
    }

    // Called from Instructions buttons on Main Menu (title screen)
    public void DisplayInstructionsScreen()
    {
        if (player != null)
        {
            player.SetActive(false);
        }
        titleScreen.gameObject.SetActive(false);
        instructionsScreen.gameObject.SetActive(true);
    }
}
