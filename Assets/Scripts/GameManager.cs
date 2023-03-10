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
    [SerializeField] WaveManager waveManager;
    [SerializeField] UIManager uiManager;
    private PlayerController playerController;
    
    [SerializeField] GameObject player;
    
    [SerializeField] public int difficulty;
    [SerializeField] public bool isGameActive;
    [SerializeField] public bool isPaused;

    [SerializeField] public AudioSource engineAudio;
    //private AudioSource gameMusic;
    private AudioSource gameAudio;
    
    [SerializeField] public GameObject destroyAsteroidsVFX;
    [SerializeField] public AudioClip missedMineralSFX;
    [SerializeField] public List<AudioClip> destroyAsteroidsSFX;
    
    [SerializeField] public int credits;


    // Start is called before the first frame update
    void Start()
    {
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        waveManager = GameObject.Find("Wave Manager").GetComponent<WaveManager>();
        uiManager = GameObject.Find("UI Manager").GetComponent<UIManager>();

        engineAudio = GameObject.Find("Audio Engine SFX").GetComponent<AudioSource>();
        engineAudio.Play();

        //gameMusic = GameObject.Find("Audio Game Music").GetComponent<AudioSource>();
        //gameMusic.Play();
        
        player = GameObject.Find("Player");
        player.SetActive(true);
        playerController= player.GetComponent<PlayerController>();

        isPaused = false; 
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
         //Check if the Pause key is pressed (escape) 
         if (Input.GetKeyDown(KeyCode.Escape) && isGameActive)
         {
            TogglePause();
         }   
    }

    // Called from DifficultyControl
    public void StartGame(int playerDifficulty)
    {
        credits = 0;
        isPaused = false;  
        isGameActive = true;
        player.SetActive(true);
        difficulty = playerDifficulty;

        playerController.InitializePlayer(difficulty);
        uiManager.InitializeStatusScreen();
        waveManager.InitializeWave();
        spawnManager.SpawnObjects();
    }

    // Called from ShipyardManager, MineralManager & ObjectBehaviour
    public void UpdateCredits(int addToCredits)
    {
        if(isGameActive || waveManager.isEndWave)
        {
            if (addToCredits > 0)
            {
                addToCredits += (difficulty - 1);
            }
            credits += addToCredits;
            uiManager.DisplayCredits(credits);
        }
    }

    void TogglePause ()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = 0;
            
            //gameMusic.Pause();
            engineAudio.Pause();

            uiManager.TogglePauseScreen(true);
        }
        else
        {
            Time.timeScale = 1;

            //gameMusic.Play();
            engineAudio.Play();
            
            uiManager.TogglePauseScreen(false);
        }
    } 
   
    // Called from PlayerController when player is destroyed
    public void GameOver()
    {
        StartCoroutine(LaunchGameOver()); //launch the timer of destruction
    }

    // Wait for the indicated time, then destroy the player ship and display Game Over screen with final score
    IEnumerator LaunchGameOver() 
    {
        float destructionTime = 1.5f;

        yield return new WaitForSeconds(destructionTime); 
        
        engineAudio.Stop();
        //gameMusic.Play();
        
        uiManager.DisplayGameOver();
        
        isGameActive = false;
        Time.timeScale = 0;
    }
    
    // Called from Restart buttons on Game Over & Credits screens
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    // Called from Exit buttons on Game Over & Credits screens
    public void ExitGame() 
    {
        Application.Quit();
    }
}
