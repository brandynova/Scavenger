using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

// Scavenger Lite
public class GameManager : MonoBehaviour
{
    [SerializeField] SpawnManager spawnManager;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI shieldsText;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI greenText;
    [SerializeField] TextMeshProUGUI purpleText;
    [SerializeField] TextMeshProUGUI blueText;
    [SerializeField] TextMeshProUGUI redText;
    [SerializeField] TextMeshProUGUI yellowText;
    [SerializeField] TextMeshProUGUI finalScoreText;
    [SerializeField] TextMeshProUGUI shipyardCreditsText;
    [SerializeField] TextMeshProUGUI shipyardMaxShieldsText;
    [SerializeField] TextMeshProUGUI shipyardCurrShieldsText;
    [SerializeField] TextMeshProUGUI repairText;
    [SerializeField] TextMeshProUGUI enhanceText;
    [SerializeField] TextMeshProUGUI endWaveText;

    [SerializeField] Slider shieldSlider;
    [SerializeField] Slider shipyardSlider;
    [SerializeField] Button repairButton;
    [SerializeField] Button enhanceButton;
    
    [SerializeField] GameObject titleScreen;
    [SerializeField] GameObject pauseScreen;
    [SerializeField] GameObject statusScreen;
    [SerializeField] GameObject endWaveScreen;
    [SerializeField] GameObject shipyardScreen;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject creditsScreen;
    [SerializeField] GameObject player;
    
    [SerializeField] public float shieldHealth;
    [SerializeField] public int repairPrice;
    [SerializeField] public int enhancePrice;
    [SerializeField] public int difficulty;
    [SerializeField] public int destructionTime;
    [SerializeField] public int spawnMax;
    [SerializeField] public bool isGameActive;
    [SerializeField] public bool isPaused;
    [SerializeField] public bool isEndWave;
    
    private CanvasGroup screenCanvas;
    
    private GameObject playerVFX;

    private AudioSource gameAudio;
    private AudioSource gameMusic;
    private AudioSource engineAudio;
    
    //[SerializeField] public AudioClip menuSFX;
    [SerializeField] public AudioClip engineSFX;
    [SerializeField] public AudioClip launchMissileSFX;
    [SerializeField] public AudioClip hitPlayerSFX;
    [SerializeField] public AudioClip destroyPlayerSFX;
    [SerializeField] public AudioClip collectMineralSFX; 
    [SerializeField] public List<AudioClip> destroyAsteroidsSFX;
    

    [SerializeField] public float waveBaseInterval = 20f;
    
    private Vector3 startPosition = new Vector3 (0f, -20f, 0f);

    private float fadeTime = 2f;
    private float waveInterval;
    private float waveTimer;
    private float currentAlpha;
    private float desiredAlpha;
    private float minutes;
    private float seconds;

    private int purchaseRepairs;
    private int purchaseEnhance;
    private int maxDifficulty = 3;
    private int numMinerals = 5;
    private int numGreen;
    private int numPurple;
    private int numBlue;
    private int numRed;
    private int numYellow;
    private int score;
    private int waveNumber;

    private bool doFade;


    // Start is called before the first frame update
    void Start()
    {
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();

        gameAudio = GetComponent<AudioSource>();
        engineAudio = GameObject.Find("Audio Engine SFX").GetComponent<AudioSource>();
        //gameMusic = GameObject.Find("Audio Game Music").GetComponent<AudioSource>();

        player = GameObject.Find("Player");
        player.SetActive(true);

        //gameMusic.Play();
        engineAudio.Play();
        
        shipyardScreen.gameObject.SetActive(true);
        repairButton = GameObject.Find("Repair Button").GetComponent<Button>();
        enhanceButton = GameObject.Find("Enhance Button").GetComponent<Button>();
        shipyardScreen.gameObject.SetActive(false);

        titleScreen.gameObject.SetActive(true);
        statusScreen.gameObject.SetActive(false);
        endWaveScreen.gameObject.SetActive(false);
        shipyardScreen.gameObject.SetActive(false);
        gameOverScreen.gameObject.SetActive(false);
        creditsScreen.gameObject.SetActive(false);

        Time.timeScale = 0;
        isPaused = false;
        isEndWave = false;
        doFade = false;
        waveInterval = waveBaseInterval;
        waveTimer = waveInterval;        
    }

