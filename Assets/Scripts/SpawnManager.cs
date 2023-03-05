using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Scavenger Lite
public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject[] asteroids;
    [SerializeField] GameObject[] minerals;

    [SerializeField] GameManager gameManager;

    [SerializeField] public float asteroidSpawnTime;

    private float asteroidSpawnRate;
    private float xRange = 30;
    private float yPos = 40;
    private float zPos = 0;
    private int randomIndex;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

    }

    public void SpawnObjects()
    {
        asteroidSpawnRate = asteroidSpawnTime / gameManager.difficulty;
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

    
    Vector3 RandomSpawnPos()
    {
        float randomX = Random.Range(-xRange, xRange);
        
        return new Vector3(randomX, yPos, zPos);
    }


    // Generate one or more minerals when an asteroid is destroyed by a missile
    public void GenerateMinerals(Vector3 position)
    {
        int amountToSpawn = Random.Range(0,gameManager.spawnMax);
        for (int i = 0; i < amountToSpawn; i++)
        {
            int randomIndex = Random.Range(0, minerals.Length);

            float xPos = position.x + (float)i + 1f;
            Vector3 spawnPos = new Vector3(xPos, position.y, position.z);

            Instantiate(minerals[randomIndex], spawnPos, minerals[randomIndex].transform.rotation);
        }
    }
}
