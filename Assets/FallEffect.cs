using UnityEngine;

public class FallEffect : MonoBehaviour
{
    public Transform cameraTransform;
    private Quaternion originalRotation;

    void Start()
    {
        originalRotation = cameraTransform.localRotation;
    }

    public void StartFall()
    {
        StopAllCoroutines();
        StartCoroutine(FallTilt());
    }

    public void ResetCamera()
    {
        StopAllCoroutines();
        cameraTransform.localRotation = originalRotation;
    }

    System.Collections.IEnumerator FallTilt()
    {
        float t = 0;
        while (t < 1.5f)
        {
            t += Time.deltaTime;
            float angle = Mathf.Lerp(0, 20f, t / 1.5f);
            cameraTransform.localRotation = Quaternion.Euler(angle, 0, 0);
            yield return null;
        }
    }
}