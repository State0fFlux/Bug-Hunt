using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Travel : MonoBehaviour
{
    // changed these to private as theyre modified with scriptable objects
    private float speed = 1.0f;
    private float wanderRadius = 1.5f;         // CIRCLE_RADIUS
    private float wanderDistance = 2.0f;       // CIRCLE_DISTANCE
    private Vector3 origin = new Vector3(5, 0, 5);
    private float boundaryRadius = 5.0f;

    private float angleChange = 2.5f;          // ANGLE_CHANGE
    public float pullConst = 1.0f;

    public float wanderAngle = 0f;
    private Rigidbody rb;
    public Transform body;

    public BugSettings settings;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // body = transform.GetChild(0);

        if (settings != null)
        {
            speed = settings.speed;
            wanderRadius = settings.wanderRadius;
            wanderDistance = settings.wanderDistance;
            origin = settings.origin;
            boundaryRadius = settings.boundaryRadius;
        }   
    }

    void FixedUpdate()
    {
        Vector3 wanderForce = Wander();
        wanderForce.y = 0f;

        Vector3 position = rb.position;
        Vector3 toCenter = origin - position;
        toCenter.y = 0f;

        float distance = toCenter.magnitude;

        if (distance > boundaryRadius)
        {
            float overshoot = distance - boundaryRadius;

            float pullStrength = Mathf.Clamp01(overshoot / boundaryRadius);
            Vector3 pullForce = toCenter.normalized * speed * pullStrength * pullConst;

            wanderForce = Vector3.Lerp(wanderForce, pullForce, pullStrength);
        }

        rb.AddForce(wanderForce, ForceMode.Acceleration);
        Debug.DrawLine(position, position + wanderForce, Color.green);

        if (rb.linearVelocity.sqrMagnitude > 0.01f)
        {
            Quaternion look = Quaternion.LookRotation(rb.linearVelocity.normalized);
            body.rotation = Quaternion.Slerp(rb.rotation, look, 5f);
            //body.rotation = look;
        }

        float maxSpeed = speed;
        if(rb.linearVelocity.magnitude > maxSpeed)
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
    }

    Vector3 Wander()
    {
        Vector3 circleCenter = rb.linearVelocity.magnitude > 0.1f ? rb.linearVelocity.normalized : transform.forward;
        circleCenter *= wanderDistance;

        Vector3 displacement = new Vector3(0, 0, -1f) * wanderRadius;
        displacement = SetAngle(displacement, wanderAngle);

        wanderAngle += Random.value * angleChange - angleChange * 0.5f;
        return circleCenter + displacement;
    }

    Vector3 SetAngle(Vector3 vector, float angle)
    {
        float len = vector.magnitude;
        return new Vector3(Mathf.Cos(angle) * len, 0f, Mathf.Sin(angle) * len);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(origin, boundaryRadius);
    }
}
