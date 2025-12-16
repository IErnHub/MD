using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitCard : MonoBehaviour
{
    public GameObject unitPrefab; // Bu kart hangi askeri çıkarır?
    public int cost = 3;          // Fiyatı ne?
    
    public TMP_Text costText;
    public Button button;

    void Start()
    {
        if(costText != null) costText.text = cost.ToString();
        
        button.onClick.AddListener(OnCardClicked);
    }

    void Update()
    {
        // Paramız yetmiyorsa buton sönük dursun
        if (EconomyManager.Instance.currentEnergy < cost)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    }

    void OnCardClicked()
    {
        PlacementManager.Instance.SelectUnitToPlace(unitPrefab, cost);
    }
}