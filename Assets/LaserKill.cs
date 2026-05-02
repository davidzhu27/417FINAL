using UnityEngine;

public class LaserKill : MonoBehaviour
{
    public Transform leftBeam;
    public Transform rightBeam;

    public Transform leftOrigin;
    public Transform rightOrigin;

    public Transform playerCam;

    private bool active = false;

    void Start()
    {
        leftBeam.gameObject.SetActive(false);
        rightBeam.gameObject.SetActive(false);
    }

    public void StartLaser()
    {
        active = true;
        leftBeam.gameObject.SetActive(true);
        rightBeam.gameObject.SetActive(true);
    }

    public void StopLaser()
    {
        active = false;
        leftBeam.gameObject.SetActive(false);
        rightBeam.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!active) return;

        UpdateBeam(leftBeam, leftOrigin);
        UpdateBeam(rightBeam, rightOrigin);
    }

    void UpdateBeam(Transform beam, Transform origin)
    {
        if (playerCam == null || origin == null) return;

        Vector3 target = playerCam.position;
        target.y -= 0.1f;
   
        Vector3 direction = target - origin.position;

        float distance = direction.magnitude;

        if (distance < 0.05f)
            distance = 0.05f;

        // 🔴 POSITION: midpoint between origin and target
        beam.position = origin.position + direction / 2f;

        // 🔴 ROTATION: align beam's Y axis to direction
        beam.up = direction.normalized;

        // 🔴 SCALE: Y = length, X/Z fixed thickness
        beam.localScale = new Vector3(0.01f, distance / 2f, 0.01f);
    }
}