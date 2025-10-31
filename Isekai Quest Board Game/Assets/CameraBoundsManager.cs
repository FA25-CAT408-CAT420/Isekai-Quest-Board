using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CompositeCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class CameraBoundsManager : MonoBehaviour
{
    private static CameraBoundsManager instance;
    private CompositeCollider2D composite;

    void Awake()
    {
        instance = this;
        composite = GetComponent<CompositeCollider2D>();
        var rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.simulated = true;
    }

    public static void AddRoomBounds(PolygonCollider2D roomBounds)
    {
        if (instance == null)
        {
            Debug.LogError("No CameraBoundsManager found in scene!");
            return;
        }

        // Clone the collider onto the composite object
        PolygonCollider2D clone = Instantiate(roomBounds, instance.transform);
        clone.transform.position = roomBounds.transform.position;
        clone.transform.rotation = roomBounds.transform.rotation;

        // Force Unity to rebuild the composite
        instance.composite.GenerateGeometry();
    }
}
