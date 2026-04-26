using UnityEngine;

public class Vehicle : NPC {
    [SerializeField] private GameObject frontLeftW;
    [SerializeField] private GameObject backLeftW;
    [SerializeField] private GameObject frontRightW;
    [SerializeField] private GameObject backRightW;
    [SerializeField] private float rotation_speed = 60.0f;
    public void setup(string moving_dir) {
        moving_direction = moving_dir;
    }
    public override void animateNPC() {
        if(moving) {
            frontLeftW.transform.Rotate(rotation_speed*Time.deltaTime,0,0);
            backLeftW.transform.Rotate(rotation_speed*Time.deltaTime,0,0);
            frontRightW.transform.Rotate(rotation_speed*Time.deltaTime,0,0);
            backRightW.transform.Rotate(rotation_speed*Time.deltaTime,0,0);
        }
 
    }
    public void OnCollisionEnter(Collision collision) {
        if (!collision.gameObject.CompareTag("Ground") && collision.gameObject.name.ToLower() != "floor") moving = false;
    }
    public void OnCollisionExit(Collision collision) {
        if (!moving) moving = true;
    }
}