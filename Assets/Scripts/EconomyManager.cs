using UnityEngine;
using TMPro; // TextMeshPro kullandığımız için

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance;

    [Header("Energy Settings")]
    public float maxEnergy = 10f;
    public float currentEnergy = 5f; 
    public float energyRegenRate = 0.4f; // Hızı düşürmüştük

    [Header("UI References")]
    public TMP_Text energyText; // Sadece Text kaldı

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (WaveManager.Instance != null && WaveManager.Instance.isGameStarted)
        {
            if (currentEnergy < maxEnergy)
            {
                currentEnergy += energyRegenRate * Time.deltaTime;
                UpdateUI();
            }
        }
    }

    void UpdateUI()
    {
        // Slider kodu silindi
        if (energyText != null)
        {
            // Sayıyı yuvarlayarak yaz (5.49 yerine 5 yazar)
            energyText.text = Mathf.FloorToInt(currentEnergy).ToString();
        }
    }

    public bool SpendEnergy(int amount)
    {
        if (currentEnergy >= amount)
        {
            currentEnergy -= amount;
            UpdateUI();
            return true; 
        }
        return false; 
    }




    public void GainEnergy(int amount)
    {
        currentEnergy += amount;
        
        // Enerji sınırını aşmasın
        if (currentEnergy > maxEnergy) currentEnergy = maxEnergy;
        
        UpdateUI();
    }



    
}