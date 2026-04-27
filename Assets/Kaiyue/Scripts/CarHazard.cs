using UnityEngine;

public class CarHazard : MonoBehaviour
{
    public Transform playerStartPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player was hit by car!");

            if (playerStartPoint != null)
            {
                other.transform.position = playerStartPoint.position;
            }
        }
    }
}