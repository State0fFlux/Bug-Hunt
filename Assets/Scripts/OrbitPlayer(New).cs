using UnityEngine;

public class PlayerWithIndependentCamera : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 720f; // degrees per second

    [Header("Camera Settings")]
    public Transform cameraPivot; // assign the pivot object (camera's parent)
    public Transform body;
    public float cameraRotateSpeed = 50f;

    void Update()
    {
        HandleCameraRotation();
        HandleMovement();
    }

    void HandleMovement()
    {
        float h = Input.GetAxisRaw("Horizontal"); // A/D
        float v = Input.GetAxisRaw("Vertical");   // W/S

        Vector3 moveInput = new Vector3(h, 0, v).normalized;

        if (moveInput.magnitude > 0.1f)
        {
            // Get camera's horizontal orientation
            Vector3 camForward = cameraPivot.forward;
            Vector3 camRight = cameraPivot.right;

            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            // Movement direction relative to camera
            Vector3 moveDir = camForward * moveInput.z + camRight * moveInput.x;

            // Rotate player toward move direction
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            body.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);

            // Move player
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }
    }

    void HandleCameraRotation()
    {
        float camInput = 0f;

        if (Input.GetKey(KeyCode.LeftArrow))
            camInput = -1f;
        else if (Input.GetKey(KeyCode.RightArrow))
            camInput = 1f;

        if (camInput != 0f)
        {
            cameraPivot.Rotate(Vector3.up, camInput * cameraRotateSpeed * Time.deltaTime);
        }
    }
}
