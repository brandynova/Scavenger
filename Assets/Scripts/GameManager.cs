using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
#if UNITY_EDITOR
    using UnityEditor;
#endif

// Scavenger v2
public class GameManager : MonoBehaviour
{
    [SerializeField] SpawnManager spawnManager;
    [SerializeField] WaveManager waveManager;
    [SerializeField] MineralManager mineralManager;
    [SerializeField] UIManager uiManager;

    [SerializeField] GameObject player;
    
    [SerializeField] public bool isGameActive;
    [SerializeField] public bool isPaused;
    
    private PlayerController playerController;

    private AudioSource sfxAudio;
    private AudioSource engineAudio;
    
    [SerializeField] public GameObject destroyAsteroidsVFX;
    [SerializeField] public AudioClip missedMineralSFX;
    [SerializeField] public List<AudioClip> destroyAsteroidsSFX;
    
    [SerializeField] public int credits;
    [SerializeField] public Vector3 playerStartPosition = new Vector3 (0f, -20f, 0f);

    // Start is called before the first frame update
    void Awake()
    {
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        waveManager = GameObject.Find("Wave Manager").GetComponent<WaveManager>();
        mineralManager = GameObject.Find("Mineral Manager").GetComponent<MineralManager>();
        uiManager = GameObject.Find("UI Manager").GetComponent<UIManager>();
        
        sfxAudio = GetComponent<AudioSource>();
        sfxAudio.volume = MusicManager.Instance.gameMusic.volume;

        engineAudio = GameObject.Find("Audio Engine SFX").GetComponent<AudioSource>();
        engineAudio.volume  = MusicManager.Instance.gameMusic.volume;
        engineAudio.Stop();
                
        player = GameObject.Find("Player");
        playerController= player.GetComponent<PlayerController>();
        player.SetActive(true);

        Time.timeScale = 1;

        //Debug.Log("GameManager is Awake.  TimeScale set to " + Time.timeScale);
        StartGame();
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

    // Called from UIManager.TransitionScreen
    public void StartGame()
    {
        credits = 0;
        MainManager.Instance.finalScore = 0;
        isPaused = false;  
        isGameActive = true;
        player.SetActive(true);
        engineAudio.Play();

        playerController.InitializePlayer();
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
                addToCredits += (MainManager.Instance.gameDifficulty - 1);
            }
            credits += addToCredits;
            uiManager.DisplayCredits(credits);
            CalculateScore();
            uiManager.DisplayScore();
        }
    }

    void TogglePause ()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = 0;

            MusicManager.Instance.PauseGameMusic();
            //gameMusicAudio.Pause();
            engineAudio.Pause();

            uiManager.TogglePauseScreen(true);
        }
        else
        {
            Time.timeScale = 1;

            
            MusicManager.Instance.PlayGameMusic();
            //gameMusicAudio.Play();
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
        
        isGameActive = false;
        yield return new WaitForSeconds(destructionTime); 

        MusicManager.Instance.StartFadeOut(); // fade out game music
        engineAudio.Stop();
        Time.timeScale = 0;

        CalculateScore();
        uiManager.DisplayGameOver();

        MainManager.Instance.SavePlayerData();
    }
    
    public void CalculateScore()
    {
        int mineralScore = 0;
        int difficultyAdjustment = 1;

        switch(MainManager.Instance.gameDifficulty)
        {
            case 1:
                difficultyAdjustment = 1;
                break;
            case 2:
                difficultyAdjustment = 2;
                break;
            case 3:
                difficultyAdjustment = 3;
                break;
        }
            
        for (int i = 0; i < mineralManager.mineralCount.Count; ++i)
        {
            mineralScore += mineralManager.mineralCount[i] * (i + 1);
        }
        MainManager.Instance.finalScore = (mineralScore + waveManager.waveNumber + credits) * difficultyAdjustment;
        MainManager.Instance.finalWave = waveManager.waveNumber;
    }


    // Called from Main Menu button on Game Over screen
    public void LoadMainMenu()
    {
        //SceneManager.LoadScene(0);
        MainManager.Instance.LoadScene(0);
    }
    
    // Called from the Exit button on the Menu 
    public void Exit()
    {
        // Exit the Unity Player if running the editor, otherwise exit the application 
        #if UNITY_EDITOR
            //Debug.Log("Exiting playmode");
            EditorApplication.ExitPlaymode();
        #else
            //Debug.Log("Quitting the application");
            Application.Quit();
        #endif
    }
}
