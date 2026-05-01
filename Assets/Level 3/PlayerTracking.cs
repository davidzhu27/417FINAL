using UnityEngine;
using System.Collections.Generic;
public class PlayerTracking : MonoBehaviour
{
    [SerializeField] Transform cameraTransform;
    [SerializeField] float max_turn_amount = 120.0f;
    [SerializeField] Teacher teacher;
    private Quaternion prev_rotation;
    private float total_degs_turned = 0.0f;
    private float time_window_size = 1.0f;
    private struct AngleRotated {
        public float time_measured;
        public float angle_rotated;
    }
    private Queue<AngleRotated> angles = new Queue<AngleRotated>();
    private bool triggered = false;
    private Vector3 startPosition;
    private Quaternion initRotation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        prev_rotation = cameraTransform.rotation;
        startPosition = cameraTransform.position;
        initRotation = cameraTransform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        float angle_change = Quaternion.Angle(prev_rotation, cameraTransform.rotation);
        prev_rotation = cameraTransform.rotation;

        total_degs_turned += angle_change;
        float current_time = Time.time;
        while (angles.Count > 0 && ((current_time - angles.Peek().time_measured) > time_window_size)) total_degs_turned -= angles.Dequeue().angle_rotated;
        angles.Enqueue(new AngleRotated {time_measured = current_time, angle_rotated = angle_change});
        if (!triggered && total_degs_turned >= max_turn_amount) KillPlayer();
    }
    void KillPlayer() {
        triggered = true;
        teacher.SetExecuteStudent();
    }
    void ResetPlayer() {
        cameraTransform.position = startPosition;
        cameraTransform.rotation = initRotation;
    }
}
