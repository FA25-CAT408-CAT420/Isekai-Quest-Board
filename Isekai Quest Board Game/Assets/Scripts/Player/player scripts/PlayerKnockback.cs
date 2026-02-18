using UnityEngine;
using System.Collections;

public class PlayerKnockback : MonoBehaviour
{
    public LayerMask wallLayer;
    public float tileSize = 1f;
    public float rayDistance = 0.7f;
    public float knockbackSpeed = 5f;      // ← Tweak this! 4–8 range for smooth ~0.2–0.3s push
    public float knockbackDuration = 0.3f; // Safety max time (prevents infinite if stuck)

    // NEW – short delay before allowing input again (prevents immediate glide from held stick)
    public float postKnockStunTime = 0.1f;

    private Vector2 knockDir;
    private PlayerMovement playerMovement;
    private Coroutine knockbackCoroutine;

    // NEW – reference to Rigidbody2D so we can zero velocity
    private Rigidbody2D rb;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();  // ← grab it here

        if (playerMovement == null)
        {
            Debug.LogError("PlayerKnockback: PlayerMovement component not found!");
        }
        if (rb == null)
        {
            Debug.LogError("PlayerKnockback: Rigidbody2D component not found!");
        }
    }

    public void ApplyKnockback(Vector3 enemyPosition)
    {
        // Prevent overlap / spam
        if (knockbackCoroutine != null)
        {
            StopCoroutine(knockbackCoroutine);
            knockbackCoroutine = null;
        }

        // Interrupt normal movement
        playerMovement.StopMovementCoroutine();

        Vector3 rawDir = (transform.position - enemyPosition).normalized;

        // Cardinalize (same as your movement)
        if (Mathf.Abs(rawDir.x) > Mathf.Abs(rawDir.y))
            knockDir = new Vector2(Mathf.Sign(rawDir.x), 0);
        else
            knockDir = new Vector2(0, Mathf.Sign(rawDir.y));

        // Check if path is blocked before starting
        RaycastHit2D initialHit = Physics2D.Raycast(transform.position, knockDir, tileSize, wallLayer);
        if (initialHit.collider != null)
        {
            Debug.Log("Knockback blocked by wall (initial check).");
            return; // No knockback at all
        }

        Vector3 targetPos = transform.position + (Vector3)knockDir * tileSize;

        knockbackCoroutine = StartCoroutine(KnockbackRoutine(targetPos));
    }

    private IEnumerator KnockbackRoutine(Vector3 targetPos)
    {
        float elapsed = 0f;
        bool hitWallDuringMove = false;

        while ((targetPos - transform.position).sqrMagnitude > 0.001f && elapsed < knockbackDuration)
        {
            // Mid-move wall check (same as your PlayerMovement)
            RaycastHit2D hit = Physics2D.Raycast(transform.position, knockDir, rayDistance, wallLayer);
            if (hit.collider != null)
            {
                hitWallDuringMove = true;
                Debug.Log("Knockback stopped mid-move - hit wall.");
                break;
            }

            // Move smoothly
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPos,
                knockbackSpeed * Time.deltaTime
            );

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Final snap to grid
        transform.position = new Vector3(
            Mathf.Round(transform.position.x),
            Mathf.Round(transform.position.y),
            Mathf.Round(transform.position.z)
        );

        // NEW: stop any residual physics sliding
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        // NEW: short stun before player can move again (prevents held input from starting new move instantly)
        yield return new WaitForSeconds(postKnockStunTime);

        knockbackCoroutine = null;
        Debug.Log($"Knockback finished (wallHitDuringMove={hitWallDuringMove})");
    }

    public void StopKnockbackCoroutine()
    {
        if (knockbackCoroutine != null)
        {
            StopCoroutine(knockbackCoroutine);
            knockbackCoroutine = null;
        }

        // Force snap + zero velocity if interrupted mid-knock
        transform.position = new Vector3(
            Mathf.Round(transform.position.x),
            Mathf.Round(transform.position.y),
            Mathf.Round(transform.position.z)
        );

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

    private void OnDisable()
    {
        StopKnockbackCoroutine();
    }
}