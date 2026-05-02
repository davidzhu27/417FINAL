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
        ScreenFade.Instance.FadeOut(2f);
        yield return new WaitForSeconds(2f);

        // ⚫ FORCE FULL BLACK (important safety step)
        ScreenFade.Instance.FadeToColor(Color.black, 0.01f);
        yield return null;

        // 🔁 LOAD SCENE (now hidden behind black)
        SceneManager.LoadScene(sceneName);

        // ⏳ wait for scene to fully settle
        yield return new WaitForEndOfFrame();
        yield return null;

        // ⚫ ENSURE STILL BLACK AFTER LOAD
        ScreenFade.Instance.FadeToColor(Color.black, 0.01f);

        // 🟣 SHOW LEVEL COMPLETE ON BLACK SCREEN
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

        // 🟣 HIDE TEXT
        if (levelCompleteText != null)
            levelCompleteText.SetActive(false);

        // 🌅 FADE INTO NEW SCENE VISUALLY
        ScreenFade.Instance.FadeIn(1.5f);

        isTransitioning = false;
    }
}