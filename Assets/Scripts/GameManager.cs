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
    [SerializeField] MusicManager musicManager;

    [SerializeField] GameObject player;
    
    [SerializeField] public int difficulty;
    [SerializeField] public bool isGameActive;
    [SerializeField] public bool isPaused;
    
    private PlayerController playerController;

    private AudioSource engineAudio;
    private AudioSource gameMusicAudio;
    
    [SerializeField] public GameObject destroyAsteroidsVFX;
    [SerializeField] public AudioClip missedMineralSFX;
    [SerializeField] public List<AudioClip> destroyAsteroidsSFX;
    
    [SerializeField] public int credits;
    [SerializeField] public Vector3 playerStartPosition = new Vector3 (0f, -20f, 0f);


    // Start is called before the first frame update
    void Start()
    {
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        waveManager = GameObject.Find("Wave Manager").GetComponent<WaveManager>();
        uiManager = GameObject.Find("UI Manager").GetComponent<UIManager>();
        musicManager = GameObject.Find("Game Music").GetComponent<MusicManager>();

        engineAudio = GameObject.Find("Audio Engine SFX").GetComponent<AudioSource>();
        engineAudio.Stop();
        
        gameMusicAudio = GameObject.Find("Game Music").GetComponent<AudioSource>();
        gameMusicAudio.Play();
        
        player = GameObject.Find("Player");
        playerController= player.GetComponent<PlayerController>();
        player.SetActive(true);

        isPaused = true; 
        //Time.timeScale = 0;
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
        engineAudio.Play();

        playerController.InitializePlayer(difficulty);
        uiManager.InitializeStatusScreen();
        waveManager.InitializeWave();
        spawnManager.SpawnObjects(1);
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

            //Cursor.lockState = CursorLockMode.None;
            //Cursor.visible = true;
            
            gameMusicAudio.Pause();
            engineAudio.Pause();

            uiManager.TogglePauseScreen(true);
        }
        else
        {
            Time.timeScale = 1;

            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;

            gameMusicAudio.Play();
            engineAudio.Play();
            
            uiManager.TogglePauseScreen(false);
        }
    } 
   
    // Called from PlayerController when player is destroyed
    public void GameOver()
    {
        spawnManager.StopSpawning();
        StartCoroutine(LaunchGameOver()); //launch the timer of destruction
    }

    // Wait for the indicated time, then destroy the player ship and display Game Over screen with final score
    IEnumerator LaunchGameOver() 
    {
        float destructionTime = 1.75f;
        float fadeTime = 5f;
        
        isGameActive = false;
        yield return new WaitForSeconds(destructionTime); 

        musicManager.StartFadeOut(fadeTime); // fade out game music
        engineAudio.Stop();
        Time.timeScale = 0;
        
        uiManager.DisplayGameOver();
    }
    
    // Called from Restart buttons on Game Over screen
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    // Called from Exit buttons on Game Over & Title screens
    public void ExitGame() 
    {
        Application.Quit();
    }
}
