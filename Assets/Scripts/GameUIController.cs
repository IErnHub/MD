using UnityEngine;
using UnityEngine.UI; // Slider ve Button için
using TMPro; // TextMeshPro için
using UnityEngine.SceneManagement;

public class GameUIController : MonoBehaviour
{
    public static GameUIController Instance;

    [Header("UI References")]
    public TMP_Text healthText;     // Sol üstteki Can yazısı
    public Slider medalSlider;      // Üstteki Sarı Bar
    public GameObject endGamePanel; // Bitiş Paneli
    public TMP_Text resultText;     // Kazandın/Kaybettin yazısı

    [Header("Game Settings")]
    public int targetScore = 30;    // Varsayılan, ama kodla değişecek
    private int currentScore = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if(endGamePanel != null) endGamePanel.SetActive(false);
        UpdateHealthUI(100); // Başlangıçta temsili bir değer
    }

    // --- YENİ EKLENEN FONKSİYON ---
    // LevelManager tarafından oyun başlarken çağrılır
    public void InitializeForLevel(int level)
    {
        // Hedef Skoru Belirle: 20 + (Level * 10)
        // Lvl 1: 30
        // Lvl 2: 40
        // Lvl 3: 50
        targetScore = 20 + (level * 10);
        
        // Mevcut skoru sıfırla
        currentScore = 0;

        // Slider'ı ayarla
        if (medalSlider != null)
        {
            medalSlider.maxValue = targetScore;
            medalSlider.value = 0;
        }

        Debug.Log($"Level {level} Başladı. Hedef Skor: {targetScore}");
    }
    // -----------------------------

    public void AddScore()
    {
        currentScore++;
        
        if(medalSlider != null)
            medalSlider.value = currentScore;

        // Hedefe ulaştık mı?
        if (currentScore >= targetScore)
        {
            GameOver(true); // KAZANDIN
        }
    }

    public void UpdateHealthUI(int currentHealth)
    {
        if(healthText != null)
            healthText.text = "Health: " + currentHealth;
    }

    public void GameOver(bool isVictory)
    {
        // Savaşı durdur
        WaveManager.Instance.isGameStarted = false; 
        
        // Oyunu dondur (Kimse hareket edemesin)
        Time.timeScale = 0; 
        
        if(endGamePanel != null) endGamePanel.SetActive(true);

        if(resultText != null)
        {
            if (isVictory)
            {
                resultText.text = "BÖLGE KURTARILDI!";
                
                LevelManager.Instance.LevelComplete();
            }
            else
            {
                resultText.text = "ŞEHİR DÜŞTÜ...";
                
            }
        }
    }

    // "Haritaya Dön" butonu buna bağlı olmalı
    public void BackToMap()
    {
        // Zamanı tekrar akıt (Yoksa harita animasyonları çalışmaz)
        Time.timeScale = 1;
        
        endGamePanel.SetActive(false);
        
        // Harita ekranına geç
        LevelManager.Instance.ShowMap();
    }
}