using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
    public float damage;
    public bool isEnabled = true;
    // Start is called before the first frame update
    void OnCollisionStay2D (Collision2D other)
    {

        if (isEnabled)
        {
            if (other.gameObject.tag == "Player")
            {
                HandlePlayerContact(other.gameObject);
            }  
        }
        else if (!isEnabled)
        {
            return;
        }

    }

    void Start()
    {
        
    }
    private void HandlePlayerContact(GameObject player)
    {
        Debug.Log("Enemy contacted Player – attempting damage + knockback");

        // Damage
        var health = player.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }

        // Stop ongoing move
        var movement = player.GetComponent<PlayerMovement>();
        if (movement != null)
        {
            movement.StopMovementCoroutine();
        }

        // Knockback – this is the critical call
        var knockback = player.GetComponent<PlayerKnockback>();
        if (knockback != null)
        {
            knockback.ApplyKnockback(transform.position);
            Debug.Log("Called ApplyKnockback from enemy at " + transform.position);
        }
        else
        {
            Debug.LogWarning("Player has no PlayerKnockback component!");
        }
    }
}
