using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipyardManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] WaveManager waveManager;
    [SerializeField] UIManager uiManager;

    [SerializeField] GameObject player;
    
    private Vector3 startPosition = new Vector3 (0f, -20f, 0f);
    private int repairPrice = 10;
    private int enhancePrice = 20;
    private int purchaseRepairs;
    private int purchaseEnhance;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        waveManager = GameObject.Find("Wave Manager").GetComponent<WaveManager>();
        uiManager = GameObject.Find("UI Manager").GetComponent<UIManager>();
        
        player = GameObject.Find("Player");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void InitializeShipyard()
    {
        purchaseRepairs = repairPrice * gameManager.difficulty * (waveManager.waveNumber - 1);
        purchaseEnhance = enhancePrice * gameManager.difficulty * (waveManager.waveNumber - 1);
        uiManager.DisplayShipyard(purchaseRepairs, purchaseEnhance);
    }

    public void RepairShields()
    {
        ++gameManager.shieldHealth;
        
        gameManager.UpdateCredits(-purchaseRepairs);
        uiManager.DisplayShields();
        uiManager.DisplayShipyard(purchaseRepairs, purchaseEnhance);
    }

    public void EnhanceShields()
    {
        ++gameManager.shieldHealth;
        ++uiManager.shieldSlider.maxValue;
        ++uiManager.shipyardSlider.maxValue;

        gameManager.UpdateCredits(-purchaseEnhance);
        uiManager.DisplayShields();
        uiManager.DisplayShipyard(purchaseRepairs, purchaseEnhance);
    }
    
    public void LeaveShipyard()
    {
        if(player != null) // In case player was destroyed as the wave ended
        {
            player.SetActive(true);
            player.transform.position = startPosition;

            //gameMusic.Play();
            gameManager.engineAudio.Play();

            uiManager.UILeaveShipyard();
        }
    }


}
