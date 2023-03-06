using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Scavenger Lite

public class PlayerController : MonoBehaviour

{
    [SerializeField] GameManager gameManager;

    [SerializeField] public GameObject destructionVFX;
    [SerializeField] public GameObject hitVFX;
    [SerializeField] public GameObject shipGun;
    [SerializeField] public float destructionTime;

    private ParticleSystem shipGunVFX;
    private AudioSource gameAudio;

    private GameObject pooledMissile;
    
    private float horizontalInput;
    private float speed = 20.0f;
    private float xBounds = 30f;
    private float yOffset = 1.1f;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
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
                gameAudio.PlayOneShot(gameManager.launchMissileSFX, 0.5f);

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
            CollectMineral(other.gameObject);
        }
        else if(other.gameObject.CompareTag("Asteroid"))
        {
            Destroy(other.gameObject);
            HitPlayer();
        }
    }

    // Destroy the mineral and increment credits - the mineral's credit value is also the index to the mineral
    void CollectMineral(GameObject mineral)
    {
            int addToCredits = mineral.GetComponent<ObjectBehaviour>().creditValue;
    
            gameAudio.PlayOneShot(gameManager.collectMineralSFX, 1.0f);

            gameManager.UpdateCredits(addToCredits);
            gameManager.UpdateMineralCount(addToCredits); 

            Destroy(mineral); 
    }
    
    // Destroy the player if health is down to 1.  Otherwise reduce shields and keep playing.
    void HitPlayer()                           
    {   
        if (gameManager.shieldHealth == 1)
        {
            gameAudio.PlayOneShot(gameManager.destroyPlayerSFX, 1.0f);
        
            Destroy(gameObject); // Destroy player ship
            Instantiate(destructionVFX, transform.position, Quaternion.identity);  // spawn explosion
        }
        else
        {
            gameAudio.PlayOneShot(gameManager.hitPlayerSFX, 1.0f);
            Instantiate(hitVFX, transform.position, Quaternion.identity);  // spawn explosion
        }
        gameManager.GameOver(); 
    }
}
