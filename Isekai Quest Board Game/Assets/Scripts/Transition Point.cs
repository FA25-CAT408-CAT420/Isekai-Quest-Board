using UnityEngine;

public class TransitionPoint : MonoBehaviour
{
    public Transform destinationPoint;

    [Header("Object to trigger teleport:")]
    public string triggeringTag = "Player";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(triggeringTag))
            return;

        // Stop player movement coroutine so it doesn't keep running after teleport
        PlayerMovement pm = collision.GetComponent<PlayerMovement>();
        if (pm != null)
        {
            pm.StopMovementCoroutine();
            pm.isMoving = false;  
        }

        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.velocity = Vector2.zero;

        // Teleport
        if (destinationPoint != null)
        {
            collision.transform.position = destinationPoint.position;
        }
        else
        {
            Debug.LogWarning($"{name} has no destination assigned", this);
        }
    }
}
