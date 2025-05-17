using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FloraSpawner))]
public class TreeSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        FloraSpawner spawner = (FloraSpawner)target;

        EditorGUILayout.Space();

        if (GUILayout.Button("Spawn All"))
        {
            Undo.RegisterFullObjectHierarchyUndo(spawner.gameObject, "Spawn All");
            spawner.SpawnAll();
        }

        if (GUILayout.Button("Clear All"))
        {
            Undo.RegisterFullObjectHierarchyUndo(spawner.gameObject, "Clear All");
            spawner.ClearAll();
        }
    }
}
