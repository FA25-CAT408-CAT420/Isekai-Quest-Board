using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeAcquisition : MonoBehaviour, IShopInterface
{
    private PlayerCombat playerCombat;
    private PlayerStatManager psm;
    private GameManager gameManager;
    public int price = 5;
    public enum StatType {HealthUp, DefenseUp, StrengthUp};
    public StatType currentStat;
    public GameObject prefabReference;

    void Start(){
        playerCombat = GameObject.FindWithTag("Player").GetComponent<PlayerCombat>();
        gameManager = FindObjectOfType<GameManager>();
        psm = GameObject.FindWithTag("Player").GetComponent<PlayerStatManager>();
    }
    public void Initialize(GameObject prefab)
    {
        prefabReference = prefab;
    }

    public void Interacted(){
        if (gameManager.soulPoints >= price) {
            gameManager.soulPoints -= price;
            Debug.Log("Upgrade destroyed: " + gameObject.name);
            HandleCurrentStat(currentStat);
            psm.CalculateStats();
            if (prefabReference != null)
            {
                gameManager.totalSpells.Remove(prefabReference);
            }
            Destroy(gameObject);
        }
        else {
        }
    }

    public void HandleCurrentStat(StatType stat)
    {
        switch (stat)
        {
            case StatType.HealthUp:
                gameManager.gmHealthSP++;
                break;
            case StatType.DefenseUp:
                gameManager.gmDefenseSP++;
                break;
            case StatType.StrengthUp:
                gameManager.gmStrengthSP++;
                break;
            default:
                break;
        }
    }
}
