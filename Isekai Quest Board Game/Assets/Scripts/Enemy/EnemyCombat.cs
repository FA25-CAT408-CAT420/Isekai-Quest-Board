using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public GameObject SlimeSpit;
    public float damage;
    public Transform attackPoint;
    public float weaponRange;
    public LayerMask playerLayer;
    public EnemyBase baseScript;

    void Start()
    {
        
    }

    void OnCollisionEnter2D (Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            HandlePlayerContact(other.gameObject);
        }
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

    public void Attack()
    {
        Debug.Log("Attacking Player Now!");
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, playerLayer);

        if (hits.Length > 0)
        {
            hits[0].GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }

    void Shoot()
    {
        Instantiate(SlimeSpit, attackPoint.position, Quaternion.identity);
    }
}
