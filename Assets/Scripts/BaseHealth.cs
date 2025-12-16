using UnityEngine;

public class BaseHealth : MonoBehaviour
{
    public int health = 10;

    public void TakeDamage(int damage)
    {
        health -= damage;
        
        // --- YENİ EKLENEN KISIM ---
        // UI'daki can yazısını güncelle
        if (GameUIController.Instance != null)
        {
            GameUIController.Instance.UpdateHealthUI(health);
        }

        if (health <= 0)
        {
            // Kaybettin haberini ver
            GameUIController.Instance.GameOver(false);
        }
        // --------------------------
    }
}