using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Scavenger Lite
public class DifficultyControl : MonoBehaviour
{

    //public TitleScreenController titleScreenController;
    public UIManager uiManager;
    public int difficulty; // Set per button (easy, medium, hard)

    private Button button;
    

    // Start is called before the first frame update
    void Start()
    {
        uiManager = GameObject.Find("UI Manager").GetComponent<UIManager>();

        button = GetComponent<Button>();
        button.onClick.AddListener(SetDifficulty);
    }
    

    void SetDifficulty()
    {
        uiManager.StartScreenTransition(difficulty);
    }

 
}
