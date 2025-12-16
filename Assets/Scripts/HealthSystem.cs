using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [Header("Settings")]
    public int maxHealth = 100; 
    public int currentHealth;

    [Header("Tags")]
    public bool isBase = false; 

    void Start()
    {
        // Oyun ilk açıldığında çalışır
        ResetHealth();
    }

    // --- YENİ EKLENEN FONKSİYON ---
    // Bunu dışarıdan (LevelManager'dan) çağıracağız
    public void ResetHealth()
    {
        currentHealth = maxHealth; // Canı tepeye çek

        // Eğer bu Base ise UI'ı da güncelle
        if (isBase && GameUIController.Instance != null)
        {
            GameUIController.Instance.UpdateHealthUI(currentHealth);
        }
    }
    // ------------------------------

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (isBase && GameUIController.Instance != null)
        {
            GameUIController.Instance.UpdateHealthUI(currentHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isBase)
        {
            if (GameUIController.Instance != null) 
                GameUIController.Instance.GameOver(false); 
        }
        else
        {
            Destroy(gameObject);
        }
    }
}