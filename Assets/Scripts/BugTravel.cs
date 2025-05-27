using UnityEngine;
using System.Collections.Generic;

public class BugTravel : MonoBehaviour
{
    // TODO: currently acts on a single bug.
    // need multiple bugs as well as types of bug.
    // private GameObject bug;
    // private Transform bugTransform;

    // public float speed;
    // public Vector3 travelOrigin;
    // public float travelRadius;

    private Vector3 dest;

    public struct Metadata
    {
        public float speed;
        public Vector3 origin;
        public float radius;

        public Metadata(float speed, Vector3 origin, float radius)
        {
            this.speed = speed;
            this.origin = origin;
            this.radius = radius;
        }
    }

    private Dictionary<string, Metadata> metadata = new Dictionary<string, Metadata>
    {
        {"LadyBug", new Metadata(1.0f, new Vector3(0.0f, 0.0f, 0.0f), 5.0f) }
    };

    private GameObject[] bugs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bugs = GameObject.FindGameObjectsWithTag("LadyBug");
        // bugTransform = bug.transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject bug in bugs)
        {
            Metadata rangeData = metadata["LadyBug"];
            Travel(bug.transform, rangeData);
            Debug.DrawLine(bug.transform.position, dest, Color.blue);
        }
        // Travel();
        //Debug.DrawRay(dest, Vector3.up * 0.01f, Color.blue);
        
    }

    void OnDrawGizmos()
    {
        Metadata rangeData = metadata["LadyBug"];
        DrawCircle(rangeData.origin, rangeData.radius, 32, Color.green);
    }

    void DrawCircle(Vector3 center, float radius, int segments, Color color)
    {
        float angleStep = 360f / segments;
        Vector3 prevPoint = center + new Vector3(Mathf.Cos(0), 0, Mathf.Sin(0)) * radius;

        for (int i = 1; i <= segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 nextPoint = center + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            Debug.DrawLine(prevPoint, nextPoint, color);
            prevPoint = nextPoint;
        }
    }

    void Travel(Transform bugTransform, Metadata rangeData)
    {
        if (bugTransform.position == dest)
        {
            // TODO: this currently just moves the bug within a square lol.
            // Consider restricting movement to a circle? or a probability density fn would be fun.
            dest = new Vector3(Random.Range(rangeData.origin.x-rangeData.radius, rangeData.origin.x+rangeData.radius),
                0.0f,
                Random.Range(rangeData.origin.z-rangeData.radius, rangeData.origin.z+rangeData.radius));
            Debug.Log("new point drawn at " + dest);
            return;
        }
        
        // TODO: simple movement mechanic for now.
        // Could expand to use linear interpolation, bezier curve, etc.
        // Also, the bug should gradually face the direction it is flying.
        Vector3 bugToDest = dest - bugTransform.position;
        Quaternion rotation = Quaternion.LookRotation(bugToDest);

        bugTransform.rotation = Quaternion.Slerp(bugTransform.rotation, rotation, 0.1f);
        bugTransform.position = Vector3.MoveTowards(bugTransform.position, dest, rangeData.speed * Time.deltaTime);    }
}
