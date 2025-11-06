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
    [SerializeField]public float Health;

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

    private void OnDrawGizmos() {
#if UNITY_EDITOR
        if (Application.isPlaying && state != null) {
            List<State> states = machine.GetActiveStateBranch();
            UnityEditor.Handles.Label(transform.position, "Active States: "+ string.Join(">", states));
        }
#endif
    }
}

