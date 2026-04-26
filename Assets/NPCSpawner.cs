using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private string npc_type;
    [SerializeField] private string desired_move_direction;
    [SerializeField] private int spawn_interval = 3;
    [SerializeField] private int max_live_spawns = 50;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
