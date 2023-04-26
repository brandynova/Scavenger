using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Scavenger v2
public class UIManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] WaveManager waveManager;
    [SerializeField] MineralManager mineralManager;
    [SerializeField] PlayerController playerController;

    [SerializeField] public GameObject endWaveScreen;
    [SerializeField] public GameObject statusScreen;
    
    [SerializeField] GameObject shipyardScreen;
    [SerializeField] GameObject pauseScreen;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject congratsFields;
    [SerializeField] GameObject newScoreFields;
    
    [SerializeField] public Slider shieldSlider;
    [SerializeField] public Slider shipyardSlider;
    [SerializeField] Button repairShieldsButton;
    [SerializeField] Button upgradeShieldsButton;
    [SerializeField] Button upgradeThrustButton;

    [SerializeField] TextMeshProUGUI pilotNameText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI creditsText;
    [SerializeField] TextMeshProUGUI shieldsText;
    [SerializeField] TextMeshProUGUI thrustText;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI greenText;
    [SerializeField] TextMeshProUGUI purpleText;
    [SerializeField] TextMeshProUGUI blueText;
    [SerializeField] TextMeshProUGUI redText;
    [SerializeField] TextMeshProUGUI yellowText;

    [SerializeField] TextMeshProUGUI shipyardNameText;
    [SerializeField] TextMeshProUGUI shipyardCreditsText;
    [SerializeField] TextMeshProUGUI shipyardThrustText;
    [SerializeField] TextMeshProUGUI shipyardMaxShieldsText;
    [SerializeField] TextMeshProUGUI shipyardCurrShieldsText;
    [SerializeField] TextMeshProUGUI repairShieldsText;
    [SerializeField] TextMeshProUGUI upgradeShieldsText;
    [SerializeField] TextMeshProUGUI upgradeThrustText;

    [SerializeField] TextMeshProUGUI waveTextValue;
    [SerializeField] TextMeshProUGUI waveShadowValue;
    [SerializeField] TextMeshProUGUI finalScoreText;
    [SerializeField] TextMeshProUGUI finalScoreShadow;
    [SerializeField] TextMeshProUGUI previousHighScoreValue;

    
    [SerializeField] GameObject player;

    private float alphaMax = 1f;
    private float alphaFade = .1f;

    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        waveManager = GameObject.Find("Wave Manager").GetComponent<WaveManager>();
        mineralManager = GameObject.Find("Mineral Manager").GetComponent<MineralManager>();

        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();

        shipyardScreen.gameObject.SetActive(true);
        repairShieldsButton = GameObject.Find("Repair Shields Button").GetComponent<Button>();
        upgradeShieldsButton = GameObject.Find("Upgrade Shields Button").GetComponent<Button>();
        upgradeThrustButton = GameObject.Find("Upgrade Thrust Button").GetComponent<Button>();
        shipyardScreen.gameObject.SetActive(false);
    }

    
    // Called from GameManager 
    public void InitializeStatusScreen()
    {
        shieldSlider.maxValue = playerController.shieldHealth;
        shieldSlider.fillRect.gameObject.SetActive(true);

        shipyardSlider.maxValue = playerController.shieldHealth;
        shipyardSlider.fillRect.gameObject.SetActive(true);

        mineralManager.InitializeMinerals();

        DisplayPilotName();
        DisplayCredits(0);
        DisplayThrust();
        DisplayShields();
        DisplayScore();
        
        statusScreen.gameObject.SetActive(true);
    }
    
    // Called from GameManager and UIManager
    public void DisplayMinerals()
    {
        greenText.text = mineralManager.mineralCount[0].ToString();
        purpleText.text = mineralManager.mineralCount[1].ToString();
        blueText.text = mineralManager.mineralCount[2].ToString();
        redText.text = mineralManager.mineralCount[3].ToString();
        yellowText.text = mineralManager.mineralCount[4].ToString();
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

    private void DisplayPilotName()
    {
        pilotNameText.text = MainManager.Instance.pilotName;
    }

    public void DisplayCredits(int credits)
    {
        creditsText.text = "Credits: " + credits;
    }

    public void DisplayScore()
    {
        scoreText.text = "Score: " + MainManager.Instance.finalScore;
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
        shipyardNameText.text = MainManager.Instance.pilotName;
        shipyardCreditsText.text = "Credits: " + gameManager.credits;
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
        gameOverScreen.gameObject.SetActive(true);  

        string formattedScore = MainManager.Instance.FormatScore(MainManager.Instance.finalScore);

        finalScoreText.text = "\n" + formattedScore;
        finalScoreShadow.text = "\n" + formattedScore;
        
        congratsFields.SetActive(false);
        newScoreFields.SetActive(false);

        switch(MainManager.Instance.gameDifficulty)
        {
            case 1: //EASY
                if(MainManager.Instance.finalScore > MainManager.Instance.highScore1)
                {
                    //Display High Score kudos unless this is the first high score
                    if(!string.IsNullOrEmpty(MainManager.Instance.highScoreName1))
                    {
                        congratsFields.SetActive(true);
                        previousHighScoreValue.text = "\n" + MainManager.Instance.FormatScore(MainManager.Instance.highScore1) + " by " + MainManager.Instance.highScoreName1;
                    }
                    else
                        newScoreFields.SetActive(true);
                }
                break;

            case 2: //NORMAL
                if(MainManager.Instance.finalScore > MainManager.Instance.highScore2)
                {
                    //Display High Score kudos unless this is the first high score
                    if(!string.IsNullOrEmpty(MainManager.Instance.highScoreName2))
                    {
                        congratsFields.SetActive(true);
                        previousHighScoreValue.text = "\n" + MainManager.Instance.FormatScore(MainManager.Instance.highScore2) + " by " + MainManager.Instance.highScoreName2;
                    }
                    else
                        newScoreFields.SetActive(true);
                }
                break;

            case 3: //HARD
                if(MainManager.Instance.finalScore > MainManager.Instance.highScore3)
                {
                    //Display High Score kudos unless this is the first high score
                    if(!string.IsNullOrEmpty(MainManager.Instance.highScoreName3))
                    {
                        congratsFields.SetActive(true);
                        previousHighScoreValue.text = "\n" + MainManager.Instance.FormatScore(MainManager.Instance.highScore3) + " by " + MainManager.Instance.highScoreName3;
                    }
                    else
                        newScoreFields.SetActive(true);
                }
                break;
        }
    }
    

    // Called from Game Manager to toggle the Pause screen on/off
    public void TogglePauseScreen(bool toggle)
    {
        pauseScreen.SetActive(toggle);
    }
}
