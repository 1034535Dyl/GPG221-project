using UnityEngine;

public class TurnToward : SteeringBehaviour_Base
{
    public Transform target;
    public Rigidbody rb;
    public float speed;
 

    // Update is called once per frame
    void FixedUpdate()
    {
        //debug.draw
        Vector3 targetDirAndDistance;
        //get direction and distance
        targetDirAndDistance = target.position - transform.position;
        Debug.DrawRay(transform.position, targetDirAndDistance, Color.red);
        //keep just the direction
        Vector3 direction = targetDirAndDistance.normalized;
        Debug.DrawRay(transform.position, direction, Color.green);
        float angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);
        
        // Rotate RB toward target using torque
        float clampedAngle = Mathf.Clamp(angle, -1f, 1f);
        rb.AddTorque(0, clampedAngle * speed, 0);
    }
}
