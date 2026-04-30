using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager Instance;

    public PlayableDirector[] cutscenes;
    public GameObject playerRig; // XR Origin
    public MonoBehaviour moveProvider;
    public MonoBehaviour turnProvider;
    public MonoBehaviour teleportProvider;
    public MonoBehaviour jumpProvider;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayCutscene(string cutsceneName)
    {
        DisablePlayer();

        foreach (var cs in cutscenes)
        {
            if (cs.name == cutsceneName)
            {
                cs.Play();
                return;
            }
        }

        Debug.LogWarning("Cutscene not found: " + cutsceneName);
    }

    void DisablePlayer()
    {
        moveProvider.enabled = false;
        turnProvider.enabled = false;
        teleportProvider.enabled = false;
        jumpProvider.enabled = false;
    }

    public void EnablePlayer()
    {
        moveProvider.enabled = true;
        turnProvider.enabled = true;
        teleportProvider.enabled = true;
        jumpProvider.enabled = true;
    }
}