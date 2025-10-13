using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class NavigateState : State
{
    public Vector2 destination;
    public float moveSpeed = 200f;
    public float nextWaypointDistance = 0.5f;
    //public State animation;

    Seeker seeker;
    Path path;
    int currentWaypoint;
    bool reachedEndOfPath;

    public override void Enter()
    {
        seeker = core.GetComponent<Seeker>();

        if (seeker != null)
        {
            seeker.StartPath(rb.position, destination, OnPathComplete);
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

         Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * moveSpeed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        // Flip sprite
        if (rb.velocity.x != 0)
        {
            core.transform.localScale = new Vector3(Mathf.Sign(rb.velocity.x), 1, 1);
        }

       /* if (Vector2.Distance(core.transform.position, destination) < threshold) {
            isComplete = true;
        }
        core.transform.localScale = new Vector3(Mathf.Sign(rb.velocity.x), 1, 1);
        */
    }

    public override void FixedDo(){
        Vector2 direction = (destination - (Vector2)core.transform.position).normalized;
        rb.velocity = new Vector2(direction.x, direction.y * moveSpeed);
    }
}
