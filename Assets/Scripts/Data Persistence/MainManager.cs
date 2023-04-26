using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.IO;

// Scavenger v2
[DefaultExecutionOrder(1000)]
public class MainManager : MonoBehaviour
{
    //This code enables you to access the MainManager object from any other script.  
    [SerializeField] public static MainManager Instance;

    //Current game data
    [SerializeField] public string pilotName;
    [SerializeField] public int finalScore;
    [SerializeField] public int finalWave;
    [SerializeField] public int gameDifficulty;
    [SerializeField] public float savedVolume;
    [SerializeField] public float defaultVolume = .5f;
    
    //Easy High Score
    [SerializeField] public string highScoreName1;
    [SerializeField] public int highScore1; 
    [SerializeField] public int highScoreWave1;
    [SerializeField] public int highScoreDifficulty1;
    //Normal High Score
    [SerializeField] public string highScoreName2;
    [SerializeField] public int highScore2; 
    [SerializeField] public int highScoreWave2;
    [SerializeField] public int highScoreDifficulty2;
    //Hard High Score
    [SerializeField] public string highScoreName3;
    [SerializeField] public int highScore3; 
    [SerializeField] public int highScoreWave3;
    [SerializeField] public int highScoreDifficulty3;

    [SerializeField] public bool isSavedData = false;
    [SerializeField] public int currentScore;
    [SerializeField] public int maxNameLength = 24;

    //Previous pilot data
    private string previousName;
    private int previousScore; 
    private int previousWave;
    private int previousDifficulty;

    
    //Data class for the player information saved to a JSON file
    [System.Serializable]
    class SaveData
    {
        public string previousName;
        public int previousScore;
        public int previousWave;
        public int previousDifficulty;
        public float savedVolume;

        public string highScoreName1;
        public int highScore1; 
        public int highScoreWave1;
        public int highScoreDifficulty1;

        public string highScoreName2;
        public int highScore2; 
        public int highScoreWave2;
        public int highScoreDifficulty2;
        
        public string highScoreName3;
        public int highScore3; 
        public int highScoreWave3;
        public int highScoreDifficulty3;
    }

    private void Awake()
    {
        // Keep a single instance of MainManager running at a time
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        //Stores the current instance of MainManager in the class member Instance
        Instance = this;

        //Marks the MainManager GameObject attached to this script not to be destroyed when the scene changes
        DontDestroyOnLoad(gameObject);


        LoadPlayerData(); 
    }

    //Read the player info from the JSON file.  Set a flag if the file does not exist or has bee cleared.
    public void LoadPlayerData()
    {
        SaveData data = new SaveData();

        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            data = JsonUtility.FromJson<SaveData>(json);
            
            previousName = data.previousName;
            pilotName = data.previousName;       //set the current player name to the previous name as a curtesy for the player
            previousScore = data.previousScore;
            previousWave = data.previousWave;
            previousDifficulty = data.previousDifficulty;
            gameDifficulty = data.previousDifficulty; //set the current player name to the previous name as a curtesy for the player

            savedVolume = data.savedVolume; 
            
            highScoreName1 = data.highScoreName1;
            highScore1 = data.highScore1;
            highScoreWave1 = data.highScoreWave1;
            highScoreDifficulty1 = data.highScoreDifficulty1;
            
            highScoreName2 = data.highScoreName2;
            highScore2 = data.highScore2;
            highScoreWave2 = data.highScoreWave2;
            highScoreDifficulty2 = data.highScoreDifficulty2;
            
            highScoreName3 = data.highScoreName3;
            highScore3 = data.highScore3;
            highScoreWave3 = data.highScoreWave3;
            highScoreDifficulty3 = data.highScoreDifficulty3;

            // ***** TEST DATA *****
            //pilotName = "pilot_name_test_30_01234567890";
            //pilotName = "";
            //previousWave = -15;
            //gameDifficulty = 25;
            //previousScore = -100;
            //savedVolume = 100;
            //highScoreName1 = "highscore_name1_test_31_4567890";
            //highScore1 = -200000;
            //highScoreWave1 = -30;
            //highScoreDifficulty1 = -1;
            //highScoreName2 = "highscore_name2_test_40_4567890123456789";
            //highScore2 = -2000000000;
            //highScoreWave2 = -3000;
            //highScoreDifficulty2 = 22;
            //highScoreName3 = "highscore_name3_test_25_5";
            //highScore3 = -200;
            //highScoreWave3 = -3;
            //highScoreDifficulty3 = 1;

            ConfirmData();
            isSavedData = true;
        }
        else
        {
            isSavedData = false;
            pilotName = "";
            previousScore = -1;
            previousWave = -1;
            previousDifficulty = -1;
            gameDifficulty = -1;
            savedVolume = defaultVolume;  

            highScoreName1 = "";
            highScoreName2 = "";
            highScoreName3 = "";
        }    
            
