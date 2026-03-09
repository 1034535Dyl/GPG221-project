using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Spawner))]
public class SpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Spawner spawner = (Spawner)target;

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        // Create a styled button
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("SPAWN OBJECTS", GUILayout.Height(50)))
        {
            spawner.Spawn();
            Debug.Log("Objects spawned at " + spawner.transform.position);
        }
        GUI.backgroundColor = Color.white;
    }
}
