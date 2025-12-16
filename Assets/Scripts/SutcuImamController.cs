using UnityEngine;

public class SutcuImamController : MonoBehaviour
{
    [Header("Hero Stats")]
    public int damage = 100;      // Tek atış!
    public float fireRate = 2.5f; // Yavaş
    public float range = 6f;      // Uzun menzil
    // knockbackDist değişkeni silindi.

    private float nextFireTime = 0f;

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            FindAndShootEnemy();
        }
    }

    void FindAndShootEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearestEnemy = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && minDistance <= range)
        {
            Shoot(nearestEnemy);
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot(GameObject target)
    {
        EnemyAI enemyScript = target.GetComponent<EnemyAI>();
        if (enemyScript != null)
        {
            // Sadece hasar veriyoruz, itme kodu yok.
            enemyScript.TakeDamage(damage);
        }
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; // Kırmızı menzil (Hero)
        Gizmos.DrawWireSphere(transform.position, range);
    }
}