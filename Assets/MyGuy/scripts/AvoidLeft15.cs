using UnityEngine;


public class AvoidLeft15 : SteeringBehaviour_Base
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
      // 45 degree angle to the left
      Vector3 rayDirection = Quaternion.Euler(0, -20, 0) * transform.forward;
      didItHitAnything = Physics.Raycast(transform.position, rayDirection, out hitInfo, maxDistance);


      if (didItHitAnything)
      {
         Debug.DrawLine(transform.position, hitInfo.point, Color.red);
        rb.AddRelativeTorque(0, turnSpeed, 0);
         
      }
      else
      {
         Debug.DrawLine(transform.position, transform.position + rayDirection * maxDistance, Color.green);
      }
   }
}


