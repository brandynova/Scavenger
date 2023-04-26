using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Scavenger v2
public class DifficultyControl : MonoBehaviour
{
    [SerializeField] MenuUIHandler menuUIHandler;
    
    //public TitleScreenController titleScreenController;
    //public UIManager uiManager;
    public int difficulty; // Set per button (easy, normal, hard)

    private Button button;
    

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SetDifficulty);
        menuUIHandler = GameObject.Find("Menu UI Manager").GetComponent<MenuUIHandler>();
    }
    

    void SetDifficulty()
    {
        MainManager.Instance.gameDifficulty = difficulty;
        menuUIHandler.ManageDifficultyButtons();
    }
}
