using UnityEngine;

public class FoodPoisonEffect : MonoBehaviour
{
    public Transform cameraTransform;
    private bool active;

    public void StartNausea()
    {
        active = true;
    }

    public void StopNausea()
    {
        active = false;
        cameraTransform.localRotation = Quaternion.identity;
    }

    void Update()
    {
        if (!active) return;

        float x = Mathf.Sin(Time.time * 2f) * 1.5f;
        float y = Mathf.Sin(Time.time * 1.3f) * 1.5f;

        cameraTransform.localRotation = Quaternion.Euler(x, y, 0);
    }
}