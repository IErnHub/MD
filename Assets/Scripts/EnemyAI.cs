using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    [Header("Stats (Özellikler)")]
    public float moveSpeed = 2f;      // Yürüme hızı
    public int damage = 25;           // Askerlere/Kaleye vuruş gücü
    public float attackRate = 1f;     // Saniyede kaç kez vursun?
    public int maxHealth = 150;       // Düşmanın canı

    [Header("Rotation Settings (Dönme Ayarı)")]
    // Sprite'ın çizim yönüne göre bunu değiştir. 
    public float rotationOffset = -90f; 

    private int currentHealth;
    private float nextAttackTime = 0f;

    [Header("Visuals (Görsellik)")]
    private SpriteRenderer spriteRenderer; 
    private Color originalColor;           

    [Header("State (Durum)")]
    private Transform targetBase;     // Hedef (Kale)
    private HealthSystem blockerUnit; // Önümü kesen engel (Asker veya Kale)
    
    // isKnockedBack değişkeni silindi.

    void Start()
    {
        currentHealth = maxHealth;

        // Flash efekti için rengi kaydet
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null) originalColor = spriteRenderer.color;

        // Kaleyi bul
        GameObject baseObj = GameObject.FindGameObjectWithTag("Base");
        if (baseObj != null) targetBase = baseObj.transform;
    }

    void Update()
    {
        // if (isKnockedBack) return; // Bu satır silindi.

        // 1. Önümüzde Engel Var mı?
        if (blockerUnit != null)
        {
            if (Time.time >= nextAttackTime)
            {
                AttackBlocker();
            }
            // Engel varsa Yürüme, Dur.
        }
        // 2. Önümüz Boşsa Yürü ve Dön
        else if (targetBase != null)
        {
            MoveAndRotate();
        }
    }

    // --- HAREKET VE DÖNME ---
    void MoveAndRotate()
    {
        // A) Yürü
        transform.position = Vector3.MoveTowards(transform.position, targetBase.position, moveSpeed * Time.deltaTime);

        // B) Hedefe Dön (Toz efekti için şart!)
        Vector3 direction = targetBase.position - transform.position;
        direction.Normalize();
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(Vector3.forward * (angle + rotationOffset));
    }

    // --- SALDIRI ---
    void AttackBlocker()
    {
        if (blockerUnit == null) return;

        blockerUnit.TakeDamage(damage);
        nextAttackTime = Time.time + attackRate;
    }

    // --- ÇARPIŞMA KONTROLÜ ---
    void OnTriggerEnter2D(Collider2D other)
    {
        // "Hero" (Asker) veya "Base" (Kale) ise dur ve saldır
        if (other.CompareTag("Hero") || other.CompareTag("Base"))
        {
            HealthSystem hp = other.GetComponent<HealthSystem>();
            if (hp != null)
            {
                blockerUnit = hp; 
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Önümdeki öldüyse veya gittiyse yürümeye devam et
        if (blockerUnit != null && other.gameObject == blockerUnit.gameObject)
        {
            blockerUnit = null;
        }
    }

    // --- HASAR ALMA ---
    public void TakeDamage(int dmg)
    {
        // Flash Efekti
        if (spriteRenderer != null) StartCoroutine(FlashEffect());

        currentHealth -= dmg;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator FlashEffect()
    {
        spriteRenderer.color = Color.white; // Parlama rengi
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
    }

    void Die()
    {
        // Skor ve Enerji Ödülü
        if (GameUIController.Instance != null) GameUIController.Instance.AddScore();
        if (EconomyManager.Instance != null) EconomyManager.Instance.GainEnergy(1);
            
        Destroy(gameObject);
    }
}