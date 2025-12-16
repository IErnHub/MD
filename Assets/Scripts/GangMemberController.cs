using UnityEngine;
using System.Collections; 

public class GangMemberController : MonoBehaviour
{
    [Header("Combat Stats")]
    public int damage = 34;
    public float fireRate = 1.5f; // Biraz yavaşlat ki burst ateşi belli olsun
    public float range = 3.5f;

    [Header("Visuals")]
    public Transform firePoint;       // Namlunun ucu
    public LineRenderer lineRenderer; // Sarı çizgi bileşeni

    private float nextFireTime = 0f;

    [Header("Burst Settings (Seri Atış Ayarı)")]
    public int shotCountPerFire = 3;  // Bir kere ateş edince kaç kez yanıp sönsün? (Trrrraak!)
    public float timeBetweenShots = 0.05f; // Yanıp sönme hızı

    public void ApplySpeedBuff()
    {
        fireRate = fireRate / 2f; 
    }

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            FindAndShoot();
        }
    }

    void FindAndShoot()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject e in enemies)
        {
            float d = Vector3.Distance(transform.position, e.transform.position);
            if (d < minDist) { minDist = d; nearest = e; }
        }

        if (nearest != null && minDist <= range)
        {
            // 1. BURST ATEŞİ BAŞLAT (Trrrraak!)
            StartCoroutine(BurstFireEffect(nearest));

            // Hasarı burada vermiyoruz, efektin içinde vereceğiz ki her "çizgi"de can gitsin
            // Veya tek seferde verebiliriz, ama efektle senkronize olması daha gerçekçi.
            
            nextFireTime = Time.time + fireRate;
        }
    }

    IEnumerator BurstFireEffect(GameObject target)
    {
        EnemyAI enemyScript = target.GetComponent<EnemyAI>();

        // Belirlediğimiz sayı kadar (örn: 3 kere) döngüye gir
        for (int i = 0; i < shotCountPerFire; i++)
        {
            // Hedef öldüyse döngüyü kır
            if (target == null) break;

            // A) Çizgiyi Aç
            if (lineRenderer != null)
            {
                lineRenderer.enabled = true;
                lineRenderer.SetPosition(0, firePoint.position);
                
                // Biraz sapma ekleyelim mi? (Hafif titrek ateş etsin)
                Vector3 randomOffset = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0);
                lineRenderer.SetPosition(1, target.transform.position + randomOffset);
            }

            // B) Çok kısa bekle (Görünme süresi)
            yield return new WaitForSeconds(0.03f);

            // C) Çizgiyi Kapat
            if (lineRenderer != null) lineRenderer.enabled = false;

            // D) Hasar Ver (Hasarı 3'e bölerek veriyoruz, toplam hasar aynı kalıyor)
            if (enemyScript != null)
            {
                // Toplam hasarı mermi sayısına böl (örn: 34 / 3 = 11)
                // Böylece can barı "tık tık tık" diye iner
                int damagePerShot = Mathf.CeilToInt((float)damage / shotCountPerFire);
                enemyScript.TakeDamage(damagePerShot);
            }

            // E) İki mermi arasındaki bekleme
            yield return new WaitForSeconds(timeBetweenShots);
        }
    }
}