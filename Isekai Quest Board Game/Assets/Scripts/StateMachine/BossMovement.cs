using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : State
{
    public NavigateState navigate;

    public float orbitRadiusInTiles = 4f;
    public Vector2 tileSize = new Vector2(1f, 1f);
    public float pauseDuration = 1.5f;

    private Transform player;
    private bool isWaiting;
    private float waitTimer;

    public override void Enter()
    {
        if (player ==null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        GoToNextOrbitTile();

        navigate.SetCore(core);
    }

    public override void Do()
    {
        if (machine.state == navigate)
        {
            if (navigate.isComplete)
            {
                isWaiting = true;
                waitTimer = pauseDuration;
                rb.velocity = Vector2.zero;
            }
        }
        else if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                isWaiting = false;
                GoToNextOrbitTile();
            }
        }
    }

     private void GoToNextOrbitTile()
    {
        // Choose a random angle around the player for orbiting
        float angle = Random.Range(0f, Mathf.PI * 2f);
        Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * orbitRadiusInTiles * tileSize;

        Vector2 orbitTarget = (Vector2)player.position + offset;

        // Snap to nearest tile
        Vector2 snapped = SnapToTile(orbitTarget);

        // Send to NavigateState for A* pathfinding
        navigate.destination = snapped;

        // Switch to NavigateState
        Set(navigate, true);
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
    }
}
