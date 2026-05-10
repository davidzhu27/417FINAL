using UnityEngine;
using Random = UnityEngine.Random;
using System;

public class PersonShooting : NPC {
    private float timeForMotion = 0.0f;
    [SerializeField] private Animator animator;
    //person_type = 0 -> mostly outdoors, walking a lot, 1 -> mostly indoors, occasionally walks
    //2-> test_taker, only does thinking idle animation
    [SerializeField] private int person_type = 0;
    private float acc_time = 0.0f;
    protected override void Awake() {
        moving = false;
        move_speed = 1.0f;
    }

    public void FacePlayer(Camera playerCamera)
    {
        if (playerCamera == null) return;
        FacePlayer(playerCamera.transform);
    }

    public void FacePlayer(Transform target)
    {
        if (target == null) return;

        Vector3 toTarget = target.position - transform.position;
        toTarget.y = 0f; // yaw only
        if (toTarget.sqrMagnitude < 0.0001f) return;

        transform.rotation = Quaternion.LookRotation(toTarget.normalized, Vector3.up);
    }

    public override void Update()
    {
        if (basic_path)
        {
            animateNPC();

            if (person_type == 99)
            {
                // Lock to Y-axis rotation only (no X/Z tilt) and disable horizontal translation.
                var euler = transform.eulerAngles;
                transform.rotation = Quaternion.Euler(0f, euler.y, 0f);
                return;
            }

            if (!moving) return;

            if (moving_direction == "x" || moving_direction == "-x")
            {
                transform.position = new Vector3(
                    transform.position.x + (move_sign * move_speed * Time.deltaTime),
                    transform.position.y,
                    transform.position.z
                );
            }
            else
            {
                transform.position = new Vector3(
                    transform.position.x,
                    transform.position.y,
                    transform.position.z + (move_sign * move_speed * Time.deltaTime)
                );
            }
        }
        else
        {
            // Non-basic paths not implemented here.
        }
    }
    public void setup(string npc_type) {
        if (npc_type.Equals("indoor", StringComparison.OrdinalIgnoreCase)) person_type = 1;
        else if (npc_type.Equals("test_taker", StringComparison.OrdinalIgnoreCase)) person_type = 2;
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
    public override void animateNPC() {
        if (person_type == 2) {
            if (acc_time < 1.0f) {
                animator.SetTrigger("ThinkingIdle");
                acc_time = 2.0f;
            } 
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
                if (person_type == 0) {
                    int z_dir = Random.Range(0, 2);
                    moving_direction = "z";
                    if (z_dir == 0) {
                        move_sign = 1.0f;
                        transform.Rotate(0, 0.0f - transform.eulerAngles.y, 0);
                    } else {
                        move_sign = -1.0f;
                        transform.Rotate(0, 180.0f - transform.eulerAngles.y, 0);
                    }
                    Walk(z_dir);
                } else {
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
}   
