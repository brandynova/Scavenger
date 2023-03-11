using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipyardManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] WaveManager waveManager;
    [SerializeField] UIManager uiManager;
    [SerializeField] PlayerController playerController;

    [SerializeField] GameObject player;
    
    private Vector3 startPosition = new Vector3 (0f, -20f, 0f);
    private int upgradeThrustPrice = 15;
    private int repairShieldsPrice = 10;
    private int upgradeShieldsPrice = 20;
    private int purchaseRepairShields;
    private int purchaseUpgradeShields;
    private int purchaseUpgradeThrust;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        waveManager = GameObject.Find("Wave Manager").GetComponent<WaveManager>();
        uiManager = GameObject.Find("UI Manager").GetComponent<UIManager>();
        playerController= GameObject.Find("Player").GetComponent<PlayerController>();
        
        player = GameObject.Find("Player");
    }
        
    public void InitializeShipyard()
    {
        purchaseRepairShields = repairShieldsPrice * gameManager.difficulty * (waveManager.waveNumber - 1);
        purchaseUpgradeShields = upgradeShieldsPrice * gameManager.difficulty * (waveManager.waveNumber - 1);
        purchaseUpgradeThrust = upgradeThrustPrice * gameManager.difficulty * (waveManager.waveNumber - 1);
        uiManager.DisplayShipyard(purchaseRepairShields, purchaseUpgradeShields, purchaseUpgradeThrust);
    }

    public void RepairShields()
    {
        ++playerController.shieldHealth;
        
        gameManager.UpdateCredits(-purchaseRepairShields);
        uiManager.DisplayShields();
        uiManager.DisplayShipyard(purchaseRepairShields, purchaseUpgradeShields, purchaseUpgradeThrust);
    }

    public void UpgradeShields()
    {
        ++playerController.shieldHealth;
        ++uiManager.shieldSlider.maxValue;
        ++uiManager.shipyardSlider.maxValue;

        gameManager.UpdateCredits(-purchaseUpgradeShields);
        uiManager.DisplayShields();
        uiManager.DisplayShipyard(purchaseRepairShields, purchaseUpgradeShields, purchaseUpgradeThrust);
    }

    public void UpgradeThrust()
    {
        playerController.horizontalThrust += 10f;

        gameManager.UpdateCredits(-purchaseUpgradeThrust);
        uiManager.DisplayThrust();
        uiManager.DisplayShipyard(purchaseRepairShields, purchaseUpgradeShields, purchaseUpgradeThrust);
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
