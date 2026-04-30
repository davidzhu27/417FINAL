using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public void TransitionToScene(string sceneName)
    {
        StartCoroutine(Transition(sceneName));
    }

    IEnumerator Transition(string sceneName)
    {
        // 🟡 0. SMALL DELAY BEFORE ANYTHING
        yield return new WaitForSeconds(0.5f); // 👈 adjust (0.3–1.0 feels good)

        // 1. Fade out
        ScreenFade.Instance.FadeOut(2f);

        // 2. Wait for fade to finish
        yield return new WaitForSeconds(2f);

        // 3. Load scene
        SceneManager.LoadScene(sceneName);

        // 4. Wait one frame (ensure scene loads)
        yield return null;

        // 5. Fade in
        ScreenFade.Instance.FadeIn(2f);
    }
}