using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Scavenger Lite
public class RepeatingBackground : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    public float verticalLimit;
    
    // Start is called before the first frame update    
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    private void Update()
    {        
        //if background goes down below the viewport, move up above the viewport
        if (transform.position.y < -verticalLimit && gameManager.isGameActive && !gameManager.isPaused) 
        {
            RepositionBackground();
        }
    }

    void RepositionBackground() 
    {
        Vector2 verticalOffset = new Vector2(0, verticalLimit * 2f);
        transform.position = (Vector2)transform.position + verticalOffset;
    }
}
