using UnityEngine;

public class SceneGoalTrigger : MonoBehaviour
{
    public string nextSceneName;
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;

            SceneTransitionManager.Instance.TransitionToScene(nextSceneName);
        }
    }
}