using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class EnemyCore : MonoBehaviour
{
    //reference to the rigidbody
    public Rigidbody2D rb;
    //reference to the player for all enemies
    public GameObject player;
    //Reference to Animator
    public Animator anim;
    // To manage enemy Health
    public int currentHealth;
    public int maxHealth;
    public PlayerHealth playerHealth;
    public GameObject soul;
    public GameObject SlimeSpit;
    public float damage = 5f;
    public Transform attackPoint;
    public float weaponRange;
    public LayerMask playerLayer;

    public StateMachine machine;

    public State state => machine.state;

    protected void Set(State newState, bool forceRest = false){
        machine.Set(newState, forceRest);
    }

    public void SetUpInstances() {
        machine = new StateMachine();


        State[] allChildrenStates = GetComponentsInChildren<State>();
        foreach (State state in allChildrenStates) {
            state.SetCore(this);
        }
    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;

        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        else if (currentHealth <= 0)
        {
            Instantiate(soul, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos() {
#if UNITY_EDITOR
        if (Application.isPlaying && state != null) {
            List<State> states = machine.GetActiveStateBranch();
            UnityEditor.Handles.Label(transform.position, "Active States: "+ string.Join(">", states));
        }
#endif
    }
}

