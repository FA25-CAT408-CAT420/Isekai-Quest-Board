using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BossNavigation : State
{
    public Vector2 destination;
    public float moveSpeed = 4f;
    public float nextWaypointDistance = 0.1f;
    public Vector2 tileSize = new Vector2(1f, 1f);
    public float pathUpdateRate = 0.5f;

    private Seeker seeker;
    private Path path;
    private int currentWaypoint;
    private float pathUpdateTimer;
    private bool reachedEndOfPath;

    public override void Enter()
    {
        seeker = core.GetComponent<Seeker>();

        reachedEndOfPath = false;
        isComplete = false;
        path = null;
        currentWaypoint = 0;
        pathUpdateTimer = 0f;

        Vector2 start = SnapToTile(rb.position);
        Vector2 end = SnapToTile(destination);
        seeker.StartPath(start, end, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    public override void Do()
    {
        if (path == null) return;

        // Periodically refresh path
        pathUpdateTimer += Time.deltaTime;
        if (pathUpdateTimer >= pathUpdateRate)
        {
            pathUpdateTimer = 0f;
            seeker.StartPath(SnapToTile(rb.position), SnapToTile(destination), OnPathComplete);
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            isComplete = true;
            rb.velocity = Vector2.zero;
            return;
        }

        Vector2 targetPosition = (Vector2)path.vectorPath[currentWaypoint];
        Vector2 newPosition = Vector2.MoveTowards(rb.position, targetPosition, moveSpeed * Time.deltaTime);
        rb.MovePosition(newPosition);

        float distance = Vector2.Distance(rb.position, targetPosition);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        // Face direction horizontally
        float dirX = targetPosition.x - rb.position.x;
        if (Mathf.Abs(dirX) > 0.01f)
        {
            core.transform.localScale = new Vector3(Mathf.Sign(dirX), 1, 1);
        }
    }

    private Vector2 SnapToTile(Vector2 worldPos)
    {
        float snappedX = Mathf.Round(worldPos.x / tileSize.x) * tileSize.x;
        float snappedY = Mathf.Round(worldPos.y / tileSize.y) * tileSize.y;
        return new Vector2(snappedX, snappedY);
    }

    public override void Exit()
    {
        rb.velocity = Vector2.zero;
    }
}
