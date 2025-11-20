using UnityEngine;

public class TransitionPoint : MonoBehaviour
{
    [Header("Where should this point send the player?")]
    public Transform destinationPoint;   // Drag another TransitionPoint here

    [Header("Optional: Which object triggers the teleport?")]
    public string triggeringTag = "Player";  // You can change this in Inspector

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(triggeringTag))
            return;

        // Stop player movement coroutine so it doesn't keep running after teleport
        PlayerMovement pm = collision.GetComponent<PlayerMovement>();
        if (pm != null)
        {
            pm.StopMovementCoroutine();
            pm.isMoving = false;        // hard reset
        }

        // Optional: If using RigidBody2D, clear velocity
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
            Debug.LogWarning($"{name} has no destination assigned!", this);
        }
    }
}
