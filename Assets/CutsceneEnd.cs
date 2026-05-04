using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneEnd : MonoBehaviour
{
    public Transform respawnPoint;
    public GameObject playerRig;
    [Header("References")]
    public AttemptManager attemptManager;


    public FallEffect fallEffect;

    public void ResetAfterDeath()
    {
        // playerRig.transform.position = respawnPoint.position;
        // if (attemptManager != null)
        // {
        //     attemptManager.AddAttempt();
        // }
        

        // // fallEffect.ResetCamera(); // ✅ FIX

        // // CutsceneManager.Instance.EnablePlayer();
        // // ScreenFade.Instance.FadeIn(1f);
        Time.timeScale = 1f;
        if (attemptManager != null)
        {
            attemptManager.AddAttempt();
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}