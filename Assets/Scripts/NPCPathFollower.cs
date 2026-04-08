using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// NPC movement controller that follows the path calculated by PathFinder.cs
/// </summary>
public class NPCPathFollower : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PathFinder pathFinder;
    
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float stoppingDistance = 0.3f;
    [SerializeField] private bool rotateTowardMovement = true;
    
    private int currentWaypointIndex = 0;
    private Vector3 currentTarget;
    private bool isMoving = false;

    private void Start()
    {
        // Find PathFinder if not assigned
        if (pathFinder == null)
        {
            pathFinder = GetComponent<PathFinder>();
        }

        if (pathFinder == null)
        {
            Debug.LogError("NPCPathFollower: PathFinder component not found! Please assign it in the inspector or add it to this GameObject.");
        }
    }

    private void Update()
    {
        if (pathFinder == null || pathFinder.path == null || pathFinder.path.corners.Length == 0)
        {
            isMoving = false;
            return;
        }

        // Reset waypoint index if path changes
        if (currentWaypointIndex >= pathFinder.path.corners.Length)
        {
            currentWaypointIndex = 0;
        }

        // Get current target waypoint
        currentTarget = pathFinder.path.corners[currentWaypointIndex];

        // Check if reached current waypoint
        if (Vector3.Distance(transform.position, currentTarget) < stoppingDistance)
        {
            currentWaypointIndex++;
            
            // Check if reached end of path
            if (currentWaypointIndex >= pathFinder.path.corners.Length)
            {
                isMoving = false;
                return;
            }

            currentTarget = pathFinder.path.corners[currentWaypointIndex];
        }

        isMoving = true;

        // Move toward current waypoint
        MoveTowardWaypoint();

        // Rotate toward movement direction
        if (rotateTowardMovement)
        {
            RotateTowardMovement();
        }
    }

    /// <summary>
    /// Moves the NPC toward the current waypoint.
    /// </summary>
    private void MoveTowardWaypoint()
    {
        Vector3 direction = (currentTarget - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    /// <summary>
    /// Rotates the NPC to face the direction of movement.
    /// </summary>
    private void RotateTowardMovement()
    {
        Vector3 direction = (currentTarget - transform.position).normalized;
        
        if (direction.magnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Gets whether the NPC is currently moving.
    /// </summary>
    public bool IsMoving()
    {
        return isMoving;
    }

    /// <summary>
    /// Stops the NPC from moving.
    /// </summary>
    public void StopMovement()
    {
        isMoving = false;
        currentWaypointIndex = 0;
    }

    /// <summary>
    /// Resumes movement if stopped.
    /// </summary>
    public void ResumeMovement()
    {
        isMoving = true;
    }

    /// <summary>
    /// Sets the movement speed.
    /// </summary>
    public void SetMovementSpeed(float speed)
    {
        moveSpeed = Mathf.Max(0f, speed);
    }

    /// <summary>
    /// Gets the current movement speed.
    /// </summary>
    public float GetMovementSpeed()
    {
        return moveSpeed;
    }

    /// <summary>
    /// Draws the path and current waypoint in the scene for debugging.
    /// </summary>
    private void OnDrawGizmos()
    {
        if (pathFinder == null || pathFinder.path == null || pathFinder.path.corners.Length == 0)
            return;

        // Draw path
        Gizmos.color = Color.blue;
        for (int i = 0; i < pathFinder.path.corners.Length - 1; i++)
        {
            Gizmos.DrawLine(pathFinder.path.corners[i], pathFinder.path.corners[i + 1]);
        }

        // Draw waypoint spheres
        Gizmos.color = Color.yellow;
        foreach (Vector3 corner in pathFinder.path.corners)
        {
            Gizmos.DrawSphere(corner, 0.15f);
        }

        // Highlight current target waypoint
        if (currentWaypointIndex < pathFinder.path.corners.Length)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(pathFinder.path.corners[currentWaypointIndex], 0.2f);
        }

        // Draw stopping distance
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawSphere(currentTarget, stoppingDistance);
    }
}
