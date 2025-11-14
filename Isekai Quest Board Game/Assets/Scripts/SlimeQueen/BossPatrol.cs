using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BossPatrol : State
{
    public BossNavigation bossNavigation;

public float patrolRadiusInTiles = 5f;   // How far the enemy can wander from its starting point
   public Vector2 tileSize = new Vector2(2f,2f); // Tile dimentions
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
        bossNavigation.destination = SnapToTile(nextTile);

        Set(bossNavigation, true);
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
    if (machine.state == bossNavigation){
        if (bossNavigation.isComplete)
        {
            GoToNextDestination();
        }
    } 
   }
}
