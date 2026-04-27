using UnityEngine;

public class CarMover : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float speed = 3f;

    private Vector3 target;

    void Start()
    {
        if (startPoint != null)
        {
            transform.position = startPoint.position;
        }

        if (endPoint != null)
        {
            target = endPoint.position;
        }
    }

    void Update()
    {
        if (startPoint == null || endPoint == null)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            transform.position = startPoint.position;
            target = endPoint.position;
        }
    }
}