using UnityEngine;
using DG.Tweening; // DOTween kütüphanesi

public class CameraShaker : MonoBehaviour
{
    public static CameraShaker Instance;

    // Sarsıntı ayarları
    public float duration = 0.5f;  // Ne kadar sürsün?
    public float strength = 0.5f;  // Ne kadar şiddetli?
    public int vibrato = 10;       // Titreşim sıklığı

    void Awake()
    {
        Instance = this;
    }

    public void Shake()
    {
        // Eğer zaten sallanıyorsa üst üste binmesin, önce durdurup (Complete) sonra sallasın
        transform.DOComplete(); 
        
        // DOTween büyüsü: Pozisyonu salla
        transform.DOShakePosition(duration, strength, vibrato, 90, false, true);
    }
}