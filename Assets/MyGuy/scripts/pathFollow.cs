using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Steers myguy along the NavMeshPath calculated by a PathFinder component.
/// Attach alongside a PathFinder and a Rigidbody.
/// </summary>
public class PathFollow : SteeringBehaviour_Base
{
    [Header("References")]
    [SerializeField] private PathFinder pathFinder;
    [SerializeField] private Rigidbody rb;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float rotationSpeed = 8f;
    [SerializeField] private float waypointReachedDistance = 0.3f;

    private int currentWaypointIndex = 0;
    private NavMeshPath _lastPath;

    private void Awake()
    {
        if (pathFinder == null)
            pathFinder = GetComponent<PathFinder>();

        if (rb == null)
            rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (pathFinder == null || pathFinder.path == null || pathFinder.path.corners.Length == 0)
            return;

        // Reset index whenever a new path is received
        if (pathFinder.path != _lastPath)
        {
            currentWaypointIndex = 0;
            _lastPath = pathFinder.path;
        }

        if (currentWaypointIndex >= pathFinder.path.corners.Length)
            return;

        Vector3 target = pathFinder.path.corners[currentWaypointIndex];
        Vector3 toTarget = target - transform.position;
        toTarget.y = 0f; // keep movement flat

        // Rotate smoothly toward next waypoint
        if (toTarget.magnitude > 0.01f)
        {
            Quaternion desiredRot = Quaternion.LookRotation(toTarget.normalized);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, desiredRot, rotationSpeed * Time.fixedDeltaTime));
        }

        // Move forward along the path
        rb.MovePosition(transform.position + transform.forward * moveSpeed * Time.fixedDeltaTime);

        // Advance to next waypoint when close enough
        if (Vector3.Distance(transform.position, target) < waypointReachedDistance)
        {
            currentWaypointIndex++;
        }
    }

    private void OnDrawGizmos()
    {
        if (pathFinder == null || pathFinder.path == null || pathFinder.path.corners.Length == 0)
            return;

        // Draw remaining path from current waypoint
        Gizmos.color = Color.cyan;
        for (int i = currentWaypointIndex; i < pathFinder.path.corners.Length - 1; i++)
            Gizmos.DrawLine(pathFinder.path.corners[i], pathFinder.path.corners[i + 1]);

        // Highlight next waypoint
        if (currentWaypointIndex < pathFinder.path.corners.Length)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(pathFinder.path.corners[currentWaypointIndex], 0.2f);
        }
    }
}
