using UnityEngine;
using System.Collections;

public class PlayerKnockback : MonoBehaviour
{
    public LayerMask wallLayer;
    public float tileSize = 1f;
    public float rayDistance = 0.7f;
    private Vector2 moveDir;
    public PlayerMovement playerMovement;
    public Coroutine knockbackCoroutine;

    public void ApplyKnockback(Vector3 enemyPosition)
    {
        // Cancel existing knockback
        if (knockbackCoroutine != null)
        {
            StopCoroutine(knockbackCoroutine);
            knockbackCoroutine = null;
        }

        playerMovement.isMoving = true; // lock movement input

        Vector3 dir = (transform.position - enemyPosition).normalized;

        // Cardinalize
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            moveDir = new Vector2(Mathf.Sign(dir.x), 0);
        else
            moveDir = new Vector2(0, Mathf.Sign(dir.y));

        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDir, tileSize, wallLayer);
        if (hit.collider != null)
        {
            Debug.Log("Knockback blocked by wall.");
            playerMovement.isMoving = false;
            return;
        }

        Vector3 targetPos = transform.position + (Vector3)moveDir;
        knockbackCoroutine = StartCoroutine(Move(targetPos));
    }

    IEnumerator Move(Vector3 targetPos)
    {
        float elapsed = 0f;
        float timeout = 0.3f; // safety timeout
        bool hitWall = false;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon && elapsed < timeout)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDir, rayDistance, wallLayer);
            if (hit.collider != null)
            {
                hitWall = true;
                break;
            }

            transform.position = Vector3.MoveTowards(transform.position, targetPos, 15f * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = new Vector3(
            Mathf.Round(transform.position.x),
            Mathf.Round(transform.position.y),
            Mathf.Round(transform.position.z)
        );

        playerMovement.isMoving = false;
        knockbackCoroutine = null;
        Debug.Log($"Knockback finished (wallHit={hitWall})");
    }

    public void StopKnockbackCoroutine()
    {
        if (knockbackCoroutine != null)
        {
            StopCoroutine(knockbackCoroutine);
            knockbackCoroutine = null;
            playerMovement.isMoving = false;
        }
    }
}
