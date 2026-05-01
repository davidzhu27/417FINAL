using UnityEngine;
using UnityEngine.Events;
public class EventManager : MonoBehaviour
{
    public static EventManager Instance;
    public UnityEvent stopCars;
    public UnityEvent startCars;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stopCars = new UnityEvent();
        startCars = new UnityEvent();
        if (Instance == null) Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
