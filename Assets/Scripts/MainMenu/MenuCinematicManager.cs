using UnityEngine;
using UnityEngine.SceneManagement; 
using DG.Tweening; 
using UnityEngine.UI; 

public class MenuCinematicManager : MonoBehaviour
{
    [Header("Referanslar")]
    public Camera mainCamera;       
    public Transform zoomTarget;    
    public CanvasGroup menuUI;      
    public CanvasGroup blackScreen; 
    public string gameSceneName = "GameScene"; 

    [Header("Animasyon Ayarları")]
    public float riseAmount = 1.5f;     
    public float tiltAmount = 15f;      
    public float prepDuration = 0.5f;   
    public float diveDuration = 1.2f;   

    [Header("Audio")]
    public AudioClip startGameSound; // Oyuna giriş sesi (Whoosh/Start)

    public void StartGameSequence()
    {
        // --- SES EKLEMESİ ---
        // Menü müziğini biraz kısabilirsin veya efekt çalabilirsin
        if(AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(startGameSound);
        }
        // --------------------

        if (menuUI != null)
        {
            menuUI.interactable = false;    
            menuUI.blocksRaycasts = false;  
            menuUI.DOFade(0f, 0.3f);        
        }

        Sequence seq = DOTween.Sequence();

        Vector3 risePos = mainCamera.transform.position + (Vector3.up * riseAmount);
        Vector3 tiltRot = mainCamera.transform.eulerAngles + new Vector3(tiltAmount, 0, 0);

        seq.Append(mainCamera.transform.DOMove(risePos, prepDuration).SetEase(Ease.OutQuad));
        seq.Join(mainCamera.transform.DORotate(tiltRot, prepDuration).SetEase(Ease.OutQuad));

        seq.Append(mainCamera.transform.DOMove(zoomTarget.position, diveDuration).SetEase(Ease.InCubic));
        seq.Join(mainCamera.transform.DORotate(zoomTarget.rotation.eulerAngles, diveDuration).SetEase(Ease.InCubic));

        if (blackScreen != null)
        {
            float fadeStartTime = prepDuration + (diveDuration * 0.5f); 
            seq.Insert(fadeStartTime, blackScreen.DOFade(1f, diveDuration * 0.4f));
        }

        seq.OnComplete(() => {
            SceneManager.LoadScene(gameSceneName);
        });
    }

    public void QuitGame()
    {
        Debug.Log("Oyundan Çıkılıyor..."); 
        Application.Quit();
    }
}