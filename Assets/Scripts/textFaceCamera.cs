using UnityEngine;

public class textFaceCamera : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(worldPosition: transform.position + Camera.main.transform.rotation * Vector3.forward, worldUp:Camera.main.transform.rotation * Vector3.up);
    }
}
