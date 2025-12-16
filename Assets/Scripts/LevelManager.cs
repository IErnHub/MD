using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; 

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("UI Panels")]
    public GameObject mapPanel;       
    public GameObject battleUIPanel;  
    public GameObject endGamePanel;   
    
    [Header("Map & Progression")]
    public Button[] levelButtons;     
    public GameObject[] medalPieces;  
    public int unlockedLevel = 1;     
    public int currentLevel = 1;      

    [Header("Dynamic Visuals")]
    public Sprite[] groundSprites; 
    public Sprite[] baseSprites;   
    public SpriteRenderer sceneGroundRenderer; 
    public SpriteRenderer sceneBaseRenderer;   

    [Header("Card Buttons (Locking)")]
    public GameObject bombCardBtn;    
    public GameObject gruntCardBtn;   
    public GameObject senemCardBtn;   
    public GameObject heroCardBtn;    

    [Header("Scene Transition")]
    public Camera mainCamera;
    public CanvasGroup cloudPanel;    
    public Transform mapCamPos;       
    public Transform battleCamPos;    

    [Header("Animation Settings")]
    public float cloudFadeDuration = 1.5f; 

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ShowMap(); 
    }

   public void ShowMap()
    {
        Time.timeScale = 1; 
        mapPanel.SetActive(true);
        battleUIPanel.SetActive(false);
        endGamePanel.SetActive(false);
        
        // --- GÜNCELLENEN TEMİZLİK KISMI ---
        
        // 1. PlacementManager üzerinden tüm askerleri (kutuyu) sildiriyoruz
        if(PlacementManager.Instance != null) 
        {
            PlacementManager.Instance.ClearAllUnits();    // Eldeki hayaleti sil
            PlacementManager.Instance.DestroyAllSoldiers(); // Kutudaki askerleri sil
        }
        
        // 2. Düşmanları temizle
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject e in enemies) Destroy(e);

        // ------------------------------------

        if(mapCamPos != null) mainCamera.transform.position = mapCamPos.position;
        mainCamera.orthographicSize = 15; 

        UpdateMapButtons();
    }

    void UpdateMapButtons()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i < unlockedLevel) levelButtons[i].interactable = true;
            else levelButtons[i].interactable = false;
        }

        for (int i = 0; i < medalPieces.Length; i++)
        {
            if (i < unlockedLevel - 1) medalPieces[i].SetActive(true);
            else medalPieces[i].SetActive(false);
        }
    }

    public void SelectLevel(int levelIndex)
    {
        currentLevel = levelIndex;
        StartTransitionToBattle();
    }

    void StartTransitionToBattle()
    {
        mapPanel.SetActive(false);
        
        if (currentLevel - 1 < groundSprites.Length && groundSprites[currentLevel-1] != null)
            sceneGroundRenderer.sprite = groundSprites[currentLevel - 1];
            
        if (currentLevel - 1 < baseSprites.Length && baseSprites[currentLevel-1] != null)
            sceneBaseRenderer.sprite = baseSprites[currentLevel - 1];

        UnlockCardsForLevel(currentLevel);

        Sequence seq = DOTween.Sequence();
        
        if(cloudPanel != null)
        {
            cloudPanel.gameObject.SetActive(true);
            cloudPanel.alpha = 0;
            seq.Append(cloudPanel.DOFade(1, cloudFadeDuration));
        }

        seq.AppendCallback(() => {
            if(battleCamPos != null) mainCamera.transform.position = battleCamPos.position;
            mainCamera.orthographicSize = 8; 
            battleUIPanel.SetActive(true);
        });

        if(cloudPanel != null) 
        {
            seq.Append(cloudPanel.DOFade(0, cloudFadeDuration));
        }

        seq.OnComplete(() => {
            if(cloudPanel != null) cloudPanel.gameObject.SetActive(false);
            
            if (GameUIController.Instance != null) 
                GameUIController.Instance.InitializeForLevel(currentLevel);
            
            WaveManager.Instance.StartLevel(currentLevel); 
        });


        seq.OnComplete(() => {
            if(cloudPanel != null) cloudPanel.gameObject.SetActive(false);
            
            if (GameUIController.Instance != null) 
                GameUIController.Instance.InitializeForLevel(currentLevel);
            
            // --- YENİ EKLENEN KISIM: KALE CANINI YENİLE ---
            GameObject baseObj = GameObject.FindGameObjectWithTag("Base");
            if (baseObj != null)
            {
                // HealthSystem scriptini bul ve resetle
                baseObj.GetComponent<HealthSystem>().ResetHealth();
            }
            // ----------------------------------------------
            
            WaveManager.Instance.StartLevel(currentLevel); 
        });
    }

    void UnlockCardsForLevel(int level)
    {
        if(bombCardBtn) bombCardBtn.SetActive(true);
        if(gruntCardBtn) gruntCardBtn.SetActive(true);

        if(senemCardBtn)
        {
            if (level >= 2) senemCardBtn.SetActive(true);
            else senemCardBtn.SetActive(false);
        }

        if(heroCardBtn)
        {
            if (level >= 3) heroCardBtn.SetActive(true);
            else heroCardBtn.SetActive(false);
        }
    }

    public void LevelComplete()
    {
        if (currentLevel == unlockedLevel) unlockedLevel++;
        if(currentLevel - 1 < medalPieces.Length) medalPieces[currentLevel - 1].SetActive(true);
    }
}