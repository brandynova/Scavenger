using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Scavenger Lite
public class MovingBackground : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    public float speed;
    
    // Start is called before the first frame update    
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    //moving the background stars with the defined speed
    private void Update()
    {
        UpdateBackgroundPosition();
    }

    void UpdateBackgroundPosition()
    {
        if (gameManager.isGameActive && !gameManager.isPaused)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime); 
        }
    }
}
