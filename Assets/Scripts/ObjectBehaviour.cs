using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Scavenger Lite
public class ObjectBehaviour : MonoBehaviour
{
    [SerializeField] SpawnManager spawnManager;
    [SerializeField] GameManager gameManager;

    [SerializeField] GameObject destructionVFX;

    [SerializeField] public float rotateSpeed;
    [SerializeField] public float speed;
    [SerializeField] public int creditValue;
    
    private Rigidbody objectRb;
    private AudioSource gameAudio;
    private float yBounds = -40f;

    // Start is called before the first frame update    
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        gameAudio = gameManager.GetComponent<AudioSource>();
        objectRb = GetComponent<Rigidbody>();

        LaunchObject();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateObjectPosition();
        CheckObjectBoundary();
    }

    // Set the speed of the object based on the difficulty selected by the user
    void LaunchObject()
    {
        float objectSpeed = speed + gameManager.difficulty * (gameManager.difficulty + 1);
        objectRb.velocity = new Vector3(0, -objectSpeed, 0); 
    }
    
    void UpdateObjectPosition()
    {
        transform.Rotate(Time.deltaTime*rotateSpeed, Time.deltaTime*rotateSpeed, Time.deltaTime*rotateSpeed);
    }

    void CheckObjectBoundary()
    {
        // Destroy the object if it travels outside the game boundary.  Deduct the score value of the object from the total score when playing on HARD difficulty
        if (transform.position.y < yBounds)
        {
            if (gameManager.difficulty == 3) 
            {
                int deductCredits = -gameObject.GetComponent<ObjectBehaviour>().creditValue;
                gameManager.UpdateCredits(deductCredits);
            }
            Destroy(gameObject);
        }
    }


    
    // Do something when the player collides with an object (no physics)
    void OnTriggerEnter(Collider other)
    {
        if (gameObject.CompareTag("Asteroid"))
        {
            if (other.gameObject.CompareTag("Mineral"))
            {
                Destroy(other.gameObject); // Destroy the mineral if it collides with an asteroid
            }
            else if (other.gameObject.CompareTag("Missile"))
            {
                other.gameObject.SetActive(false); // deactivate the missile
                spawnManager.GenerateMinerals(transform.position);
                DestroyAsteroid();
            }
        }
    }

    
    void DestroyAsteroid()                           
    {        
        int randomIndex = Random.Range(0,gameManager.destroyAsteroidsSFX.Count);
        gameAudio.PlayOneShot(gameManager.destroyAsteroidsSFX[randomIndex], .8f);

        Instantiate(destructionVFX, transform.position, Quaternion.identity); 
        Destroy(gameObject);
    }
}
