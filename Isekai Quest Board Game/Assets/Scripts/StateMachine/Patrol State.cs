using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PatrolState : State
{
   public NavigateState navigate;
   public IdleState idle;

   public float patrolRadiusInTiles = 5f;   // How far the enemy can wander from its starting point
   public Vector2 tileSize = new Vector2(1f,1f); // Tile dimentions
   public Vector2 patrolCenter;

   bool hasCenter = false;

   void GoToNextDestination()
   {
        if (!hasCenter)
        {
            patrolCenter = SnapToTile(core.transform.position);
            hasCenter = true;
        }

        // Choose a random tile offset within the patrol radius
        int offsetX = Random.Range(-Mathf.RoundToInt(patrolRadiusInTiles), Mathf.RoundToInt(patrolRadiusInTiles)+ 1);
        int offsetY = Random.Range(-Mathf.RoundToInt(patrolRadiusInTiles), Mathf.RoundToInt(patrolRadiusInTiles)+ 1);
        
        Vector2 nextTile = patrolCenter + new Vector2(offsetX * tileSize.x, offsetY * tileSize.y);

        // Snap to grid and send destination to navigate state
        navigate.destination = SnapToTile(nextTile);

        Set(navigate, true);
   }

   Vector2 SnapToTile(Vector2 worldPos)
   {
        float snappedX = Mathf.Round(worldPos.x / tileSize.x) * tileSize.x;
        float snappedY = Mathf.Round(worldPos.y / tileSize.y) * tileSize.y;
        return new Vector2(snappedX, snappedY);
   }

   public override void Enter()
   {
        GoToNextDestination();
   }

   public override void Do(){
    if (machine.state == navigate){
        if (navigate.isComplete)
        {
            Set(idle, true);
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    } else {
        if (machine.state.time > 1.5) 
        {
            GoToNextDestination();
        }
    }
   }

}
