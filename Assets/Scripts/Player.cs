using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // player movement
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 7f;
    public Transform cameraPivot; // reference to the camera transform for movement direction
    public Transform body; // reference to the player body transform for rotation
    
    public bool walking;

    // player inventory
    [Header("Inventory Settings")]
    public static int bugsNeeded = 5;
    public static Dictionary<BugType, int> inventory = new Dictionary<BugType, int>();
    public static Action OnInventoryUpdate;

    // components
    private Rigidbody rb;
    // input variables
    private bool sprintInput;
    private float horizontalInput;
    private float verticalInput;

    // camera bobbing
    [Header("Camera Bobbing Settings")]
    public float bobAmount = 0.1f;  // amplitude of bobbing
    private float bobTimer = 0f;
    private Vector3 cameraInitialLocalPos;

    // character animations - being used for debug purposes in another scene
    // ignore/remove if these are causing problems
    //[Header("Character Animation Settings")]
    //public Animator animator;

    void Start()
    {
        foreach (BugType bug in Enum.GetValues(typeof(BugType)))
        {
            inventory[bug] = 0;
        }
        OnInventoryUpdate?.Invoke();
        cameraInitialLocalPos = cameraPivot.localPosition;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        // Get input
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        sprintInput = Input.GetButton("Sprint");

        // Camera bobbing
        if (horizontalInput != 0f || verticalInput != 0f) // && controller.isGrounded)
        {
            bobTimer += Time.deltaTime * 2 * (sprintInput ? sprintSpeed: walkSpeed);
            float bobOffset = Mathf.Sin(bobTimer) * bobAmount;
            Vector3 newCamPos = cameraInitialLocalPos + new Vector3(0, bobOffset, 0);
            cameraPivot.localPosition = newCamPos;
        }
        else
        {
            bobTimer = 0f;
            cameraPivot.localPosition = Vector3.Lerp(cameraPivot.localPosition, cameraInitialLocalPos, Time.deltaTime * 5f);
        }
    }

    void FixedUpdate()
    {
        // Get camera-relative input directions
        Vector3 forward = cameraPivot.forward;
        Vector3 right = cameraPivot.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        // Combine input axes
        Vector3 input = Input.GetAxis("Horizontal") * right + Input.GetAxis("Vertical") * forward;

        // Project onto ground if grounded
        Vector3 move = input;
        if (input != Vector3.zero)
        {
            if (Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out RaycastHit hit, 1.5f)) // project onto ground
            {
                move = Vector3.ProjectOnPlane(input, hit.normal).normalized;
            }
        }
        
        // Trigger switch between walk animation or idle animation
        //animator.SetBool("walking", move.magnitude > 0.001f);

        // Move the player
        rb.MovePosition(rb.position + (sprintInput ? sprintSpeed : walkSpeed) * Time.fixedDeltaTime * move);
        Vector3 targetVelocity = (sprintInput ? sprintSpeed : walkSpeed) * move;
        targetVelocity.y = rb.linearVelocity.y; // Keep vertical velocity (gravity)
        rb.linearVelocity = targetVelocity;

        // Rotate the player to face movement direction
        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            body.rotation = Quaternion.Slerp(body.rotation, targetRotation, Time.fixedDeltaTime * 5f);
        }
    }

    public void CatchBug(Bug bug)
    {
        inventory[bug.type]++;
        OnInventoryUpdate?.Invoke();
    }
}
