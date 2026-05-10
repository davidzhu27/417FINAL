using UnityEngine;
using UnityEngine.AI;
public abstract class NPC : MonoBehaviour
{
    protected float move_speed = 5.0f;
    protected string moving_direction = "z";
    protected bool moving = true;
    protected bool basic_path = true;
    protected float move_sign = 1.0f;
    [SerializeField] protected NavMeshAgent agent;
    protected Vector3 agent_destination;
    protected virtual void Awake() {

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (basic_path) basicPathFollowing();
        else {
            if (moving) agent.SetDestination(agent_destination);
        }
    }
    public void basicPathFollowing() {
        if (agent != null) agent.updatePosition = false;
        animateNPC();
        if (moving) {
            if (moving_direction == "x" || moving_direction == "-x") {
                transform.position = new Vector3(transform.position.x+(move_sign*move_speed*Time.deltaTime), transform.position.y, transform.position.z);  
            } else {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z+(move_sign*move_speed*Time.deltaTime));
            }
        }
    }
    public abstract void animateNPC();
    public void collided() {
        moving = false;
    }
}
