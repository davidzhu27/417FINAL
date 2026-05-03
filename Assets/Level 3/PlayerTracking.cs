using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class PlayerTracking : MonoBehaviour
{
    [SerializeField] Transform cameraTransform;
    [SerializeField] float max_turn_amount = 120.0f;
    [SerializeField] Teacher teacher;
    [SerializeField] TextMeshProUGUI[] texts;
    private Quaternion prev_rotation;
    private float total_degs_turned = 0.0f;
    private float time_window_size = 1.0f;
    private struct AngleRotated {
        public float time_measured;
        public float angle_rotated;
    }
    private Queue<AngleRotated> angles = new Queue<AngleRotated>();
    private bool triggered = false;
    private bool player_won = false;
    private Vector3 startPosition;
    private Quaternion initRotation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        prev_rotation = cameraTransform.rotation;
        startPosition = new Vector3(-8.0f, 0.0f, 22.0f);
        initRotation = cameraTransform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (player_won) return;
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
        for (int i= 0; i < texts.Length; i++) {
            texts[i].text = "You have been accused of cheating for looking around too much. Punishment will be delivered";
        }
        teacher.SetExecuteStudent();
    }
    public void ResetPlayer() {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Win() {
        player_won = true;
    }
}
