using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    public string cutsceneName;
    public MohammadPlayerHUDController controller;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (controller != null)
                controller.ChangeHealth(-999);
            Debug.Log("Collision detected. You lost");
            CutsceneManager.Instance.PlayCutscene(cutsceneName);
        }
    }
}