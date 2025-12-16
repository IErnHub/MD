using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public static PlacementManager Instance;
    
    [Header("Ayarlar")]
    public Camera mainCamera;
    
    // --- YENİ: Askerlerin toplanacağı kutu ---
    public Transform unitsContainer; 
    
    [Header("Efektler")]
    // --- YENİ: İnspectordan sürükleyeceğin efekt prefabı ---
    public GameObject placementVFX; 
    // ------------------------------------------------------

    private GameObject currentGhost; // Mouse ucundaki hayalet asker
    private int currentCost;         // Şuan yerleştirmeye çalıştığımız askerin fiyatı

    void Awake()
    {
        Instance = this;

        // Kutu yoksa otomatik oluştur
        if (unitsContainer == null)
        {
            GameObject go = new GameObject("UnitsContainer");
            unitsContainer = go.transform;
        }
    }

    void Update()
    {
        if (currentGhost != null)
        {
            Vector3 mousePos = GetMouseWorldPosition();
            currentGhost.transform.position = mousePos;

            if (Input.GetMouseButtonDown(0))
            {
                // Son bir kontrol: Paramız hala yetiyor mu?
                if (EconomyManager.Instance != null && EconomyManager.Instance.currentEnergy >= currentCost)
                {
                    PlaceUnit(mousePos);
                }
                else
                {
                    ClearAllUnits();
                }
            }
        }
    }

    public void SelectUnitToPlace(GameObject unitPrefab, int cost)
    {
        if(currentGhost != null) Destroy(currentGhost);
        
        currentCost = cost;
        currentGhost = Instantiate(unitPrefab);
        
        // Yaratılan askeri kutunun içine koy
        if (unitsContainer != null)
        {
            currentGhost.transform.SetParent(unitsContainer);
        }
        
        // Collider'ı kapat
        Collider2D col = currentGhost.GetComponent<Collider2D>();
        if(col != null) col.enabled = false; 
    }

    void PlaceUnit(Vector3 position)
    {
        // 1. Parayı Harca
        if(EconomyManager.Instance != null)
        {
            EconomyManager.Instance.SpendEnergy(currentCost);
        }

        // --- YENİ: EFEKTİ OYNAT ---
        if (placementVFX != null)
        {
            // Efekti tam karakterin olduğu yerde oluştur
            GameObject vfx = Instantiate(placementVFX, position, Quaternion.identity);
            
            // Ortalığı kirletmesin diye 2 saniye sonra otomatik silinsin
            Destroy(vfx, 2.0f); 
        }
        // ---------------------------

        // 2. Collider'ı geri aç
        Collider2D col = currentGhost.GetComponent<Collider2D>();
        if(col != null) col.enabled = true;

        // 3. Asker scriptini aktif et
        GangMemberController controller = currentGhost.GetComponent<GangMemberController>();
        if(controller != null) controller.enabled = true;

        currentGhost = null; // Elimizi boşalt
    }

    public void DestroyAllSoldiers()
    {
        if(currentGhost != null) Destroy(currentGhost);
        currentGhost = null;

        foreach(Transform child in unitsContainer)
        {
            Destroy(child.gameObject);
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Plane plane = new Plane(Vector3.forward, Vector3.zero); 
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        float distance;
        if (plane.Raycast(ray, out distance))
        {
            return ray.GetPoint(distance);
        }
        return Vector3.zero;
    }

    public void ClearAllUnits()
    {
        if(currentGhost != null) Destroy(currentGhost);
        currentGhost = null;
    }
}