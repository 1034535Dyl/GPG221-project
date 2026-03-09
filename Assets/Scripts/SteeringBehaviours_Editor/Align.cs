using System.Collections.Generic;
using UnityEngine;

public class Align : SteeringBehaviour_Base
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    // Update is called once per frame


    // Variable pointing to your Neighbours component
    public Neighbours neighbours;
    public Rigidbody rb;
    public float force = 100f;


    void FixedUpdate()
    {
        // Some are Torque, some are Force		
        Vector3 targetDirection = CalculateMove(neighbours.NeighboursList);

        // Cross will take YOUR direction and the TARGET direction and turn 
        // it into a rotation force vector. It CROSSES through both at 90 degrees
        Vector3 cross = Vector3.Cross(transform.forward, targetDirection);

// TODO: Visualise the cross product vector/direction

        rb.AddTorque(cross * force);
    }

    public Vector3 CalculateMove(List<GameObject> neighbours)
    {
        if (neighbours.Count == 0)
            return Vector3.zero;

        Vector3 alignmentDirection = Vector3.zero;

        // Average of all neighbours directions
        // I'm using a list of transforms in my neighbours script, you might be using GameObjects etc
        foreach (GameObject item in neighbours)
        {
            alignmentDirection += item.transform.forward;
        }

        alignmentDirection /= neighbours.Count;

// TODO: Draw a debug line for the DESIRED direction (Your character won't immediately be snapping to this, line, they're GRADUALLY turn to it)
// Where I WANT to face
        Debug.DrawRay(transform.position, alignmentDirection.normalized * 10f, Color.blue);

        // Where I'm facing right now
        Debug.DrawRay(transform.position, transform.forward * 10f, Color.deepPink);
        return alignmentDirection;
    }
}