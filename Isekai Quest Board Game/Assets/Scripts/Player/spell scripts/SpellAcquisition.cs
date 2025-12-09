using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellAcquisition : MonoBehaviour
{
    private PlayerCombat playerCombat;
    private GameManager gameManager;
    public Spells spellData;
    public int price = 5;
    public GameObject prefabReference;

    void Start(){
        playerCombat = GameObject.FindWithTag("Player").GetComponent<PlayerCombat>();
        gameManager = FindObjectOfType<GameManager>();
    }

    public void Interacted(){
        if (gameManager.soulPoints >= price) {
            gameManager.soulPoints -= price;
            Debug.Log("Spell destroyed: " + gameObject.name);
            playerCombat.specials.Add(spellData);

            if (prefabReference != null)
            {
                gameManager.totalSpells.Remove(prefabReference);
            }
                
            Destroy(gameObject);
        }
        else {

        }
        
    }
}
