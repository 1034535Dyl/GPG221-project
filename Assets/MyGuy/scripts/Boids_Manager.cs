using System.Collections.Generic;
using UnityEngine;

public class Boids_Manager : SteeringBehaviour_Base

{
    public void ToggleBoids()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        List<GameObject> myguyClones = new List<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.name.Contains("myguy"))
            {
                myguyClones.Add(obj);
            }
        }




        foreach (Align item in FindObjectsByType<Align>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
            item.enabled = !item.enabled;
    }

    private GameObject[] FindObjectsOfType<T>(FindObjectsInactive includeInactive, FindObjectsSortMode none)
    {
        return new GameObject[] { };
    }
}

