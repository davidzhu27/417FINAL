using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFade : MonoBehaviour
{
    public static ScreenFade Instance;
    public Image fadeImage;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject); // 👈 add this
    }

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

    public void FadeToColor(Color color, float duration)
    {
        StartCoroutine(FadeRoutine(color, duration));
    }

    public void FadeOut(float duration)
    {
        FadeToColor(Color.black, duration);
    }

    public void FadeIn(float duration)
    {
        StartCoroutine(FadeRoutine(new Color(0,0,0,0), duration));
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
    }
}