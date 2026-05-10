using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHUDController : MonoBehaviour
{
    public TextMeshProUGUI progressText;

    public GameObject healthGroup;
    public TextMeshProUGUI healthText;

    public AudioSource heartbeatAudioSource;

    public int currentHealth = 100;
    public int totalPlayableLevels = 4;

    private Coroutine progressCoroutine;
    private Coroutine healthCoroutine;

    private void Start()
    {
        UpdateHealthText();

        if (healthGroup != null)
            healthGroup.SetActive(false);

        ShowProgressAtStart();
    }

    public void ShowProgressAtStart()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int progressPercent = 0;

        if (totalPlayableLevels > 0)
        progressPercent = Mathf.RoundToInt((currentIndex / (float)totalPlayableLevels) * 100f);

        if (progressText != null)
        {
            progressText.text = "Progress: " + progressPercent + "%";
            progressText.gameObject.SetActive(true);

            if (progressCoroutine != null)
                StopCoroutine(progressCoroutine);

            progressCoroutine = StartCoroutine(HideProgressAfterDelay(3f));
        }
    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, 100);

        UpdateHealthText();

        if (healthGroup != null)
        {
            healthGroup.SetActive(true);

            if (healthCoroutine != null)
                StopCoroutine(healthCoroutine);

            healthCoroutine = StartCoroutine(HideHealthAfterDelay(2f));
        }

        if (heartbeatAudioSource != null)
            heartbeatAudioSource.Play();
    }

    private void UpdateHealthText()
    {
        if (healthText != null)
            healthText.text = currentHealth.ToString();
    }

    private IEnumerator HideProgressAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (progressText != null)
            progressText.gameObject.SetActive(false);

        progressCoroutine = null;
    }

    private IEnumerator HideHealthAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (healthGroup != null)
            healthGroup.SetActive(false);

        healthCoroutine = null;
    }
}