using UnityEngine;

[CreateAssetMenu(fileName = "BugSettings", menuName = "Scriptable Objects/BugSettings")]
public class BugSettings : ScriptableObject
{
    public string bugName;
    public float speed = 1.0f;
    public float wanderRadius = 1.5f;
    public float wanderDistance = 2.0f;
    public Vector3 origin = Vector3.zero;
    public float boundaryRadius = 10.0f; 
}
