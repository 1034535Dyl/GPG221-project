using UnityEngine;


public class Avoid : SteeringBehaviour_Base
{
   [SerializeField]
   private float maxDistance = 3f;

    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private float turnSpeed = 15f;

   private void FixedUpdate()
   {
      RaycastHit hitInfo;
      bool       didItHitAnything;
      didItHitAnything = Physics.Raycast(transform.position, transform.forward, out hitInfo, maxDistance);


      if (didItHitAnything)
      {
         Debug.DrawLine(transform.position, hitInfo.point, Color.red);
        rb.AddRelativeTorque(0, turnSpeed, 0);
         
      }
      else
      {
         Debug.DrawLine(transform.position, transform.position + transform.forward * maxDistance, Color.green);
      }
   }
}


