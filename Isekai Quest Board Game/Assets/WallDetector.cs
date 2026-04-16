using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetector : MonoBehaviour
{
    public float range = 2f;
    public Vector2 direction;
    public GameObject blocker;
    [SerializeField] private LayerMask wallLayer;

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, range, wallLayer);

        if (hit.collider != null)
        {
            Instantiate(blocker, transform.position, Quaternion.identity);
        }

        // Draws a 10-meter green line from object position forward
        Debug.DrawRay(transform.position, direction * range, Color.green);
        Destroy(gameObject);
    }
}
