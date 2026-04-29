using UnityEngine;
using Random = UnityEngine.Random;
using System;
public class NPCSpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private string npc_type;
    [SerializeField] private float spawn_interval = 2.0f;
    [SerializeField] private int max_spawns = 30;
    [SerializeField] private GameObject[] person_prefabs;
    private int num_spawned = 0;
    private float acc_time = 0.0f;
    void Start()
    {
        spawnPerson();
    }

    // Update is called once per frame
    void Update()
    {
        if (acc_time < spawn_interval) acc_time+=Time.deltaTime;
        else {
            acc_time = 0.0f;
            spawnPerson();
        }
        if (num_spawned == max_spawns) gameObject.SetActive(false);
    }
    private void spawnPerson() {
        Vector3 offset;
        Quaternion init_rot;
        float rand_num;
        if (npc_type.Equals("outdoor", StringComparison.OrdinalIgnoreCase)) rand_num = Random.Range(0.2f, 0.8f);
        else rand_num = Random.Range(1.0f, 4.0f);
        int spawn_dir = Random.Range(0,4);
        if (spawn_dir == 0) {
            offset = new Vector3(-rand_num,0.0f,0.0f);
            init_rot = Quaternion.AngleAxis(270, Vector3.up);
        } else if (spawn_dir == 1) {
            offset = new Vector3(rand_num,0.0f,0.0f);
            init_rot = Quaternion.AngleAxis(90, Vector3.up);
        } else if (spawn_dir == 2) {
            offset = new Vector3(0.0f,0.0f,rand_num);
            init_rot = Quaternion.AngleAxis(0, Vector3.up);
        } else {
            offset = new Vector3(0.0f,0.0f,-rand_num);
            init_rot = Quaternion.AngleAxis(180, Vector3.up);
        }
        Vector3 start_pos = transform.position+offset;
        start_pos.y = 0.0f;
        int ind = Random.Range(0, person_prefabs.Length);
        GameObject person = Instantiate(person_prefabs[ind], start_pos, init_rot);
        Person per = person.GetComponentInChildren<Person>();
        per.setup(npc_type);
        num_spawned+=1;
    }
}
