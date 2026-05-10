using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class MohammadPlayerHUDController : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI progressText;
    public GameObject healthGroup;
    public TextMeshProUGUI healthText;

    [Header("Progress Settings")]
    public Transform playerTransform;
    public Transform goalTransform;
    private Vector3 startPosition;
    private float totalDistance;

    [Header("Health Settings")]
    public AudioSource heartbeatAudioSource;
    public int currentHealth = 100;

    private Coroutine progressCoroutine;
    private Coroutine healthCoroutine;

    private void Start()
    {
        UpdateHealthText();

        if (healthGroup != null)
            healthGroup.SetActive(false);

        // Record the player's starting position and calculate total distance to goal
        if (playerTransform != null && goalTransform != null)
        {
            startPosition = playerTransform.position;
            totalDistance = Vector3.Distance(startPosition, goalTransform.position);
        }
    }

    private void Update()
    {
        // Update the progress every frame
        UpdateProgressDistance();
    }

    private bool isLevelComplete = false; // The "Lock" variable

private void UpdateProgressDistance()
{
    // If we've already reached 100%, don't do any more math
    if (isLevelComplete || playerTransform == null || goalTransform == null || progressText == null) 
        return;

    float currentDist = Vector3.Distance(playerTransform.position, goalTransform.position);
    
    if (totalDistance <= 0.1f) return; 

    float progress = 1f - (currentDist / totalDistance);
    int progressPercent = Mathf.Clamp(Mathf.RoundToInt(progress * 100f * 1.15f), 0, 100);

    // Lock the progress if it hits 100
    if (progressPercent >= 100)
    {
        progressPercent = 100;
        isLevelComplete = true; // This prevents the code from running next frame
        
        // Optional: Add a little "Level Complete" flavor
    }

    progressText.text = $"Progress: {progressPercent}%";
}

    // --- Keep your existing ChangeHealth and Coroutine methods below ---
    
    public void ChangeHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, 100);
        UpdateHealthText();

        if (healthGroup != null)
        {
            healthGroup.SetActive(true);
            if (healthCoroutine != null) StopCoroutine(healthCoroutine);
            healthCoroutine = StartCoroutine(HideHealthAfterDelay(2f));
        }

        if (heartbeatAudioSource != null)
            heartbeatAudioSource.PlayOneShot(heartbeatAudioSource.clip);
    }

    private void UpdateHealthText()
    {
        if (healthText != null) healthText.text = currentHealth.ToString();
    }

    private IEnumerator HideHealthAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (healthGroup != null) healthGroup.SetActive(false);
        healthCoroutine = null;
    }
}