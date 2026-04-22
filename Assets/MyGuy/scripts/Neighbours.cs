using System.Collections.Generic;
using UnityEngine;

public class Neighbours : MonoBehaviour
{
    public List<GameObject> NeighboursList = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        GameObject otherObject = other.gameObject;

        if (otherObject == gameObject)
        {
            return;
        }

        if (!NeighboursList.Contains(otherObject))
        {
            NeighboursList.Add(otherObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject otherObject = other.gameObject;
        while (NeighboursList.Remove(otherObject))
        {
            // Remove all duplicates in case they were added previously.
        }
    }

    private void LateUpdate()
    {
        // Handles destroyed/disabled neighbours where trigger exit may not fire.
        NeighboursList.RemoveAll(neighbour => neighbour == null || neighbour == gameObject || !neighbour.activeInHierarchy);
    }

    private void OnDisable()
    {
        NeighboursList.Clear();
    }

    private void OnDestroy()
    {
        NeighboursList.Clear();
    }
}