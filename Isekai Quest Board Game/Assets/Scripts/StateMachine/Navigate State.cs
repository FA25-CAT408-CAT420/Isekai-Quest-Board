using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class NavigateState : State
{
    public Vector2 destination;
    public float moveSpeed = 5f;
    public float nextWaypointDistance = 0.5f;
    public Vector2Int tileSize = new Vector2Int(32,32);
    //public State animation;

    Seeker seeker;
    Path path;
    int currentWaypoint;
    private bool reachedEndOfPath;

    public override void Enter()
    {
        seeker = core.GetComponent<Seeker>();

        if (seeker != null)
        {
            seeker.StartPath(SnapToTile(rb.position), SnapToTile(destination), OnPathComplete);
        }

        currentWaypoint =0;
        reachedEndOfPath = false;
        //Set(animation, true);
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

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            isComplete = true;
            rb.velocity = Vector2.zero;
            return;
        }

        // Move toward next tile center
        Vector2 targetPosition = SnapToTile(path.vectorPath[currentWaypoint]);
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

     Vector2 SnapToTile(Vector2 pos)
    {
        float snappedX = Mathf.Round(pos.x / tileSize.x) * tileSize.x;
        float snappedY = Mathf.Round(pos.y / tileSize.y) * tileSize.y;
        return new Vector2(snappedX, snappedY);
    }

    public override void Exit()
    {
        rb.velocity = Vector2.zero;
    }
}
