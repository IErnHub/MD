using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Tıklamayı algılamak için bu kütüphane şart
using DG.Tweening; // DOTween

// Bu scripti buton olan objeye takmalısın
public class MapButtonAnim : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Animasyon Ayarları")]
    public float pressScale = 0.9f; // Basınca %10 küçülsün (0.9)
    public float duration = 0.1f;   // Animasyon süresi

    private Vector3 originalScale;

    void Start()
    {
        // Başlangıç boyutunu hafızaya al (Genelde 1,1,1'dir)
        originalScale = transform.localScale;
    }

    // 1. Mouse ile Üzerine Gelince (Hover) - Opsiyonel
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Hafifçe büyüsün (Seçildiği belli olsun)
        transform.DOScale(originalScale * 1.05f, duration);
    }

    // 2. Mouse Üzerinden Gidince
    public void OnPointerExit(PointerEventData eventData)
    {
        // Eski haline dönsün
        transform.DOScale(originalScale, duration);
    }

    // 3. Tıklayınca (Basılı Tutunca)
    public void OnPointerDown(PointerEventData eventData)
    {
        // İçeri doğru göçsün (Küçülme)
        transform.DOScale(originalScale * pressScale, duration).SetEase(Ease.OutQuad);
    }

    // 4. Tıklamayı Bırakınca
    public void OnPointerUp(PointerEventData eventData)
    {
        // Eski haline "Zıplayarak" dönsün (Ease.OutBack)
        transform.DOScale(originalScale * 1.05f, duration).SetEase(Ease.OutBack);
        // Not: 1.05f yaptık ki hover durumuna geri dönsün, yoksa mouse hala üstünde.
    }
}