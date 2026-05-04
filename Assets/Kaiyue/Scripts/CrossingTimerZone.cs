using UnityEngine;

public class CrossingTimerZone : MonoBehaviour
{
    public CrossingTimer crossingTimer;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered timer zone: " + other.name + " tag: " + other.tag);

        bool isPlayer =
            other.CompareTag("Player") ||
            other.transform.root.CompareTag("Player") ||
            other.GetComponentInParent<CharacterController>() != null;

        if (!isPlayer)
        {
            return;
        }

        if (crossingTimer != null)
        {
            if (crossingTimer.IsLevelCompleted())
            {
                Debug.Log("Level already completed, timer will not start again.");
                return;
            }

            Debug.Log("Starting crossing timer from zone.");
            crossingTimer.StartTimer();
        }
        else
        {
            Debug.LogWarning("CrossingTimer is not assigned on CrossingTimerZone!");
        }
    }

    public void ResetZone()
    {
        // Keep this method so CarHazard and BananaPeelHazard won't break.
    }
}