using UnityEngine;
using System.Linq; // Listeleme işlemleri için gerekli

public class SenemAyseSupport : MonoBehaviour
{
    public float buffRange = 4f;
    public int maxTargets = 2; // En fazla kaç kişiyi bufflasın?

    void Update()
    {
        // 1. Etraftaki her şeyi tara
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, buffRange);

        // 2. Sadece "GangMemberController" scripti olanları ayıkla (Kendisi hariç)
        var allies = hits
            .Select(h => h.GetComponent<GangMemberController>())
            .Where(unit => unit != null && unit.gameObject != gameObject)
            .ToList();

        // 3. Mesafeye göre sırala (En yakındakiler başa gelsin)
        allies.Sort((a, b) => 
            Vector3.Distance(transform.position, a.transform.position)
            .CompareTo(Vector3.Distance(transform.position, b.transform.position))
        );

        // 4. İlk 2 kişiye Buff uygula
        int count = 0;
        foreach (var ally in allies)
        {
            if (count < maxTargets)
            {
                ally.ApplySpeedBuff();
                // Görsel olarak kime bağlandığını çizelim (Sadece Scene ekranında görünür)
                Debug.DrawLine(transform.position, ally.transform.position, Color.green);
                count++;
            }
            else
            {
                break; 
            }
        }
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, buffRange);
    }
}