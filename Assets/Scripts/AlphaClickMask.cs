using UnityEngine;
using UnityEngine.UI; // UI kütüphanesi şart

public class AlphaClickMask : MonoBehaviour
{
    // Ne kadar şeffaf olunca tıklamayı yoksaysın?
    // 0.1f = %10'dan az görünüyorsa (neredeyse şeffafsa) tıklama.
    public float threshold = 0.1f; 

    void Start()
    {
        // Bu objedeki Image bileşenini al
        Image image = GetComponent<Image>();

        if (image != null)
        {
            // İŞTE SİHİRLİ KOD:
            // Alpha (Şeffaflık) değerine göre vuruş testini ayarla.
            image.alphaHitTestMinimumThreshold = threshold;
        }
    }
}