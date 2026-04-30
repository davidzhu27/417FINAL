using UnityEngine;

public class CutsceneEnd : MonoBehaviour
{
    public Transform respawnPoint;
    public GameObject playerRig;

    public FallEffect fallEffect;

    public void ResetAfterDeath()
    {
        playerRig.transform.position = respawnPoint.position;

        fallEffect.ResetCamera(); // ✅ FIX

        CutsceneManager.Instance.EnablePlayer();
        ScreenFade.Instance.FadeIn(1f);
    }
}