        Debug.Log("***** LOAD DATA *****");
        Debug.Log("pilotName: " + pilotName);
        Debug.Log("finalScore: " + finalScore);
        Debug.Log("finalWave: " + finalWave);
        Debug.Log("gameDifficulty: " + gameDifficulty);
        Debug.Log("previousName: " + previousName);
        Debug.Log("previousScore: " + previousScore);
        Debug.Log("previousWave: " + previousWave);
        Debug.Log("previousDifficulty: " + previousDifficulty);
        Debug.Log("savedVolume: " + savedVolume);
        Debug.Log("isSavedData: " + isSavedData);
        Debug.Log("***** END LOAD DATA *****");
    }

    //Check for data integrity, since JSON files can be edited outside of game
    private void ConfirmData()
    {
        if(!string.IsNullOrEmpty(pilotName) && pilotName.Length > maxNameLength) //truncate name if too long
        {
            //Debug.Log("Invalid pilotName" + pilotName);
            pilotName = pilotName.Substring(0, maxNameLength);
            //Debug.Log("pilotName truncated " + pilotName);
        }

        if(previousScore < 0)
        {
            //Debug.Log("Invalid previousScore " + previousScore);
            previousScore = 0;
        }

        if(previousWave < 0)
        {
            //Debug.Log("Invalid previousWave " + previousWave);
            previousWave = 0;
        }

        if(gameDifficulty < 1 || gameDifficulty > 3)
        {
            //Debug.Log("Invalid gameDifficulty " + gameDifficulty);
            gameDifficulty = 0;
        }

        if(savedVolume < 0f || savedVolume > 1f)
        {
            //Debug.Log("Invalid savedVolume " + savedVolume);
            //savedVolume = 1f;
        }
        
        //EASY
        if(!string.IsNullOrEmpty(MainManager.Instance.highScoreName1) && highScoreName1.Length > maxNameLength)
        {
            //Debug.Log("Invalid highScoreName1 " + highScoreName1);
            highScoreName1 = highScoreName1.Substring(0, maxNameLength);
            //Debug.Log("highScoreName1 truncated " + highScoreName1);
        }
        
        if(highScore1 < 0)
        {
            //Debug.Log("Invalid highScore1 " + highScore1);
            highScore1 = 0;
        }

        if(highScoreWave1 < 0)
        {
            //Debug.Log("Invalid highScoreWave1 " + highScoreWave1);
            highScoreWave1 = 0;
        }

        if(highScoreDifficulty1 != 1)
        {
            //Debug.Log("Invalid highScoreDifficulty1 " + highScoreDifficulty1);
            highScoreDifficulty1 = 1;
        }

        //NORMAL
        if(!string.IsNullOrEmpty(MainManager.Instance.highScoreName2) && highScoreName2.Length > maxNameLength)
        {
            //Debug.Log("Invalid highScoreName2 " + highScoreName2);
            highScoreName1 = highScoreName2.Substring(0, maxNameLength);
            //Debug.Log("highScoreName2 truncated " + highScoreName2);
        }
        
        if(highScore2 < 0)
        {
            //Debug.Log("Invalid highScore2 " + highScore2);
            highScore2 = 0;
        }

        if(highScoreWave2 < 0)
        {
            //Debug.Log("Invalid highScoreWave2 " + highScoreWave2);
            highScoreWave2 = 0;
        }

        if(highScoreDifficulty2 != 2)
        {
            //Debug.Log("Invalid highScoreDifficulty2 " + highScoreDifficulty2);
            highScoreDifficulty1 = 2;
        }

        //HARD
        if(!string.IsNullOrEmpty(MainManager.Instance.highScoreName3) && highScoreName3.Length > maxNameLength)
        {
            //Debug.Log("Invalid highScoreName3 " + highScoreName3);
            highScoreName1 = highScoreName3.Substring(0, maxNameLength);
            //Debug.Log("highScoreName3 truncated " + highScoreName3);
        }
        
        if(highScore3 < 0)
        {
            //Debug.Log("Invalid highScore3 " + highScore3);
            highScore3 = 0;
        }

        if(highScoreWave3 < 0)
        {
            //Debug.Log("Invalid highScoreWave3 " + highScoreWave3);
            highScoreWave3 = 0;
        }

        if(highScoreDifficulty3 != 3)
        {
            //Debug.Log("Invalid highScoreDifficulty3 " + highScoreDifficulty3);
            highScoreDifficulty1 = 3;
        }

    }

    //Save player data at Game Over.  Or when manually saving from the Settings screen (Main Menu)
    public void SavePlayerData()
    {
        SaveData data = new SaveData();

        // Check for high scores, based on difficulty level
        switch(gameDifficulty)
        {
            case 1: // EASY
                if(finalScore > highScore1) //New high score
                {
                    highScoreName1 = pilotName;
                    highScore1 = finalScore;
                    highScoreWave1 = finalWave;
                    highScoreDifficulty1 = gameDifficulty;
                }
                break;

            case 2: // NORMAL
                if(finalScore > highScore2) //New high score
                {
                    highScoreName2 = pilotName;
                    highScore2 = finalScore;
                    highScoreWave2 = finalWave;
                    highScoreDifficulty2 = gameDifficulty;
                }
                break;

            case 3: // HARD
                if(finalScore > highScore3) //New high score
                {
                    highScoreName3 = pilotName;
                    highScore3 = finalScore;
                    highScoreWave3 = finalWave;
                    highScoreDifficulty3 = gameDifficulty;
                }
                break;
        }

        //Check if there is no previously saved data, set previous to current values
        if(!isSavedData)
        {
            previousName = pilotName;
            previousScore = finalScore;
            previousWave = finalWave;
            previousDifficulty = gameDifficulty;
            savedVolume = MusicManager.Instance.gameMusic.volume;
        }
        else
            //player may have entered a pilot name and/or difficulty, then returned to the main menu to save settings
            if(string.IsNullOrEmpty(pilotName)) 
                pilotName = previousName;
            if(finalScore <= 0)
            {
                finalScore = previousScore;
                finalWave = previousWave;
            }
            if(gameDifficulty <= 0)
                gameDifficulty = previousDifficulty;

        //Build the data to write to the file
        data.previousName = pilotName;
        data.previousScore = finalScore; 
        data.previousWave = finalWave;
        data.previousDifficulty = gameDifficulty;
        data.savedVolume = MusicManager.Instance.gameMusic.volume;

        data.highScoreName1 = highScoreName1;
        data.highScore1 = highScore1;
        data.highScoreWave1 = highScoreWave1;
        data.highScoreDifficulty1 = highScoreDifficulty1;
        
        data.highScoreName2 = highScoreName2;
        data.highScore2 = highScore2;
        data.highScoreWave2 = highScoreWave2;
        data.highScoreDifficulty2 = highScoreDifficulty2;
        
        data.highScoreName3 = highScoreName3;
        data.highScore3 = highScore3;
        data.highScoreWave3 = highScoreWave3;
        data.highScoreDifficulty3 = highScoreDifficulty3;

        string json = JsonUtility.ToJson(data);
  
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json); 
        
        Debug.Log("***** SAVE DATA *****");
        Debug.Log("Application.persistentDataPath" + Application.persistentDataPath);
        Debug.Log("pilotName: " + pilotName);
        Debug.Log("finalScore: " + finalScore);
        Debug.Log("finalWave: " + finalWave);
        Debug.Log("gameDifficulty: " + gameDifficulty);
        Debug.Log("previousName: " + previousName);
        Debug.Log("previousScore: " + previousScore);
        Debug.Log("previousWave: " + previousWave);
        Debug.Log("previousDifficulty: " + previousDifficulty);
        Debug.Log("savedVolume: " + savedVolume);

        Debug.Log("data.previousName: " + data.previousName);
        Debug.Log("data.previousScore: " + data.previousScore);
        Debug.Log("data.previousWave: " + data.previousWave);
        Debug.Log("data.previousDifficulty: " + data.previousDifficulty);
        Debug.Log("data.savedVolume: " + data.savedVolume);
        Debug.Log("***** END SAVE DATA *****");
    }

    //Clear all high scores, but retain pilot information
    //Called from ScoreUIHandler
    public void ClearHighScores()
    {
        SaveData data = new SaveData();
        
        highScoreName1 = "";
        highScoreName2 = "";
        highScoreName3 = "";
        highScore1 = 0;
        highScore2 = 0;
        highScore3 = 0;

        data.highScoreName1 = "";
        data.highScoreName2 = "";
        data.highScoreName3 = "";
        data.highScore1 = 0;
        data.highScore2 = 0;
        data.highScore3 = 0;

        if(!string.IsNullOrEmpty(previousName))
            data.previousName = previousName;
        else
            data.previousName = pilotName;

        if(previousDifficulty > 0)
            data.previousDifficulty = previousDifficulty;
        else
            data.previousDifficulty = gameDifficulty;

        //Debug.Log("data.previousName: " + data.previousName + ". data.previousDifficulty: " + data.previousDifficulty);

        data.previousScore = previousScore; 
        data.previousWave = previousWave;
        data.savedVolume = MusicManager.Instance.gameMusic.volume;

        //Debug.Log("gameDifficulty: " + gameDifficulty + ". previousDifficulty: " + previousDifficulty);

        string json = JsonUtility.ToJson(data);
  
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json); 
    }

    //Clear all data, including pilot information and high scores
    public void ClearData()
    {
        SaveData data = new SaveData();
        
        pilotName = "";
        previousName = "";
        previousScore = 0;
        previousWave = 0;
        previousDifficulty = 0;
        gameDifficulty = 0;
        savedVolume = defaultVolume;

        highScoreName1 = "";
        highScoreName2 = "";
        highScoreName3 = "";

        //data.previousName = "";
        //data.previousScore = 0;
        //data.previousWave = 0;
        //data.previousDifficulty = 0;

        //data.highScoreName1 = "";
        //data.highScoreName2 = "";
        //data.highScoreName3 = "";

        string json = JsonUtility.ToJson(data);
  
        //File.WriteAllText(Application.persistentDataPath + "/savefile.json", json); // Write select fields to file
        //File.WriteAllText(Application.persistentDataPath + "/savefile.json", string.Empty); // Clear all data from file
        File.Delete(Application.persistentDataPath + "/savefile.json"); // Delete the file
        
        MusicManager.Instance.gameMusic.volume = defaultVolume;
        //Debug.Log("ClearData reset gameMusic volume to: " + MusicManager.Instance.gameMusic.volume);
    }
    
    public string FormatScore(float scoreValue)
    {
        string formatting = "{0:#,##0.##}";
        string formattedScore = string.Format(formatting, scoreValue);
        return formattedScore;
    }

    // Load the input scene:
    // 0 = Menu scene (called from Game Over screen)
    // 1 = MyGame scene (called from the New Game screen)
    public void LoadScene(int sceneIndex)
    {
        //string scenePath = SceneUtility.GetScenePathByBuildIndex(sceneIndex);
        //string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
        //Debug.Log("Load scene: " + sceneName);

        SceneManager.LoadScene(sceneIndex);
    }
}
