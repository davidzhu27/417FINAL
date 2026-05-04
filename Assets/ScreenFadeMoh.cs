using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScreenFadeMoh : MonoBehaviour
{
    public static ScreenFadeMoh Instance;

    [Header("Assign Quad Renderer")]
    public Renderer fadeRenderer;

    private Material fadeMat;
    private Coroutine currentFade;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        SetupMaterial();
        SetFade(0f); // start invisible
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetupMaterial();
        SetFade(0f);
    }

    // ================= CORE =================

    void SetupMaterial()
    {
        if (fadeRenderer == null) return;

        fadeMat = Instantiate(fadeRenderer.material);
        fadeRenderer.material = fadeMat;
    }

    public void FadeOut(float duration)
    {
        StartFade(1f, duration);
    }

    public void FadeIn(float duration)
    {
        StartFade(0f, duration);
    }

    public IEnumerator FadeOutCoroutine(float duration)
    {
        yield return FadeRoutine(1f, duration);
    }

    void StartFade(float target, float duration)
    {
        if (currentFade != null)
            StopCoroutine(currentFade);

        currentFade = StartCoroutine(FadeRoutine(target, duration));
    }

    IEnumerator FadeRoutine(float target, float duration)
    {
        float start = GetFade();
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float f = Mathf.Lerp(start, target, t / duration);
            SetFade(f);
            yield return null;
        }

        SetFade(target);
    }

    void SetFade(float value)
    {
        if (fadeMat != null)
            fadeMat.SetFloat("_Fade", value);
    }

    float GetFade()
    {
        return fadeMat != null ? fadeMat.GetFloat("_Fade") : 0f;
    }
}