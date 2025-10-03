using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float HP = 50f;

    public void TakeDamage(float amount)
    {
        HP -= amount;
        if (HP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        SceneManager.LoadScene("DeathScene");
        Destroy(gameObject);
    }
}
