using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipyardManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] WaveManager waveManager;
    [SerializeField] UIManager uiManager;
    [SerializeField] PlayerController playerController;
    
    private AudioSource engineAudio;
    private AudioSource shipyardAudio;

    [SerializeField] GameObject player;
    
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

        player = GameObject.Find("Player");
        playerController= player.GetComponent<PlayerController>();
        
        engineAudio = GameObject.Find("Audio Engine SFX").GetComponent<AudioSource>();
        shipyardAudio = GameObject.Find("Audio Shipyard SFX").GetComponent<AudioSource>();
        shipyardAudio.volume = MusicManager.Instance.gameMusic.volume;
        shipyardAudio.Stop();
    }
        
    public void InitializeShipyard()
    {
        shipyardAudio.Play();
        engineAudio.Pause();

        purchaseRepairShields = repairShieldsPrice * MainManager.Instance.gameDifficulty * (waveManager.waveNumber - 1);
        purchaseUpgradeShields = upgradeShieldsPrice * MainManager.Instance.gameDifficulty * (waveManager.waveNumber - 1);
        purchaseUpgradeThrust = upgradeThrustPrice * MainManager.Instance.gameDifficulty * (waveManager.waveNumber - 1);
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
}
