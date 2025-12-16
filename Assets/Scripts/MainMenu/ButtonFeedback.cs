using UnityEngine;
using UnityEngine.EventSystems; 
using DG.Tweening;

public class ButtonFeedback : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float pressScale = 0.9f; 
    public float duration = 0.1f;

    [Header("Audio")]
    public AudioClip clickSound; // Inspector'a tıklama sesini sürükle

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.DOScale(pressScale, duration).SetEase(Ease.OutQuad);

        // --- SES EKLEMESİ ---
        if(AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(clickSound);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.DOScale(1f, duration).SetEase(Ease.OutBack); 
    }
}