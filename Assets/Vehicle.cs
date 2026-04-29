using UnityEngine;

public class Vehicle : NPC {
    [SerializeField] private GameObject frontLeftW;
    [SerializeField] private GameObject backLeftW;
    [SerializeField] private GameObject frontRightW;
    [SerializeField] private GameObject backRightW;
    [SerializeField] private float rotation_speed = 60.0f;
    private float alive_time = 0.0f;
    [SerializeField] private float lifetime = 5.0f;
    public void setup(string moving_dir, float obj_lifetime) {
        moving_direction = moving_dir;
        if (moving_dir[0] == '-') move_sign = -1.0f;
        lifetime = obj_lifetime;
    }
    public override void animateNPC() {
        if (alive_time >= lifetime && moving) Destroy(transform.parent.gameObject);
        if(moving) {
            frontLeftW.transform.Rotate(rotation_speed*Time.deltaTime,0,0);
            backLeftW.transform.Rotate(rotation_speed*Time.deltaTime,0,0);
            frontRightW.transform.Rotate(rotation_speed*Time.deltaTime,0,0);
            backRightW.transform.Rotate(rotation_speed*Time.deltaTime,0,0);
        }
        alive_time += Time.deltaTime;
 
    }
    public void stop() {
        moving = false;
    }
    public void start_moving() {
        moving = true;
    }
    public void OnCollisionEnter(Collision collision) {
        if (!collision.gameObject.CompareTag("Ground") && collision.gameObject.name.ToLower() != "floor") moving = false;
    }
    public void OnCollisionExit(Collision collision) {
        if (!moving) moving = true;
    }
}