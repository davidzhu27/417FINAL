using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    public string cutsceneName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Collision detected. You lost");
            CutsceneManager.Instance.PlayCutscene(cutsceneName);
        }
    }
}