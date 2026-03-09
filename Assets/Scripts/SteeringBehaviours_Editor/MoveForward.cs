using UnityEngine;

public class MoveForward : SteeringBehaviour_Base
{
   [SerializeField]
   private Rigidbody rb;

   [SerializeField]
   private float speed = 100f;

    void FixedUpdate()
    {
        rb.AddRelativeForce(0,0, speed);
        //#Debug.Log("Moving Forward");
    }
}
