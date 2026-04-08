using UnityEngine;

public class TurnTowards : MonoBehaviour
{
    [SerializeField] private PathFinder pathFinder;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float turnSpeed = 5f;
    [SerializeField] private float cornerReachedDistance = 0.1f;
    [SerializeField] private float minTurnAngle = 2f;

    void Awake()
    {
        if (pathFinder == null)
        {
            pathFinder = GetComponent<PathFinder>();
        }

        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    void FixedUpdate()
    {
        if (rb == null || pathFinder == null || pathFinder.path == null || pathFinder.path.corners == null)
        {
            return;
        }

        Vector3[] corners = pathFinder.path.corners;
        if (corners.Length == 0)
        {
            return;
        }

        Vector3 currentPosition = rb.position;
        Vector3 currentForward = rb.rotation * Vector3.forward;
        currentForward.y = 0f;

        Vector3 targetDirection = Vector3.zero;
        float cornerReachedDistanceSqr = cornerReachedDistance * cornerReachedDistance;

        for (int i = 0; i < corners.Length; i++)
        {
            Vector3 toCorner = corners[i] - currentPosition;
            toCorner.y = 0f;

            if (toCorner.sqrMagnitude <= cornerReachedDistanceSqr)
            {
                continue;
            }

            targetDirection = toCorner.normalized;
            break;
        }

        if (targetDirection == Vector3.zero)
        {
            return; // No meaningful next corner to face.
        }

        float angle = Vector3.Angle(currentForward, targetDirection);
        if (angle < minTurnAngle)
        {
            return; // Already facing the path closely enough.
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime));
    }
}
