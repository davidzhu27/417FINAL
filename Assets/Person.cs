using UnityEngine;
using Random = UnityEngine.Random;

public class Person : NPC {
    private int framesToKeepMotion = 1;
    private int numFramesForMotion = 0;
    [SerializeField] private Animator animator;
    private bool taking_test = false;
    protected override void Awake() {
        moving = false;
        move_speed = 1.0f;
    }
    public void setup(bool set_taking_test) {
        taking_test = set_taking_test;
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
        if (taking_test) {
            if (numFramesForMotion == 0) {
                animator.SetTrigger("ThinkingIdle");
                numFramesForMotion = 1;
            } 
            return;
        }
        if (moving) {
            if (numFramesForMotion < framesToKeepMotion) {
                numFramesForMotion+=1;
            } else {
                int rand_num = Random.Range(40,60);
                numFramesForMotion = 0;
                framesToKeepMotion = rand_num;
                moving = false;
                Idle(rand_num);
            }
        } else {
            if (numFramesForMotion < framesToKeepMotion) {
                numFramesForMotion+=1;
            } else {
                int rand_num = Random.Range(100,200);
                numFramesForMotion = 0;
                framesToKeepMotion = rand_num;
                moving = true;
                if (rand_num % 4 == 0) {
                    moving_direction = "x";
                    move_sign = 1.0f;
                    transform.Rotate(0, 90.0f-transform.eulerAngles.y, 0);
                } else if (rand_num % 4 == 2) {
                    moving_direction = "-x";
                    move_sign = -1.0f;
                    transform.Rotate(0, 270.0f-transform.eulerAngles.y, 0);
                } else if (rand_num % 4 == 1) {
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