    // Update is called once per frame
    void Update()
    {
         //Check if the Pause key is pressed (escape) 
         if (Input.GetKeyDown(KeyCode.Escape) && isGameActive)
         {
            isPaused = !isPaused;
            PauseGame();
         }   
         // Process wave timer
         if (!isEndWave && !isPaused && isGameActive)
         {
            ProcessWaveTimer();
         }

         // Check if the title screen is being faded out (start of game only)
         if (doFade)
         {
            Time.timeScale = 1;
            currentAlpha = Mathf.MoveTowards(currentAlpha, desiredAlpha, fadeTime * Time.deltaTime);
            screenCanvas.alpha = currentAlpha;
            if(currentAlpha == desiredAlpha)
            { 
                doFade = false;  
                StartGame();
            }
         }

    }

    public void StartGame()
    {
        score = 0;
        numGreen = -1;
        numPurple = -1;
        numBlue = -1;
        numRed = -1;
        numYellow = -1;
        
        isPaused = false;  
        isEndWave = false;

        titleScreen.gameObject.SetActive(false);
        screenCanvas.alpha = 1f;

        titleScreen.gameObject.SetActive(false);
        creditsScreen.gameObject.SetActive(false);
        statusScreen.gameObject.SetActive(true);
        
        isGameActive = true;
        player.SetActive(true);

        UpdateScore(0);
        InitiateMinerals();
        spawnMax = (int) Mathf.Round((numMinerals + 1)/difficulty);

        shieldHealth = maxDifficulty - difficulty + 1; // Shields can vary from 1 - 3 based on difficulty
        shieldSlider.maxValue = shieldHealth;
        shieldSlider.fillRect.gameObject.SetActive(true);
        shipyardSlider.maxValue = shieldHealth;
        shipyardSlider.fillRect.gameObject.SetActive(true);
        UpdateShields();

        waveNumber = 1;
        waveTimer = waveInterval; 
        
        purchaseRepairs = repairPrice * difficulty;
        purchaseEnhance = enhancePrice * difficulty;
        
        DisplayWaveTimer();

        spawnManager.SpawnObjects();
    }

    // Difficulty is set in the DifficultyControl script.  Start fading out the title screen
    public void StartFadeTitle(int userDifficulty)
    {
        difficulty = userDifficulty;
        
        doFade = true;
        currentAlpha = 1f;
        desiredAlpha = 0f;
        screenCanvas = titleScreen.GetComponent<CanvasGroup>();
    }

    void InitiateMinerals()
    {
        for (int i = 1; i <= numMinerals; ++i)
        {
            UpdateMineralCount(i);
        }
    }
    
    void ProcessWaveTimer()
    {

        if (waveTimer <= 0) // Initiate next wave
        {
            ++waveNumber;
            waveTimer = waveInterval * waveNumber * difficulty; // increase the length of each subsequent wave based on difficulty 
            DisplayWaveTimer();   
            
            //Debug.Log("Starting wave " + waveNumber + ".  Timer set to: " + waveTimer);
            
            isEndWave = true;
            EndWave();
        }
        else
        {
            waveTimer -= Time.deltaTime;
            DisplayWaveTimer();
        }
    }

    void DisplayWaveTimer()
    {
        minutes= Mathf.FloorToInt(waveTimer / 60);   
        seconds=Mathf.FloorToInt(waveTimer % 60);
        
        timerText.text = "Wave " + waveNumber + ":  " + minutes.ToString() + ":" + seconds.ToString().PadLeft(2,'0');
    }

    void EndWave()
    {
        if(isGameActive)
        {
            //Debug.Log("End Wave");
            Time.timeScale = 0;
        
            //gameMusic.Pause();
            engineAudio.Pause();

            if (player != null)
            {
                player.SetActive(false);
                player.transform.position = startPosition;
            }
        
            DestroyObjects(); // destroy any remaining asteroids and minerals
        
            purchaseRepairs = repairPrice * difficulty * (waveNumber - 1);
            purchaseEnhance = enhancePrice * difficulty * (waveNumber - 1);
        
            endWaveText.text = "Completed \n Wave " + (waveNumber - 1);

            statusScreen.gameObject.SetActive(false);
            endWaveScreen.SetActive(true);
        }
    }

    // Between waves, remove all objects (Asteroids, minerals, explosion effects, missiles
    void DestroyObjects()
    {
        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
        foreach (GameObject nextAsteroid in asteroids) 
        {
            Destroy(nextAsteroid);
        }

        GameObject[] minerals = GameObject.FindGameObjectsWithTag("Mineral");
        foreach (GameObject nextMineral in minerals) 
        {
            Destroy(nextMineral);
        }

        GameObject[] effects = GameObject.FindGameObjectsWithTag("VFX");
        foreach (GameObject nextEffect in effects) 
        {
            Destroy(nextEffect);
        }

        GameObject[] missiles = GameObject.FindGameObjectsWithTag("Missile");
        foreach (GameObject nextMissile in missiles) 
        {
            nextMissile.SetActive(false);
        }
    }


