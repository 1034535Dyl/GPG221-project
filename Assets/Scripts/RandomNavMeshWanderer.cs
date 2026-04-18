using UnityEngine;
using UnityEngine.AI;

public class RandomNavMeshWanderer : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float wanderRadius = 8f;
    [SerializeField] private float sampleRadius = 2f;
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float rotationSpeed = 8f;
    [SerializeField] private float waypointReachedDistance = 0.25f;
    [SerializeField] private float destinationReachedDistance = 0.4f;
    [SerializeField] private int maxAttempts = 10;

    public NavMeshPath path;

    private int _currentCornerIndex;
    private Vector3 _currentDestination;
    private Transform _followTarget;

    private void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }

        path ??= new NavMeshPath();
    }

    private void Start()
    {
        PickNewDestination();
    }

    public void SetFollowTarget(Transform target)
    {
        _followTarget = target;
        RebuildPathForCurrentMode();
    }

    public void ClearFollowTarget()
    {
        _followTarget = null;
        RebuildPathForCurrentMode();
    }

    private void FixedUpdate()
    {
        if (path == null || path.corners == null || path.corners.Length == 0)
        {
            RebuildPathForCurrentMode();
            return;
        }

        if (_currentCornerIndex >= path.corners.Length)
        {
            RebuildPathForCurrentMode();
            return;
        }

        if (_followTarget != null)
        {
            // Keep path synced if the follow target moves.
            float targetDelta = (_currentDestination - _followTarget.position).sqrMagnitude;
            if (targetDelta > destinationReachedDistance * destinationReachedDistance)
            {
                RebuildPathForCurrentMode();
            }
        }

        Vector3 position = rb?.position ?? transform.position;
        Vector3 target = path.corners[_currentCornerIndex];
        Vector3 toTarget = target - position;
        toTarget.y = 0f;

        if (toTarget.sqrMagnitude <= waypointReachedDistance * waypointReachedDistance)
        {
            _currentCornerIndex++;
            if (_currentCornerIndex >= path.corners.Length)
            {
                RebuildPathForCurrentMode();
            }
            return;
        }

        Vector3 direction = toTarget.normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

        if (rb is not null)
        {
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
            rb.MovePosition(rb.position + (rb.transform.forward * (moveSpeed * Time.fixedDeltaTime)));
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
            transform.position += transform.forward * (moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void RebuildPathForCurrentMode()
    {
        if (_followTarget != null && TrySetPathTo(_followTarget.position))
        {
            return;
        }

        PickNewDestination();
    }

    private bool TrySetPathTo(Vector3 destination)
    {
        Vector3 origin = rb ? rb.position : transform.position;
        NavMeshPath newPath = new NavMeshPath();

        if (!NavMesh.CalculatePath(origin, destination, NavMesh.AllAreas, newPath))
        {
            return false;
        }

        if (newPath.status != NavMeshPathStatus.PathComplete || newPath.corners.Length == 0)
        {
            return false;
        }

        path = newPath;
        _currentDestination = destination;
        _currentCornerIndex = newPath.corners.Length > 1 ? 1 : 0;
        return true;
    }

    private void PickNewDestination()
    {
        Vector3 origin = rb ? rb.position : transform.position;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            Vector3 randomPoint = origin + Random.insideUnitSphere * wanderRadius;
            randomPoint.y = origin.y;

            if (!NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, sampleRadius, NavMesh.AllAreas))
            {
                continue;
            }

            if (TrySetPathTo(hit.position))
            {
                return;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (path == null || path.corners == null || path.corners.Length == 0)
        {
            return;
        }

        Gizmos.color = Color.cyan;
        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            Gizmos.DrawLine(path.corners[i], path.corners[i + 1]);
        }

        Gizmos.color = Color.yellow;
        foreach (Vector3 corner in path.corners)
        {
            Gizmos.DrawSphere(corner, 0.12f);
        }

        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(_currentDestination, 0.18f);
    }
}
