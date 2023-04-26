using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
#if UNITY_EDITOR
    using UnityEditor;
#endif

// Scavenger v2

// Sets the script to be executed later than all default scripts
// This is helpful for UI, since other things may need to be initialized before setting the UI
[DefaultExecutionOrder(1300)]
public class MenuUIHandler : MonoBehaviour
{
    [SerializeField] GameObject menuScreen;
    [SerializeField] GameObject newGameScreen;
    [SerializeField] GameObject settingsScreen;
    [SerializeField] GameObject creditsScreen;
    [SerializeField] GameObject instructionsScreen;
    [SerializeField] GameObject confirmSaveDialog;
    [SerializeField] GameObject clearAllDialog;
    [SerializeField] GameObject confirmClearAllDialog;

    [SerializeField] ScoresUIHandler scoresUIHandler;

    [SerializeField] public GameObject scoreScreen;
    
    [SerializeField] Button startButton;
    [SerializeField] TextMeshProUGUI startButtonText;
    
    [SerializeField] TextMeshProUGUI easyText;
    [SerializeField] TextMeshProUGUI normalText;
    [SerializeField] TextMeshProUGUI hardText;

    [SerializeField] private TMP_InputField nameInput;
    
    //private VolumeSliders volumeSlider;
    
    private float alphaMax = 1f;
    private float alphaFade = .3f;
    private Color selectedColor = new Color(1f, 0.9f, .7f, 1f);
    private Color deselectedColor = new Color(0.83f, 0.83f, 0.83f, .3f);

    private void Awake()
    {
        //Debug.Log("MenuUI: I'm awake!");
        MusicManager.Instance.PlayGameMusic();

        scoresUIHandler = GameObject.Find("ScoresUIHandler").GetComponent<ScoresUIHandler>();
        
        DisplayMenuScreen(); // Display the Menu screen to get the game started
    }
    
    public void DisplayMenuScreen()
    {
        newGameScreen.gameObject.SetActive(false);
        settingsScreen.gameObject.SetActive(false);
        scoreScreen.gameObject.SetActive(false);
        creditsScreen.gameObject.SetActive(false);
        instructionsScreen.gameObject.SetActive(false);

        menuScreen.gameObject.SetActive(true);
    }

    // Called from the Menu and Game Over screens
    public void DisplayNewGameScreen()
    {
        menuScreen.gameObject.SetActive(false);
        newGameScreen.gameObject.SetActive(true);

        startButton = GameObject.Find("Start Button").GetComponent<Button>(); //Not very efficient putting it here but unless I move Menu to a new scene, this will have to do

        CheckForPilotName(); // check for existing pilot name, when playing multiple games in one session, or when pilot name was saved in a previous session

        nameInput.onEndEdit.AddListener(delegate { NewPilotName(nameInput.text); });

        ManageDifficultyButtons();
        ManageStartButton();
    }
        
    public void CheckForPilotName()
    {
        //Debug.Log("Check for Pilot Name: " + MainManager.Instance.pilotName);
        if (!string.IsNullOrEmpty(MainManager.Instance.pilotName))
            nameInput.text = MainManager.Instance.pilotName;
        else
            nameInput.text = "";
    }
    
    public void ManageDifficultyButtons()
    {
        easyText.color = deselectedColor;
        normalText.color = deselectedColor;
        hardText.color = deselectedColor;

        // Do something to the difficulty button that is currently selected
        switch(MainManager.Instance.gameDifficulty)
        {
            case 1:
                //easyText.alpha = alphaMax;
                easyText.color = selectedColor;
                break;
            case 2:
                //normalText.alpha = alphaMax;
                normalText.color = selectedColor;
                break;
            case 3:
                //hardText.alpha = alphaMax;
                hardText.color = selectedColor;
                break;
        }
        ManageStartButton();
    }
    
    private void ManageStartButton()
    {
        if(MainManager.Instance.gameDifficulty <= 0 || string.IsNullOrEmpty(MainManager.Instance.pilotName))
        {
            //Debug.Log("Need to enter a pilot name and/or select difficulty");
            startButton.interactable = false;
            startButtonText.alpha = alphaFade;
        }
        else
        {
            startButton.interactable = true;
            startButtonText.alpha = alphaMax;
        }
    }

    public void NewPilotName(string newName)
    {
        MainManager.Instance.pilotName = newName;
        ManageStartButton();
    }
    
    // Called from High Scores button on the Menu screen 
    public void LoadHighScoresScreen()
    {
        menuScreen.gameObject.SetActive(false);
        //scoreScreen.gameObject.SetActive(true);
        scoresUIHandler.DisplayHighScores();
    }

    //private void ResetHighScoreFields()
    //{
    //    if(MainManager.Instance.highScoreName1 = null)
    //        
    //}
    
    // Called from Credits button on the Menu screen 
    public void DisplaySettingsScreen()
    {
        menuScreen.gameObject.SetActive(false);
        confirmSaveDialog.gameObject.SetActive(false);
        clearAllDialog.gameObject.SetActive(false);
        confirmClearAllDialog.gameObject.SetActive(false);
        settingsScreen.gameObject.SetActive(true);
    }
    
    // Called from Save Settings button on the Settings screen 
    public void DisplaySaveDialog()
    {
        MainManager.Instance.SavePlayerData();
        confirmSaveDialog.gameObject.SetActive(true);
    }
    
    // Called from Okay button on Save All Settings dialogue 
    public void CloseSaveDialog()
    {
        confirmSaveDialog.gameObject.SetActive(false);
    }

    // Called from Clear All button on the Settings screen 
    public void DisplayClearAllDialog()
    {
        clearAllDialog.gameObject.SetActive(true);
    }
    
    // Called from Clear All dialogue (No) and MainManager after clearing data (Yes)
    public void CloseClearAllDialog()
    {
        clearAllDialog.gameObject.SetActive(false);
    }

    // Called from Clear All dialogue after clearing data (Yes)
    public void DisplayConfirmClearAllDialog()
    {
        Debug.Log("CLEAR ALL DATA NOW");
        clearAllDialog.gameObject.SetActive(false);
        //Clear all saved data, including high scores
        MainManager.Instance.ClearData();
        confirmClearAllDialog.gameObject.SetActive(true);
    }
    
    // Called from Confirm Clear All dialogue after clearing data (Okay)
    public void CloseConfirmClearAllDialog()
    {
        confirmClearAllDialog.gameObject.SetActive(false);

        //Need to force the volume slider back to the default after confirming the Clear All action.
        //Try reloading this scene (Menu) - this did not reset the volume
        //MainManager.Instance.LoadScene(0);
        //Try calling from MusicManager, the parent of VolumeSliders.  Update: This does reset the volume, but not the slider

        MusicManager.Instance.ResetVolumeSliders();

    }

    // Called from Credits button on the Menu screen)
    public void DisplayCreditsScreen()
    {
        menuScreen.gameObject.SetActive(false);
        creditsScreen.gameObject.SetActive(true);
    }

    // Called from Instructions button on the Menu screen 
    public void DisplayInstructionsScreen()
    {
        menuScreen.gameObject.SetActive(false);
        instructionsScreen.gameObject.SetActive(true);
    }

    // Called from the Start button on the New Game screen 
    public void StartNew()
    {
        int gameIndex = 1;

        //Debug.Log("UI - Start New Game button clicked");

        MainManager.Instance.LoadScene(gameIndex);
    }

    // Called from the Exit button on the Menu screen 
    public void Exit()
    {
        // Exit the Unity Player if running the editor, otherwise exit the application 
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }
}
