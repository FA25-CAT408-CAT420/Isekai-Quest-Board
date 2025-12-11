using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrapController : MonoBehaviour
{
    private Animator anim; // Reference to the Animator component

    void Start()
    {
        // Get the Animator component attached to this GameObject
        anim = GetComponent<Animator>();
    }

    // This function is called when another collider enters the trigger collider attached to this object
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object entering the trigger is the player (assuming the player has the tag "Player")
        if (other.CompareTag("Player"))
        {
            // Set the Animator Trigger to play the spike animation
            anim.SetTrigger("SpikeTrigger");
        }
    }
}