using UnityEngine;

public class HazardCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hazard hit by " + other.gameObject.name);
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit hazard");
        }
    }
}