using UnityEngine;
using UnityEngine.XR;

public class BurnEffect : MonoBehaviour
{
    public AudioSource fireAudio;

    public void IncreaseIntensity()
    {
        StartCoroutine(Intensity());
    }

    public void TriggerHaptics()
    {
        var device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        device.SendHapticImpulse(0, 0.7f, 1f);
    }
    
    System.Collections.IEnumerator Intensity()
    {
        float t = 0;
        float startVol = fireAudio.volume;

        while (t < 2f)
        {
            t += Time.deltaTime;
            fireAudio.volume = Mathf.Lerp(startVol, 3f, 3f * t / 2f);
            yield return null;
        }
    }
    
}