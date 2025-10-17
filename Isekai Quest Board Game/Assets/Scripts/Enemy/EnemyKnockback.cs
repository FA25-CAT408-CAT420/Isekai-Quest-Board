using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnockback : MonoBehaviour
{
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private Rigidbody2D rb;
    public void Knockback(Transform playerTransform, float knockbackForce)
    {
        Vector2 direction = (transform.position - playerTransform.position).normalized * knockbackForce;
        //rb.velocity = direction * knockbackForce;
        Vector2 directionInt = new Vector2((int)direction.x, (int)direction.y);
        var targetPos = transform.position;
        targetPos += (Vector3)direction;

        StartCoroutine(Move(targetPos));
        Debug.Log("knockback applied.");
        Debug.Log(direction);
    }

    IEnumerator Move(Vector3 targetPos)
    {
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 5 * Time.deltaTime);
            yield return null;
        }

        targetPos.x = Mathf.RoundToInt(targetPos.x);
        targetPos.y = Mathf.RoundToInt(targetPos.y);
        targetPos.z = Mathf.RoundToInt(targetPos.z);
        transform.position = targetPos;
    }
}
