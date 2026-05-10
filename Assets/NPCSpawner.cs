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
    private float dest_x_coord = 0.0f;
    private float dest_z_coord = 18.0f;
    private int x_dir = 0;
    private int x_ind = 2;
    private bool isTestTaker = false;
    void Start()
    {
        isTestTaker = npc_type.Equals("test_taker", StringComparison.OrdinalIgnoreCase);
        spawnPerson();
    }

    // Update is called once per frame
    void Update()
    {
        if (num_spawned == max_spawns) gameObject.SetActive(false);
        if (acc_time < spawn_interval) acc_time+=Time.deltaTime;
        else {
            acc_time = 0.0f;
            spawnPerson();
        }
    }
    private void spawnPerson() {
        Vector3 offset;
        Quaternion init_rot;
        float rand_num;
        if (npc_type.Equals("outdoor", StringComparison.OrdinalIgnoreCase)) rand_num = Random.Range(0.2f, 0.8f);
        else if (isTestTaker) rand_num = Random.Range(0.2f, 0.6f);
        else rand_num = Random.Range(0.2f, 0.8f);
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
        if (num_spawned >= max_spawns) return;
        GameObject person = Instantiate(person_prefabs[ind], start_pos, init_rot);
        Person per = person.GetComponentInChildren<Person>();
        if (isTestTaker) {        
            Vector3 dest = new Vector3(dest_x_coord, 0.0f, dest_z_coord);
            x_ind-=1;
            if (x_ind == -1) {
                x_dir+=1;
                dest_z_coord-=4.0f;
                x_ind -=1;
            }
            x_ind = ((x_ind % 4) + 4) % 4;
            if (x_dir % 2 == 0) dest_x_coord+=8.0f;
            else dest_x_coord -= 8.0f;
            per.setup(npc_type, dest);
        }
        else per.setup(npc_type);
        num_spawned+=1;
    }
}
