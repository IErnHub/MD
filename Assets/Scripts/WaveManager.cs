using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    [Header("Settings")]
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;
    public float spawnRate = 2f;
    private float nextSpawnTime = 0f;

    public bool isGameStarted = false;

    void Awake() 
    { 
        Instance = this; 
    }

    public void StartLevel(int level)
    {
        // Zorluk ayarı: Level arttıkça spawnRate düşer (daha hızlı gelirler)
        spawnRate = 2.5f - (level * 0.5f); 
        isGameStarted = true;
    }

    void Update()
    {
        if (!isGameStarted) return;

        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

   void SpawnEnemy()
    {
        if (spawnPoints.Length == 0) return;
        
        int randomPoint = Random.Range(0, spawnPoints.Length);
        
        // YENİ: Rastgele bir düşman tipi seç
        int randomEnemy = Random.Range(0, enemyPrefabs.Length);
        
        Instantiate(enemyPrefabs[randomEnemy], spawnPoints[randomPoint].position, Quaternion.identity);
    }
}