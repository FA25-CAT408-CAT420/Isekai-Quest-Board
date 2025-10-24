using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float HP = 50f;
    public float MP = 50f;

    void Update(){
        if (HP <= 0)
        {
            Die();
        }
    }
    /* public void TakeDamage(float amount)
     {
         HP -= amount;
        
    }*/

    private void Die()
    {
        SceneManager.LoadScene("DeathScene");
        Destroy(gameObject);
    }
}
