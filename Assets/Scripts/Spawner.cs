using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private int amount;

    [SerializeField]
    private GameObject prefab;
    
    
    public void Spawn()
    {
        for (int i = 0; i < amount; i++)
        {
            Instantiate(prefab, transform.position, Quaternion.Euler(
                x: Random.Range(0, 5),
                Random.Range(0f, 360f),
                z: Random.Range(0, 5)));
        }
    }
}
