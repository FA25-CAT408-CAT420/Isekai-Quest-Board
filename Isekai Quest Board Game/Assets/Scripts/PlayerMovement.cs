using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    public GameManager gm;

    [Header("Targeting")]
    public List<GameObject> yourEnemiesInRange = new List<GameObject>();

    public GameObject targetedEnemy;
    int enemyIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        rb.velocity = new Vector3(moveHorizontal, moveVertical).normalized * speed;

        if (gm == null){
            gm = FindObjectOfType<GameManager>();
            Debug.Log("Game Manager Assigned.");
        }

        PurgeEnemies();
        
        if (yourEnemiesInRange.Count <= 0){
            targetedEnemy = null;
            enemyIndex = 0;
        }
        else {
            if (Input.GetKeyDown(KeyCode.RightBracket)) {
                enemyIndex = (enemyIndex + 1) % yourEnemiesInRange.Count;
            } 
            else if (Input.GetKeyDown(KeyCode.LeftBracket)) {
                enemyIndex--;
                if (enemyIndex < 0) {
                    enemyIndex = yourEnemiesInRange.Count - 1;
                }
            }
            targetedEnemy = yourEnemiesInRange[enemyIndex];
        }
    }

    public void AddYourEnemyToList(GameObject enemy)
    {
        if (!yourEnemiesInRange.Contains(enemy))
        {
            yourEnemiesInRange.Add(enemy);
            Debug.Log("Enemy added: " + enemy.name);
        }
    }

    public void RemoveYourEnemyFromList(GameObject enemy)
    {
        if (yourEnemiesInRange.Contains(enemy))
        {
            yourEnemiesInRange.Remove(enemy);
            Debug.Log("Enemy removed: " + enemy.name);
        }
    }

    public void PurgeEnemies()
    {
        yourEnemiesInRange.RemoveAll(item => item == null);
    }

    public void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.tag == "Soul"){
            gm.soulPoints++;
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Enemy")
        {
            AddYourEnemyToList(other.gameObject);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            RemoveYourEnemyFromList(other.gameObject);
        }
    }
}
