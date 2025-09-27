using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroState : State
{
    [SerializeField] private float mSpeed;
    [SerializeField] private float minimumDistance;
    public GameObject player;
    public bool hasLineOfSight = false; 

    public override void Enter() {

        LookingForPlayer();
    

    }

    public override void Do() {
        PlayerIsFound();
    }

    public override void Exit() {

    }

    public void LookingForPlayer() {

        RaycastHit2D ray = Physics2D.Raycast(transform.position, player.transform.position - transform.position);

        if (ray.collider != null)
        {
            hasLineOfSight = ray.collider.CompareTag("Player");

            if(hasLineOfSight)
            {
                Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red);
            }
            else
            {
                Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.green);
            }
        }
    }

    public void PlayerIsFound() {
        if (Vector2.Distance(transform.position, player.transform.position) > minimumDistance && hasLineOfSight)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, mSpeed * Time.deltaTime);
        } 
    }
}
