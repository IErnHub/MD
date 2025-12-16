using UnityEngine;

public class BombTrap : MonoBehaviour
{
    public int damage = 500;
    public float radius = 2.5f;

    [Header("Visuals")]
    public GameObject explosionEffect; // Inspector'a atacağın efekt buraya gelecek

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Explode();
        }
    }

    void Explode()
    {
        // 1. Önce Efekti Patlat (Eğer atadıysan)
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // 2. Kamerayı Salla (Tek bir kere salla)
        if (CameraShaker.Instance != null) 
        {
            CameraShaker.Instance.Shake();
        }

        if(AudioManager.Instance != null) 
            AudioManager.Instance.PlaySFX(AudioManager.Instance.explosion);
        // 3. Hasar Verme İşlemi
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D hit in enemies)
        {
            if (hit.CompareTag("Enemy"))
            {
                EnemyAI enemy = hit.GetComponent<EnemyAI>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);    
                }
            }
        }

        // 4. Bombayı Yok Et
        Destroy(gameObject); 
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}