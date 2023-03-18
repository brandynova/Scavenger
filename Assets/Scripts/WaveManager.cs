using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] SpawnManager spawnManager;
    [SerializeField] UIManager uiManager;
    [SerializeField] MusicManager musicManager;
    [SerializeField] GameObject player;

    [SerializeField] public int waveNumber;
    [SerializeField] public bool isEndWave;
    
    private AudioSource gameMusic;
    private AudioSource shipyardAudio;
    private AudioSource engineAudio;

    private float waveInterval = 20f;
    private float waveTimer;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        uiManager = GameObject.Find("UI Manager").GetComponent<UIManager>();
        musicManager = GameObject.Find("Game Music").GetComponent<MusicManager>();
        player = GameObject.Find("Player");
        
        gameMusic = GameObject.Find("Game Music").GetComponent<AudioSource>();
        engineAudio = GameObject.Find("Audio Engine SFX").GetComponent<AudioSource>();
        shipyardAudio = GameObject.Find("Audio Shipyard SFX").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
         // Process wave timer while the wave is active
         if (!isEndWave && gameManager.isGameActive && !gameManager.isPaused)
         {
            ProcessWaveTimer();
         }
    }
    
    // Called from GameManager.StartGame to initialize the first wave
    public void InitializeWave()
    {
        isEndWave = false;
        waveNumber = 1;
        waveTimer = waveInterval; 
        uiManager.DisplayWaveTimer(waveTimer);
    }
    
    void ProcessWaveTimer()
    {
        if (waveTimer <= 0) // End of this wave
        {
            ++waveNumber;
            waveTimer = waveInterval * waveNumber * gameManager.difficulty; // increase the length of each subsequent wave based on difficulty 
            uiManager.DisplayWaveTimer(waveTimer);   
            EndWave();
        }
        else // Display time left in this wave
        {
            waveTimer -= Time.deltaTime;
            uiManager.DisplayWaveTimer(waveTimer);
        }
    }

    void EndWave()
    {
        float fadeTime = 3f;
        
        spawnManager.StopSpawning();
        if(gameManager.isGameActive && player != null)
        {
            isEndWave = true;
            player.SetActive(false);
            gameManager.isPaused = true;

            musicManager.StartFadeOut(fadeTime); // fade out game music at end of wave
            
            Time.timeScale = 0;
            DestroyRemainingObjects(); 
            uiManager.DisplayWaveScreen();
        }
        else if(player == null) // In case player was destroyed as the wave ended
        {
            gameManager.GameOver();
        }
    }

    // Between waves, remove all objects (Asteroids, minerals, explosion effects, missiles)
    // Called from WaveManager when wave ends, and Game Manager when returning to Main Menu (title screen) on game over
    public void DestroyRemainingObjects()
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

    // From Next Wave button on Shipyard Screen
    public void ContinueNextWave()
    {
        float fadeTime = 10f;
                
        Time.timeScale = 1;
        shipyardAudio.Stop();
        engineAudio.Play();
        musicManager.StartFadeIn(fadeTime); // fade in game music at beginning of new wave
        
        player.transform.position = gameManager.playerStartPosition;
        player.SetActive(true);

        uiManager.UILeaveShipyard();
        gameManager.isPaused = false;
        isEndWave = false;
        spawnManager.SpawnObjects(waveNumber);
    }
}
