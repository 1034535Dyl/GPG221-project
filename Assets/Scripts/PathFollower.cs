using UnityEngine;

public class PathFollower : MonoBehaviour
{
      public PathFinder pathFinder;
      public float speed = 5f;
      public float rotationSpeed = 10f;
      private int currentCornerIndex = 0;
   
      void Update()
      {
         if (pathFinder == null || pathFinder.path == null || pathFinder.path.corners.Length == 0)
         {
               return;
         }
   
         Vector3[] corners = pathFinder.path.corners;
   
         if (currentCornerIndex >= corners.Length)
         {
               return; // Reached the end of the path
         }
   
         Vector3 targetPosition = corners[currentCornerIndex];
         Vector3 toTarget = targetPosition - transform.position;
         Vector3 direction = toTarget.normalized;
         transform.position += direction * speed * Time.deltaTime;
   
         // Turn to face movement direction (yaw-only to avoid unwanted tilt).
         Vector3 flatDirection = new Vector3(direction.x, 0f, direction.z);
         if (flatDirection.sqrMagnitude > 0.0001f)
         {
               Quaternion targetRotation = Quaternion.LookRotation(flatDirection);
               transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
         }
   
         // Check if we are close enough to the target corner to move to the next one
         if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
         {
               currentCornerIndex++;
         }
      }
}
