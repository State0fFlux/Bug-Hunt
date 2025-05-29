using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // player movement
    [Header("Movement Settings")]
    public float walkSpeed = 3f; // slowed a little to play nicer with walk cycle
    public float sprintSpeed = 5f;
    public float turnSmoothness = 25f; // delay for smooth turning;
    public Transform cameraPivot; // reference to the camera transform for movement direction
    public Transform body; // reference to the player body transform for rotation

    // player inventory
    [Header("Inventory Settings")]
    public static int bugsNeeded = 5;
    public static Dictionary<string, int> inventory = new Dictionary<string, int>();
    public static Action OnInventoryUpdate;

    // components
    private Rigidbody rb;
    private Animator animator;

    // input variables
    private bool sprintInput;
    private float horizontalInput;
    private float verticalInput;

    // camera bobbing
    [Header("Camera Bobbing Settings")]
    public float bobAmount = 0.1f;  // amplitude of bobbing
    private float bobTimer = 0f;
    private Vector3 cameraInitialLocalPos;

    public string[] bugs = { "Firefly", "Ladybug" };

    void Start()
    {
        foreach (string bug in bugs)
        {
            inventory[bug] = 0;
        }
        OnInventoryUpdate?.Invoke();
        cameraInitialLocalPos = cameraPivot.localPosition;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Get input
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        sprintInput = Input.GetButton("Sprint");

        // Camera bobbing
        if (horizontalInput != 0f || verticalInput != 0f)
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
        Vector3 move = (Input.GetAxis("Horizontal") * right + Input.GetAxis("Vertical") * forward).normalized;
        
        // Trigger switch between walk animation or idle animation
        animator.SetBool("walking", move != Vector3.zero);
        print(animator.GetBool("walking"));

        // Move the player
        rb.MovePosition(rb.position + (sprintInput ? sprintSpeed : walkSpeed) * Time.fixedDeltaTime * move);

        // Rotate the player to face movement direction
        if (move != Vector3.zero)
        {
            float angle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(-90f, 0f, angle);  // keep -90 X tilt, rotate around Z
            body.rotation = Quaternion.Slerp(body.rotation, targetRotation, Time.fixedDeltaTime * turnSmoothness);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        // Debug.Log("hit!");
        // if (other.gameObject.tag == "Bug")
        // {
        Travel bug = other.gameObject.GetComponent<Travel>();
            // if (travel.BugSettings.bugName == "LadyBug")
            // {

            // }
            CatchBug(bug.settings.bugName);
            Destroy(other.gameObject);
        // }
    }

    public void CatchBug(string bug)
    {
        inventory[bug]++;
        OnInventoryUpdate?.Invoke();
    }
}
