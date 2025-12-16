using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource; // Arka plan müzikleri için
    public AudioSource sfxSource;   // Patlama, ateş etme, tıklama sesleri için

    [Header("Menu Clips")]
    public AudioClip menuMusic;     // Menü müziği
    public AudioClip explosion;
    public AudioClip placeToken;

    void Awake()
    {
        // Singleton Yapısı (Sahneler arası yok olmaz)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Sahne değişince silinmesin
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Oyun açılınca Menü Müziğini başlat
        PlayMusic(menuMusic);
    }

    // Müzik Çalma Fonksiyonu (Döngüsel)
    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;

        musicSource.clip = clip;
        musicSource.loop = true; // Müzik hep dönsün
        musicSource.Play();
    }

    // Efekt Çalma Fonksiyonu (Tek seferlik)
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        
        // PlayOneShot: Üst üste ses binebilir (örn: makineli tüfek)
        sfxSource.PlayOneShot(clip);
    }
    
    // Müziği durdurmak istersen
    public void StopMusic()
    {
        musicSource.Stop();
    }
}