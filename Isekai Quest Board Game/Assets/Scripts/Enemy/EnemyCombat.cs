using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public GameObject SlimeSpit;
    public float damage = 5f;
    public Transform attackPoint;
    public float weaponRange;
    public LayerMask playerLayer;

    void OnCollisionEnter2D (Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
            other.gameObject.GetComponent<PlayerMovement>().StopMovementCoroutine();
            other.gameObject.GetComponent<PlayerKnockback>().ApplyKnockback(transform.position);
        }
    }

    /*public void Attack()
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
    }*/
}
