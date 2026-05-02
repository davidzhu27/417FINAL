using UnityEngine;
using System.Collections.Generic;

public class Teacher : MonoBehaviour
{
    public Transform playerCoords;
    public UnityEngine.AI.NavMeshAgent agent;
    private bool killStudent = false;
    private Vector3 current_waypoint;
    private Vector3 initial_waypoint;
    private int cur_ind;
    private int moving_dir; //moving_dir=0 -> +z, 1->-z, 2->+x,3->-x
    private int[] moving_dir_loop = new int[] {0,3,1,3,0,2,1,2};
    private float move_speed = 1.0f;
    private float move_sign = 1.0f;
    private bool reached_initial_waypoint = false;
    private bool reached_player = false;
    private bool executed = false;
    private Vector3 initial_location;
    public Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initial_location = transform.position;
        initial_waypoint = new Vector3(12.0f, 0.0f, -8.0f);
        agent.SetDestination(initial_waypoint);
        animator.SetTrigger("WalkTrigger");
    }

    // Update is called once per frame
    void Update()
    {
        if (killStudent && !reached_player) {
            agent.nextPosition = transform.position;
            agent.updatePosition = true;
            agent.SetDestination(playerCoords.position);
            if (!agent.pathPending && (agent.remainingDistance <= agent.stoppingDistance) && agent.velocity.sqrMagnitude == 0f) {
                reached_player = true;
            }
        } else if (killStudent){
            if (!executed) ExecuteStudent();
        } else if (!reached_initial_waypoint) {
            if (!agent.pathPending && (agent.remainingDistance <= agent.stoppingDistance) && agent.velocity.sqrMagnitude == 0f) {
                reached_initial_waypoint = true;
                current_waypoint = new Vector3(12.0f, 0.0f, 20.0f);
                agent.updatePosition = false;
                cur_ind = 0;
                moving_dir = moving_dir_loop[cur_ind];
                transform.Rotate(0.0f, 0.0f-transform.eulerAngles.y, 0.0f);
            }
        } else {
            if (Vector3.Distance(transform.position, current_waypoint) < 1.0f) {
                cur_ind = (cur_ind+1)%moving_dir_loop.Length;
                moving_dir = moving_dir_loop[cur_ind];
                if (moving_dir == 0) {
                    transform.Rotate(0.0f, 0.0f-transform.eulerAngles.y, 0.0f);
                    move_sign = 1.0f;
                }
                else if (moving_dir == 1){
                    transform.Rotate(0.0f, 180.0f-transform.eulerAngles.y, 0.0f);
                    move_sign = -1.0f;
                } 
                else if (moving_dir == 2) {
                    transform.Rotate(0.0f, 90.0f - transform.eulerAngles.y, 0.0f);
                    move_sign = 1.0f;
                }
                else {
                    transform.Rotate(0.0f, 270.0f - transform.eulerAngles.y, 0.0f);
                    move_sign = -1.0f;
                }
                if (moving_dir / 2 == 0) current_waypoint.z += move_sign*28.0f;
                else current_waypoint.x += move_sign*8.0f;
                // Debug.Log("New current waypoint:");
                // Debug.Log(current_waypoint);
                // Debug.Log("New moving_dir");
                // Debug.Log(moving_dir);
            } else {
                if (moving_dir / 2 == 0) {
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z+(move_sign*move_speed*Time.deltaTime));
                } else {
                    transform.position = new Vector3(transform.position.x+(move_sign*move_speed*Time.deltaTime), transform.position.y, transform.position.z);
                }
            }
        }
    }

    public void SetExecuteStudent() {
        agent.nextPosition = transform.position;
        killStudent = true;
        animator.SetTrigger("StudentTrigger");
    }
    public void ExecuteStudent() {
        executed = true;
        ResetWorld();
    }
    public void ResetWorld() {
        killStudent = false;
        reached_player = false;
        reached_initial_waypoint = false;
        executed = false;

        transform.position = initial_location;
        agent.updatePosition = false;
        agent.nextPosition = initial_location;
        agent.updatePosition = true;
        agent.SetDestination(initial_waypoint);
    }
}
