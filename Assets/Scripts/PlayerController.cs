using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Scavenger Lite

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] UIManager uiManager;
    [SerializeField] MineralManager mineralManager;

    [SerializeField] public GameObject destructionVFX;
    [SerializeField] public GameObject hitVFX;
    [SerializeField] public GameObject shipGun;

    [SerializeField] public float shieldHealth;
    [SerializeField] public float horizontalThrust;

    [SerializeField] AudioClip launchMissileSFX;
    [SerializeField] AudioClip hitPlayerSFX;
    [SerializeField] AudioClip destroyPlayerSFX;
    
    
    private ParticleSystem shipGunVFX;
    private AudioSource gameAudio;
    private GameObject pooledMissile;
    
    private float horizontalInput;
    private float xBounds = 31f;
    private float yOffset = 1.1f;
    private int maxDifficulty = 3;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        uiManager = GameObject.Find("UI Manager").GetComponent<UIManager>();
        mineralManager = GameObject.Find("Mineral Manager").GetComponent<MineralManager>();
        
        gameAudio = gameManager.GetComponent<AudioSource>();
        shipGunVFX = shipGun.GetComponent<ParticleSystem>();
        shipGunVFX.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        LimitPlayerBoundary();
        FireMissiles();
    }

    // Called from GameManager
    public void InitializePlayer(int difficulty)
    {
        shieldHealth = maxDifficulty - difficulty + 1; // Shields can vary from 1 - 3 based on difficulty
        horizontalThrust = 20.0f;
    }

    // Move player along the x-axis only at this point.   Limit based on ViewPort.
    void MovePlayer()
    {
        // Player movement left to right
        horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * Time.deltaTime * horizontalThrust * horizontalInput);
    }

    // Limit the player movement to within a set area
    void LimitPlayerBoundary()
    {
        if (transform.position.x > xBounds)
        {
            transform.position = new Vector3(xBounds, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < -xBounds)
        {
            transform.position = new Vector3(-xBounds, transform.position.y, transform.position.z);
        }
    }
    
    void FireMissiles()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Get an missile from the pool
            pooledMissile = ObjectPooler.SharedInstance.GetPooledObject();
            if (pooledMissile != null)
            {
                shipGunVFX.Play(); // play gun flash
                gameAudio.PlayOneShot(launchMissileSFX, 0.5f);

                pooledMissile.SetActive(true); // activate pooled missile
                pooledMissile.transform.position = transform.position + new Vector3 (0, yOffset, 0); // position a bit ahead of the player
            }
        }
    }

    // Collect minerals (good) or collide with asteroid (bad)
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Mineral")) 
        {
            mineralManager.CollectMineral(other.gameObject);
        } 
        else if(other.gameObject.CompareTag("Asteroid"))
        {
            Destroy(other.gameObject);
            CollideWithAsteroid();
        }
    }

    // Destroy the player and end game if shield is reduced to 0.  Otherwise keep playing.
    void CollideWithAsteroid()                           
    {   
        --shieldHealth;
        uiManager.DisplayShields();
        if (shieldHealth <= 0)
        {
            gameAudio.PlayOneShot(destroyPlayerSFX, 1.0f);
            Instantiate(destructionVFX, transform.position, Quaternion.identity);  // spawn destruct explosion
            Destroy(gameObject); // Destroy player ship
            gameManager.GameOver(); 
        }
        else
        {
            gameAudio.PlayOneShot(hitPlayerSFX, 1.0f);
            Instantiate(hitVFX, transform.position, Quaternion.identity);  // spawn hit explosion
        }
    }
}
