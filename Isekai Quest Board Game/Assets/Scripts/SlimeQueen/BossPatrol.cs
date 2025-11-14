using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BossPatrol : State
{
    public BossNavigation bossNavigation;

    public float patrolRadiusInTiles = 8f;  // How far around the arena to move
    public Vector2 tileSize = new Vector2(2f, 2f);
    public float pauseDuration = 2f;

    private Vector2 arenaCenter;            // Center point to orbit around
    private bool hasCenter = false;
    private bool isWaiting;
    private float waitTimer;
    private bool isNavigating;
    private Vector2 lastDestination;

    public override void Enter()
    {
        // Use starting position as arena center (or assign manually if needed)
        if (!hasCenter)
        {
            arenaCenter = SnapToTile(core.transform.position);
            hasCenter = true;
        }

        bossNavigation.SetCore(core);  // Ensure both share the same core
        isWaiting = false;
        isNavigating = false;
        waitTimer = 0f;

        GoToNextTile();
    }

    public override void Do()
    {
        // Handle pause timing
        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                isWaiting = false;
                GoToNextTile();
            }
            return;
        }

        // While navigating, run its behavior
        if (isNavigating)
        {
            state?.DoBranch();

            if (bossNavigation.isComplete)
            {
                isNavigating = false;
                StartPause();
            }
            return;
        }
    }

    private void GoToNextTile()
    {
        if (isNavigating) return;

        // Pick a random orbit destination around the arena center
        float angle = Random.Range(0f, Mathf.PI * 2f);
        Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * patrolRadiusInTiles * tileSize;
        Vector2 targetTile = SnapToTile(arenaCenter + offset);

        // Avoid picking nearly the same tile twice
        if (Vector2.Distance(targetTile, lastDestination) < 0.1f)
            return;

        lastDestination = targetTile;

        bossNavigation.destination = targetTile;
        Set(bossNavigation, true);
        isNavigating = true;
    }

    private void StartPause()
    {
        isWaiting = true;
        waitTimer = pauseDuration;
        rb.velocity = Vector2.zero;
    }

    private Vector2 SnapToTile(Vector2 worldPos)
    {
        float snappedX = Mathf.Round(worldPos.x / tileSize.x) * tileSize.x;
        float snappedY = Mathf.Round(worldPos.y / tileSize.y) * tileSize.y;
        return new Vector2(snappedX, snappedY);
    }

    public override void Exit()
    {
        isWaiting = false;
        isNavigating = false;
    }
}