    public void ShipyardRepairs()
    {
        shipyardCreditsText.text = "Available Credits: " + score;
        shipyardMaxShieldsText.text = "Maxium Level: " + shieldSlider.maxValue * 100;
        shipyardCurrShieldsText.text = "Current Level: " + shieldHealth * 100;
        shipyardSlider.value = shieldHealth;

        repairText.text = purchaseRepairs + " Credits";
        enhanceText.text = purchaseEnhance + " Credits";

        if (purchaseRepairs <= score && shieldHealth < shieldSlider.maxValue)
        {
            repairButton.interactable = true;
            repairText.alpha = 1f;

        }
        else
        {
            repairButton.interactable = false;
            repairText.alpha = .4f;
        }
        
        if (purchaseEnhance <= score)
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

    public void RepairShields()
    {
        ++shieldHealth;
        
        UpdateScore(-purchaseRepairs);
        UpdateShields();
        ShipyardRepairs();
    }

    public void EnhanceShields()
    {
        ++shieldHealth;
        ++shieldSlider.maxValue;
        ++shipyardSlider.maxValue;

        UpdateScore(-purchaseEnhance);
        UpdateShields();
        ShipyardRepairs();
    }

    public void ContinueNextWave()
    {
        Time.timeScale = 1;

        //gameMusic.Play();
        engineAudio.Play();
        
        isEndWave = false;
        shipyardScreen.SetActive(false);
        statusScreen.gameObject.SetActive(true);

        player.SetActive(true);
    }


    public void UpdateScore(int addToScore)
    {
        if(isGameActive || isEndWave)
        {
            if (addToScore > 0)
            {
                addToScore += (difficulty - 1);
            }
            score += addToScore;
            scoreText.text = "Credits: " + score;
        }
    }

    
    void UpdateShields()
    {
        shieldSlider.value = shieldHealth;
        shipyardSlider.value = shieldHealth;
        shieldsText.text = "Shields: " + shieldHealth * 100;
    }


    public void UpdateMineralCount(int mineralIndex)
    {
        if(isGameActive)
        {
            switch (mineralIndex)
            {
                case 1:
                    ++numGreen;
                    greenText.text = "Green: " + numGreen;
                    break;
                case 2:
                    ++numPurple;
                    purpleText.text = "Purple: " + numPurple;
                    break;
                case 3:
                    ++numBlue;
                    blueText.text = "Blue: " + numBlue;
                    break;
                case 4:
                    ++numRed;
                    redText.text = "Red: " + numRed;
                    break;
                case 5:
                    ++numYellow;
                    yellowText.text = "Yellow: " + numYellow;
                    break;
            }
        }
    }


    void PauseGame ()
    {
        if (isPaused)
        {
            Time.timeScale = 0;
            
            //gameMusic.Pause();
            engineAudio.Pause();

            pauseScreen.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;

            //gameMusic.Play();
            engineAudio.Play();

            pauseScreen.SetActive(false);
        }
    } 
   
    
    public void GameOver()
    {
        --shieldHealth;
        UpdateShields();

       if (shieldHealth == 0)
       {
            StartCoroutine(Destruction()); //launching the timer of destruction
       }
        
    }

    IEnumerator Destruction() //wait for the estimated time, and destroying or deactivating the object
    {
        int finalScore;

        yield return new WaitForSeconds(destructionTime); 
        
        engineAudio.Stop();
        //gameMusic.Play();
        gameOverScreen.gameObject.SetActive(true);  

        //Debug.Log("FINAL:  Minerals (" + numGreen + ", " + numPurple + ", " + numBlue + ", " + numRed + ", " + numYellow + ") \n Difficulty: " + difficulty + ".  Credits: " + score);
        finalScore = (numGreen + numPurple*2 + numBlue*3 + numRed*4 + numYellow*5 + waveNumber)*difficulty + score;
        finalScoreText.text = "Final Score: " + finalScore;
        
        isGameActive = false;
        Time.timeScale = 0;
    }

    
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
   

    public void ShowCredits()
    {
        if (player != null)
        {
            player.SetActive(false);
        }
        creditsScreen.gameObject.SetActive(true);
        titleScreen.gameObject.SetActive(false);
        gameOverScreen.gameObject.SetActive(false);  
    }

    public void ExitGame() 
    {
        Application.Quit();
    }
    
}
