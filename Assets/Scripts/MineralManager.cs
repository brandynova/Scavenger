using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] UIManager uiManager;
    
    private AudioSource gameAudio;
    [SerializeField] AudioClip collectMineralSFX; 

    [SerializeField] public List<string> mineralName = new List<string>();
    [SerializeField] public List<int> mineralCount = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        uiManager = GameObject.Find("UI Manager").GetComponent<UIManager>();
        
        gameAudio = gameManager.GetComponent<AudioSource>();
    }

    
    public void InitializeMinerals()
    {
        for (int i = 0; i < mineralName.Count; ++i)
        {
            mineralCount.Add(0);
        }
        uiManager.DisplayMinerals();
    }

    
    // Destroy the mineral and increment credits.  Called from PlayerController
    public void CollectMineral(GameObject mineral)
    {
        if(gameManager.isGameActive && !gameManager.isPaused)
        {
                int mineralValue = mineral.GetComponent<ObjectBehaviour>().value;
                int mineralIndex = mineralValue - 1;
    
                gameAudio.PlayOneShot(collectMineralSFX, 1.0f);
                
                ++mineralCount[mineralIndex];
                gameManager.UpdateCredits(mineralValue);
                uiManager.DisplayMinerals();

                Destroy(mineral); 
        }
    }
}
