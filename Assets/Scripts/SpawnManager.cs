using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Scavenger Lite
public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] MineralManager mineralManager;
    
    [SerializeField] GameObject[] asteroids;
    [SerializeField] GameObject[] minerals;
    
    private float asteroidSpawnRate;
    private float asteroidSpawnTime = 4f;
    private float minSpawnRate = 0.25f;
    private float xRange = 30f;
    private float yPos = 40f;
    private float zPos = 0f;

    private int randomIndex;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        mineralManager = GameObject.Find("Mineral Manager").GetComponent<MineralManager>();
    }

    // Called from Game Manager and Wave Manager
    public void SpawnObjects(int waveNumber)
    {
        //Debug.Log("SpawnObjects waveNumber: " + waveNumber);
        asteroidSpawnRate = asteroidSpawnTime / gameManager.difficulty - (waveNumber - 1f) / (gameManager.difficulty * 2);
        if (asteroidSpawnRate < minSpawnRate)
        {
            asteroidSpawnRate = minSpawnRate;
        }
        
        //Debug.Log("asteroidSpawnRate: " + asteroidSpawnRate);
        StartCoroutine(SpawnRandomAsteroid());
    }

    
    IEnumerator SpawnRandomAsteroid()
    {
        while (gameManager.isGameActive && !gameManager.isPaused)
        {
            yield return new WaitForSeconds(asteroidSpawnRate);
            randomIndex = Random.Range(0, asteroids.Length);

            Vector3 spawnPos = RandomSpawnPos();

            Instantiate(asteroids[randomIndex], spawnPos, asteroids[randomIndex].transform.rotation);
        }
    }

    // Called from Wave Manager and Game Manager
    public void StopSpawning()
    {
        StopCoroutine(SpawnRandomAsteroid());
    }

    
    Vector3 RandomSpawnPos()
    {
        float randomX = Random.Range(-xRange, xRange);
        
        return new Vector3(randomX, yPos, zPos);
    }


    // Generate one or more minerals when an asteroid is destroyed by a missile.  Called from PlayerController
    public void GenerateMinerals(Vector3 position)
    {
        int spawnMaxMinerals = (int) Mathf.Round((mineralManager.mineralCount.Count + 1)/gameManager.difficulty);
        int amountToSpawn = Random.Range(0, spawnMaxMinerals);

        for (int i = 0; i < amountToSpawn; i++)
        {
            int randomIndex = Random.Range(0, minerals.Length);

            float xPos = position.x + (float)i + 1.0f;
            Vector3 spawnPos = new Vector3(xPos, position.y, position.z);

            Instantiate(minerals[randomIndex], spawnPos, minerals[randomIndex].transform.rotation);
        }
    }
}
