using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class LadybugWander : MonoBehaviour
{
    public float wanderRadius = 2.0f;
    public float wanderDistance = 3.0f;
    public float wanderJitter = 0.5f;
    public float speed = 2.0f;

    public Vector3 origin = Vector3.zero;
    public float boundaryRadius = 5.0f;

    private Vector3 wanderTarget;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        wanderTarget = transform.forward * wanderDistance;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(origin, boundaryRadius);
    }


    void FixedUpdate()
    {
        // Add jitter
        wanderTarget += new Vector3(
            Random.Range(-1f, 1f) * wanderJitter,
            0f,
            Random.Range(-1f, 1f) * wanderJitter
        );

        wanderTarget = wanderTarget.normalized * wanderRadius;

        Vector3 targetInWorld = transform.position + transform.forward * wanderDistance + wanderTarget;
        Vector3 desired = (targetInWorld - transform.position).normalized * speed;

        // Constrain to circular boundary
        Vector3 newPosition = rb.position + desired * Time.fixedDeltaTime;
        Vector3 offsetFromOrigin = newPosition - origin;

        if (offsetFromOrigin.magnitude > boundaryRadius)
        {
            // Reflect direction back toward center
            Vector3 toCenter = (origin - rb.position).normalized;
            desired = toCenter * speed;
        }

        rb.velocity = desired;

        // Face direction
        if (desired != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(desired);
            rb.rotation = Quaternion.Slerp(rb.rotation, lookRotation, 0.1f);
        }

        // Debug.DrawLine(transform.position, transform.position + desired, Color.green);
        // Debug.DrawWireSphere(origin, boundaryRadius); // visualize boundary
    }
}
