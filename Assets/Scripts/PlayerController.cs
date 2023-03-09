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
    [SerializeField] public float destructionTime;

    private ParticleSystem shipGunVFX;
    private AudioSource gameAudio;

    [SerializeField] AudioClip launchMissileSFX;
    [SerializeField] AudioClip hitPlayerSFX;
    [SerializeField] AudioClip destroyPlayerSFX;

    private GameObject pooledMissile;
    
    private float horizontalInput;
    private float speed = 20.0f;
    private float xBounds = 30f;
    private float yOffset = 1.1f;


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

    // Move player along the x-axis only at this point.   Limit based on ViewPort.
    void MovePlayer()
    {
        // Player movement left to right
        horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * Time.deltaTime * speed * horizontalInput);
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
                pooledMissile.transform.position = transform.position + new Vector3 (0, yOffset, 0); // position it at player
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
            CollideWithPlayer();
        }
    }


    // Destroy the player if shield is down to 1.  Otherwise reduce shields and keep playing.
    void CollideWithPlayer()                           
    {   
        if (gameManager.shieldHealth == 1)
        {
            gameAudio.PlayOneShot(destroyPlayerSFX, 1.0f);
            Instantiate(destructionVFX, transform.position, Quaternion.identity);  // spawn destruct explosion
            Destroy(gameObject); // Destroy player ship
        }
        else
        {
            gameAudio.PlayOneShot(hitPlayerSFX, 1.0f);
            Instantiate(hitVFX, transform.position, Quaternion.identity);  // spawn hit explosion
        }
        gameManager.GameOver(); 
    }


}
