using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Scavenger Lite
public class DifficultyControl : MonoBehaviour
{

    public GameManager gameManager;
    public int difficulty; // Set per button (easy, medium, hard)

    private Button button;
    

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        

        button = GetComponent<Button>();
        button.onClick.AddListener(SetDifficulty);
    }
    

    void SetDifficulty()
    {
        gameManager.StartFadeTitle(difficulty);
    }

 
}
