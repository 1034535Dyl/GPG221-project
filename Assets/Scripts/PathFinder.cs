using UnityEngine;
using UnityEngine.AI;

public class PathFinder : MonoBehaviour
{
    public Transform targetTransform;
    public NavMeshPath path;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        path = new NavMeshPath();
    }

    // Update is called once per frame
    void Update()
    {
        if (targetTransform == null)
        {
            return;
        }

        if (path == null)
        {
            path = new NavMeshPath();
        }

        NavMesh.CalculatePath(transform.position, targetTransform.position, NavMesh.AllAreas, path);
    }

    private void OnDrawGizmos()
    {
        if (path == null || path.corners == null || path.corners.Length == 0)
        {
            return;
        }

        Vector3 lastPos = Vector3.zero;
        for (var index = 0; index < path.corners.Length; index++)
        {
            var pathCorner = path.corners[index];

            if (index != 0)
            {
                Gizmos.DrawLine(lastPos, pathCorner);
            }
            lastPos = pathCorner;
        }
    }

    public void BuildPath(Vector3 targetPosition)
    {
        throw new System.NotImplementedException();
    }
}
