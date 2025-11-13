using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerInteractions : MonoBehaviour
{
    public GameManager gameManager;
    public PlayerInputActions playerControls;
    private InputAction interact;

    private Collider2D nearbySpell;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnEnable()
    {
        interact = playerControls.Player.Interact;
        interact.Enable();
        interact.performed += OnInteractPerformed;
    }

    private void OnDisable()
    {
        interact.performed -= OnInteractPerformed;
        interact.Disable();
    }

    private void OnTriggerEnter2D (Collider2D other){
        if (other.gameObject.CompareTag("Soul"))
        {
            gameManager.soulPoints++;
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Spell")){
            nearbySpell = other;
        }

        if (other.gameObject.CompareTag("LockedRoom")){
            Debug.Log("HIT THE ROOM");
            other.gameObject.GetComponent<SpawnLockedRoom>().Spawn();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Spell") && other == nearbySpell)
        {
            nearbySpell = null;
        }
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        if (nearbySpell != null)
        {
            nearbySpell.GetComponent<SpellAcquisition>().Interacted();
        }
    }
}
