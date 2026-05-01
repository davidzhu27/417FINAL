using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScreenFade : MonoBehaviour
{
    public static ScreenFade Instance;
    public Image fadeImage;

    private Coroutine currentFade;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 👇 listen for scene load
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 👇 automatically fade IN after new scene loads
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FadeIn(1.5f); // adjust duration as you like
    }

    // ===================== PRESET COLORS =====================

    public void FadeToRed()
    {
        FadeToColor(new Color(1f, 0.3f, 0f, 0.6f), 2.0f);
    }

    public void FadeToRedFast()
    {
        FadeToColor(new Color(1f, 0.3f, 0f, 0.6f), 0.2f);
    }

    public void FadeToGreen()
    {
        FadeToColor(new Color(0.2f, 1f, 0.2f, 0.5f), 2.0f);
    }

    // ===================== CORE FUNCTIONS =====================

    public void FadeToColor(Color color, float duration)
    {
        StartFade(color, duration);
    }

    public void FadeOut(float duration)
    {
        StartFade(Color.black, duration);
    }

    public void FadeIn(float duration)
    {
        StartFade(new Color(0, 0, 0, 0), duration);
    }

    void StartFade(Color targetColor, float duration)
    {
        if (currentFade != null)
            StopCoroutine(currentFade);

        currentFade = StartCoroutine(FadeRoutine(targetColor, duration));
    }

    IEnumerator FadeRoutine(Color targetColor, float duration)
    {
        Color startColor = fadeImage.color;
        float t = 0;

        while (t < duration)
        {
            t += Time.deltaTime;
            fadeImage.color = Color.Lerp(startColor, targetColor, t / duration);
            yield return null;
        }

        fadeImage.color = targetColor;
    }
}