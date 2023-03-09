using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] UIManager uiManager;
    [SerializeField] ShipyardManager shipyardManager;
    [SerializeField] GameObject player;

    [SerializeField] public int waveNumber;
    [SerializeField] public bool isEndWave;
    
    private float waveInterval = 20f;
    private float waveTimer;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        uiManager = GameObject.Find("UI Manager").GetComponent<UIManager>();
        shipyardManager = GameObject.Find("Shipyard Manager").GetComponent<ShipyardManager>();
        player = GameObject.Find("Player");
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

    
    // Called from GameManager.StartGame
    public void InitializeWave()
    {
        isEndWave = false;
        waveNumber = 1;
        waveTimer = waveInterval; 
        uiManager.DisplayWaveTimer(waveTimer);
    }
    
    
    void ProcessWaveTimer()
    {
        if (waveTimer <= 0) // Initiate next wave
        {
            ++waveNumber;
            waveTimer = waveInterval * waveNumber * gameManager.difficulty; // increase the length of each subsequent wave based on difficulty 
            uiManager.DisplayWaveTimer(waveTimer);   
            EndWave();
        }
        else
        {
            waveTimer -= Time.deltaTime;
            uiManager.DisplayWaveTimer(waveTimer);
        }
    }


    void EndWave()
    {
        if(gameManager.isGameActive && player != null)
        {
            isEndWave = true;
            Time.timeScale = 0;
            player.SetActive(false);
            //gameMusic.Pause();
            gameManager.engineAudio.Pause();

            DestroyRemainingObjects(); 

            uiManager.DisplayWaveScreen();
        }
        else if(player == null)
        {
            gameManager.GameOver();
        }
    }

    // Between waves, remove all objects (Asteroids, minerals, explosion effects, missiles)
    // Called from WaveManager and from the Credits button on the Game Over screen
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
        Time.timeScale = 1;
        isEndWave = false;
        shipyardManager.LeaveShipyard();                    
    }
}
