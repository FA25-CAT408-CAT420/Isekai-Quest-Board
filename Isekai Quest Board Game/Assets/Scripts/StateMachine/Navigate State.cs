using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class NavigateState : State
{
    public Vector2 destination;
    public float moveSpeed = 5f;
    public float nextWaypointDistance = 0.1f;
    public Vector2 tileSize = new Vector2(1f,1f);

    Seeker seeker;
    Path path;
    int currentWaypoint;
    private bool reachedEndOfPath;

    public float pathUpdateRate = 0.25f; 
    private float pathUpdateTimer = 0f;

    public override void Enter()
    {
        seeker = core.GetComponent<Seeker>();

        reachedEndOfPath = false;
        isComplete = false; 

        // Start A* pathfinding from snapped positions
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

         pathUpdateTimer += Time.deltaTime;

        if (pathUpdateTimer >= pathUpdateRate)
        {
            pathUpdateTimer = 0f;
            seeker.StartPath(rb.position, destination, OnPathComplete);
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            isComplete = true;
            rb.velocity = Vector2.zero;
            return;
        }

        // Move toward next tile center
        Vector2 targetPosition = path.vectorPath[currentWaypoint];
        Vector2 newPosition = Vector2.MoveTowards(rb.position, targetPosition, moveSpeed * Time.deltaTime);
        rb.MovePosition(newPosition);


        // When close to the next tile, go to the next waypoint
        float distance = Vector2.Distance(rb.position, targetPosition);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        // Face direction (horizontal only)
        float dirX = targetPosition.x - rb.position.x;
        if (Mathf.Abs(dirX) > 0.01f)
        {
            core.transform.localScale = new Vector3(Mathf.Sign(dirX), 1, 1);
        }
    }

     Vector2 SnapToTile(Vector2 worldPos)
    {
        return new Vector2(Mathf.Round(worldPos.x / tileSize.x) * tileSize.x, Mathf.Round(worldPos.y / tileSize.y) * tileSize.y);
    }

    public override void Exit()
    {
        rb.velocity = Vector2.zero;

        seeker.CancelCurrentPathRequest();
    }
}
