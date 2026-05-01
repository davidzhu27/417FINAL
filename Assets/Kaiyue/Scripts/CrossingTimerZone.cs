using UnityEngine;

public class CrossingTimerZone : MonoBehaviour
{
    public CrossingTimer crossingTimer;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered timer zone: " + other.name + " tag: " + other.tag);

        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (crossingTimer != null)
        {
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
        // No lock now, so nothing is needed here.
        // Keep this method so CarHazard and BananaPeelHazard won't break.
    }
}