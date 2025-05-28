using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Travel : MonoBehaviour
{
    public float wanderRadius = 1.5f;         // CIRCLE_RADIUS
    public float wanderDistance = 2.0f;       // CIRCLE_DISTANCE
    public float angleChange = 2.5f;          // ANGLE_CHANGE
    public float speed = 1.0f;

    public Vector3 origin = Vector3.zero;
    public float boundaryRadius = 10.0f;

    public float pullConst = 1.0f;

    private float wanderAngle = 0f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
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

        if (rb.velocity.sqrMagnitude > 0.01f)
        {
            Quaternion look = Quaternion.LookRotation(rb.velocity.normalized);
            rb.rotation = Quaternion.Slerp(rb.rotation, look, 0.1f);
        }

        float maxSpeed = speed;
        if(rb.velocity.magnitude > maxSpeed)
            rb.velocity = rb.velocity.normalized * maxSpeed;
    }

    Vector3 Wander()
    {
        Vector3 circleCenter = rb.velocity.magnitude > 0.1f ? rb.velocity.normalized : transform.forward;
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
