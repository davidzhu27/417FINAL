using UnityEngine;
using Random = UnityEngine.Random;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] private string desired_move_direction;
    [SerializeField] private float spawn_interval = 5.0f;
    [SerializeField] private GameObject[] car_prefabs;
    [SerializeField] private float car_lifetime = 4.0f;
    private Vector3 start_pos;
    private Quaternion init_rot;
    private float acc_time = 0.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 offset;
        if (desired_move_direction == "-x") {
            offset = new Vector3(-0.5f,0.0f,0.0f);
            init_rot = Quaternion.AngleAxis(270, Vector3.up);
        } else if (desired_move_direction == "x") {
            offset = new Vector3(0.5f,0.0f,0.0f);
            init_rot = Quaternion.AngleAxis(90, Vector3.up);
        } else if (desired_move_direction == "z") {
            offset = new Vector3(0.0f,0.0f,0.5f);
            init_rot = Quaternion.AngleAxis(0, Vector3.up);
        } else {
            offset = new Vector3(0.0f,0.0f,-0.5f);
            init_rot = Quaternion.AngleAxis(180, Vector3.up);
        }
        start_pos = transform.position+offset;
        start_pos.y = 0.0f;
        int ind = Random.Range(0, car_prefabs.Length);
        GameObject car = Instantiate(car_prefabs[ind], start_pos, init_rot);
        Vehicle veh = car.GetComponentInChildren<Vehicle>();
        veh.setup(desired_move_direction, car_lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        if (acc_time < spawn_interval) acc_time+=Time.deltaTime;
        else {
            acc_time = 0.0f;
            int ind = Random.Range(0, car_prefabs.Length);
            GameObject car = Instantiate(car_prefabs[ind], start_pos, init_rot);
            Vehicle veh = car.GetComponentInChildren<Vehicle>();
            veh.setup(desired_move_direction, car_lifetime);
        }
    }
}
