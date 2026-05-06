using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    public GameObject levelCompleteText;

    private bool isTransitioning = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // if (levelCompleteText != null)
        //     DontDestroyOnLoad(levelCompleteText.transform.root.gameObject);

        Debug.Log($"SceneTransitionManager ready. levelCompleteText = {(levelCompleteText != null ? levelCompleteText.name : "NULL")}");
    }

    public void TransitionToScene(string sceneName)
    {
        if (isTransitioning) return;

        StartCoroutine(Transition(sceneName));
    }

    IEnumerator Transition(string sceneName)
    {
        isTransitioning = true;

        Debug.Log("Transition STARTED");

        // 🟡 small pre-delay
        yield return new WaitForSeconds(0.5f);

        // 🔴 FADE TO BLACK
        if (ScreenFade.Instance != null)
            ScreenFade.Instance.FadeOut(2f);
        else
            Debug.LogWarning("ScreenFade instance missing during fade-out.");

        yield return new WaitForSeconds(2f);

        // ⚫ FORCE FULL BLACK (important safety step)
        if (ScreenFade.Instance != null)
            ScreenFade.Instance.FadeToColor(Color.black, 0.01f);
        yield return null;
        Debug.Log("Screen should now be fully BLACK");

        // 🟣 SHOW LEVEL COMPLETE ON BLACK SCREEN BEFORE LOAD
        Debug.Log("Attempting to show LEVEL COMPLETE text...");
        if (levelCompleteText != null)
        {
            levelCompleteText.SetActive(true);
            Debug.Log("LEVEL COMPLETE SHOWN ON BLACK SCREEN");
        }
        else
        {
            Debug.LogError("LevelCompleteText not assigned");
        }

        // ⏱ hold on black screen
        yield return new WaitForSeconds(1.5f);

        // 🟣 HIDE TEXT BEFORE SCENE LOAD
        if (levelCompleteText != null)
            levelCompleteText.SetActive(false);

        // 🔁 LOAD SCENE (now hidden behind black)
        SceneManager.LoadScene(sceneName);
        Debug.Log("Scene load triggered: " + sceneName);

        // ⏳ wait for scene to fully settle
        yield return new WaitForEndOfFrame();
        yield return null;

        // ⚫ ENSURE STILL BLACK AFTER LOAD
        if (ScreenFade.Instance != null)
            ScreenFade.Instance.FadeToColor(Color.black, 0.01f);
        else
            Debug.LogWarning("ScreenFade instance missing after scene load.");

        // 🌅 FADE INTO NEW SCENE VISUALLY
        if (ScreenFade.Instance != null)
            ScreenFade.Instance.FadeIn(1.5f);

        isTransitioning = false;
    }
}