using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class EnemyCore : MonoBehaviour
{
    //reference to the rigidbody
    public Rigidbody2D rb;
    //reference to the player for all enemies
    private GameObject player;
    // To manage enemy Health
    [SerializeField]public float Health;
    //Every enemy will be targetable
    public bool isTargeted;

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
}

