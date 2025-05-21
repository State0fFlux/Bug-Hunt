using UnityEngine;

public class BugTravel : MonoBehaviour
{
    // TODO: currently acts on a single bug.
    // need multiple bugs as well as types of bug.
    public GameObject bug;
    private Transform bugTransform;

    public float speed;
    public Vector3 travelOrigin;
    public float travelRadius;

    private Vector3 dest;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bugTransform = bug.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Travel();
        //Debug.DrawRay(dest, Vector3.up * 0.01f, Color.blue);
        Debug.DrawLine(bugTransform.position, dest, Color.blue);
    }

    void OnDrawGizmos()
    {
        DrawCircle(travelOrigin, travelRadius, 32, Color.green);
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

    void Travel()
    {
        if (bug.transform.position == dest)
        {
            // TODO: this currently just moves the bug within a square lol.
            // Consider restricting movement to a circle? or a probability density fn would be fun.
            dest = new Vector3(Random.Range(travelOrigin.x-travelRadius, travelOrigin.x+travelRadius),
                0.0f,
                Random.Range(travelOrigin.z-travelRadius, travelOrigin.z+travelRadius));
            Debug.Log("new point drawn at " + dest);
            return;
        }
        
        // TODO: simple movement mechanic for now.
        // Could expand to use linear interpolation, bezier curve, etc.
        // Also, the bug should gradually face the direction it is flying.
        Vector3 bugToDest = dest - bugTransform.position;
        Quaternion rotation = Quaternion.LookRotation(bugToDest);

        bugTransform.rotation = Quaternion.Slerp(bugTransform.rotation, rotation, 0.1f);
        bugTransform.position = Vector3.MoveTowards(bugTransform.position, dest, speed * Time.deltaTime);    }
}
