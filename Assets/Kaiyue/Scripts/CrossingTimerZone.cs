using UnityEngine;

public class CrossingTimerZone : MonoBehaviour
{
    public CrossingTimer crossingTimer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            crossingTimer.StartTimer();
        }
    }
}