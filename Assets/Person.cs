using UnityEngine;
using Random = UnityEngine.Random;
using System;

public class Person : NPC {
    private float timeForMotion = 0.0f;
    [SerializeField] private Animator animator;
    //person_type = 0 -> mostly outdoors, walking a lot, 1 -> mostly indoors, occasionally walks
    //2-> test_taker, only does thinking idle animation
    [SerializeField] private int person_type = 0;
    private float acc_time = 0.0f;
    protected override void Awake() {
        moving = true;
        move_speed = 1.0f;
    }
    public void setup(string npc_type) {
        if (npc_type.Equals("indoor", StringComparison.OrdinalIgnoreCase)) person_type = 1;
        else if (npc_type.Equals("test_taker", StringComparison.OrdinalIgnoreCase)) person_type = 2;
    }
    public void setup(string npc_type, Vector3 dest_coords) {
        basic_path = false;
        person_type = 2;
        agent_destination = dest_coords;
        agent.nextPosition = transform.position;
        agent.updatePosition = true;
        agent.SetDestination(agent_destination);
        moving = true;
        Walk(0);
    }
    public void Walk(int num) {
        if (num % 2 == 0) {
            animator.SetTrigger("BasicWalk");
            move_speed = 1.0f;
        } else {
            animator.SetTrigger("FastWalk");
            move_speed = 2.0f;
        }
        moving = true;
    }
    public void Idle(int num) {
        if (num % 3 == 0) {
            animator.SetTrigger("BasicIdle");
        } else if (num % 3 == 1) {
            animator.SetTrigger("ThinkingIdle");
        } else {
            animator.SetTrigger("PhoneIdle");
        }
        moving = false;
    }
    public void Sit() {
        moving = false;
        agent.velocity = Vector3.zero;
        agent.updatePosition = false;
        agent.ResetPath();
        animator.SetTrigger("SitTrigger");
    }
    public override void animateNPC() {
        if (person_type == 2) {
            return;
        }
        if (moving) {
            if (acc_time < timeForMotion) { 
                acc_time += Time.deltaTime;
            } else {
                int rand_num = Random.Range(0,2); 
                if (person_type == 0) timeForMotion = Random.Range(2.0f, 4.0f);
                else timeForMotion = Random.Range(4.0f, 8.0f);
                acc_time = 0.0f;
                moving = false;
                Idle(rand_num);
            }
        } else {
            if (acc_time < timeForMotion) {
                acc_time += Time.deltaTime;
            } else if (person_type == 1) {
                int rand_num = Random.Range(0,4);
                acc_time = 0.0f;
                if (rand_num % 2 == 0) {
                    moving = false;
                    Idle(rand_num);
                    timeForMotion = Random.Range(3.0f, 5.0f);
                } else {
                    moving = true;
                    rand_num = Random.Range(0,4);
                    timeForMotion = Random.Range(2.0f, 4.0f);
                    if (rand_num == 0) {
                        moving_direction = "x";
                        move_sign = 1.0f;
                        transform.Rotate(0, 90.0f-transform.eulerAngles.y, 0);
                    } else if (rand_num == 2) {
                        moving_direction = "-x";
                        move_sign = -1.0f;
                        transform.Rotate(0, 270.0f-transform.eulerAngles.y, 0);
                    } else if (rand_num == 1) {
                        moving_direction = "z";
                        move_sign = 1.0f;
                        transform.Rotate(0, 0.0f-transform.eulerAngles.y, 0);
                    } else {
                        moving_direction = "z";
                        move_sign = -1.0f;
                        transform.Rotate(0, 180.0f-transform.eulerAngles.y, 0);
                    }
                    Walk(rand_num);
                    }
            } else {
                int rand_num = Random.Range(0,4);
                timeForMotion = Random.Range(4.0f, 8.0f);
                acc_time = 0.0f;
                moving = true;
                if (rand_num == 0) {
                    moving_direction = "x";
                    move_sign = 1.0f;
                    transform.Rotate(0, 90.0f-transform.eulerAngles.y, 0);
                } else if (rand_num == 2) {
                    moving_direction = "-x";
                    move_sign = -1.0f;
                    transform.Rotate(0, 270.0f-transform.eulerAngles.y, 0);
                } else if (rand_num == 1) {
                    moving_direction = "z";
                    move_sign = 1.0f;
                    transform.Rotate(0, 0.0f-transform.eulerAngles.y, 0);
                } else {
                    moving_direction = "z";
                    move_sign = -1.0f;
                    transform.Rotate(0, 180.0f-transform.eulerAngles.y, 0);
                }
                Walk(rand_num);
                
            }
        }
    }
}   