using UnityEngine;
using TMPro;

public class CrossingTimer : MonoBehaviour
{
    public float timeLimit = 5f;
    public TMP_Text timerText;
    public Transform player;
    public Transform playerStartPoint;

    private float currentTime;

    void Start()
    {
        ResetTimer();
    }

    void Update()
    {
        currentTime -= Time.deltaTime;

        if (timerText != null)
        {
            timerText.text = "Time: " + Mathf.Ceil(currentTime).ToString();
        }

        if (currentTime <= 0f)
        {
            ResetPlayer();
            ResetTimer();
        }
    }

    void ResetPlayer()
    {
        if (player != null && playerStartPoint != null)
        {
            player.position = playerStartPoint.position;
        }
    }

    public void ResetTimer()
    {
        currentTime = timeLimit;
    }
}