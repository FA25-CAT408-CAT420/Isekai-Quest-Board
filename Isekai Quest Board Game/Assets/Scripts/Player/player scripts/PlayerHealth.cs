using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float HP = 50f;
    public float MP = 50f;

    public bool isInvulnerable = false;
    void Update(){
        if (HP <= 0)
        {
            Die();
        }
    }
    public void TakeDamage(float amount)
    {   
        if (isInvulnerable == false)
        {
            HP -= amount; 
        }
        else if (isInvulnerable == true){
            Debug.Log("I AM IMMORTAL");
        }
           
    }

    private void Die()
    {
        SceneManager.LoadScene("DeathScene");
        Destroy(gameObject);
    }
}
