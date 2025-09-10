using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float minimumDistance;
    private GameObject player;
    private bool hasLineOfSight = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");   //Finds the player through the tag
    }

    // Update is called once per frame
    void Update()
    {
        //If the player is too far away from the minimum distance than the enemy will move towards player and has line of sight
        if (Vector2.Distance(transform.position, player.transform.position) > minimumDistance && hasLineOfSight)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        } else
        {

        }
    }

    private void FixedUpdate()
    {
        //Creates a Ray Cast towards the Player so the enemy has a "Line of sight" 
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
}
