using System;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PlayerState
{
    public readonly string name;
    public readonly float speed;
    public readonly float animationSpeed;
    private static readonly float ratio = 10f; // Adjust this ratio to control animation speed relative to movement speed

    public PlayerState(string name, float speed, float animationSpeed)
    {
        this.name = name;
        this.speed = speed;
        this.animationSpeed = animationSpeed;
    }

    // Define your "enum" values here
    public static readonly PlayerState Idle = new("Idle", 0f, 1f);
    public static readonly PlayerState Walking = new("Walking", 5f, 5f/ratio);
    public static readonly PlayerState Sprinting = new("Sprinting", 8f, 8f/ratio);
}

public class Player : MonoBehaviour
{

    // player movement
    [Header("Movement Settings")]
    public float speedScale = 1f;
    public float turnSmoothness = 25f; // delay for smooth turning;
    public Transform cameraPivot; // reference to the camera transform for movement direction
    public Transform body; // reference to the player body transform for rotation

    // player inventory
    [Header("Inventory Settings")]
    public static int bugsNeeded = 5;
    public static Dictionary<string, int> inventory = new Dictionary<string, int>();
    public static Action OnInventoryUpdate;

    // camera bobbing
    [Header("Camera Bobbing Settings")]
    public float bobAmount = 0.1f;  // amplitude of bobbing
    private float bobTimer = 0f;
    private Vector3 cameraInitialLocalPos;

    // components
    private Rigidbody rb;
    private Animator animator;

    // input variables
    private PlayerState currState = PlayerState.Idle;
    private float horizontalInput;
    private float verticalInput;

    // techy stuff
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

        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    void Update()
    {
        // Get input
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // Camera bobbing
        if (horizontalInput != 0f || verticalInput != 0f)
        {
            currState = Input.GetButton("Sprint") ? PlayerState.Sprinting : PlayerState.Walking;

            bobTimer += Time.deltaTime * 2 * currState.speed;
            float bobOffset = Mathf.Sin(bobTimer) * bobAmount;
            Vector3 newCamPos = cameraInitialLocalPos + new Vector3(0, bobOffset, 0);
            cameraPivot.localPosition = newCamPos;
        }
        else
        {
            currState = PlayerState.Idle;
            bobTimer = 0;
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


        // Trigger switch between walk animation or idle animation
        animator.SetBool("walking", currState != PlayerState.Idle);
        animator.speed = currState.animationSpeed * speedScale;
        // Move the player
        if (currState != PlayerState.Idle)
        {
            // Combine input axes
            Vector3 move = (horizontalInput * right + verticalInput * forward).normalized;

            rb.MovePosition(rb.position + currState.speed * Time.fixedDeltaTime * move * speedScale);
            float angle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg; // Rotate player to face movement direction
            Quaternion targetRotation = Quaternion.Euler(0f, angle, 0f);
            body.rotation = Quaternion.Slerp(body.rotation, targetRotation, Time.fixedDeltaTime * turnSmoothness * speedScale);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        Travel bug = other.gameObject.GetComponent<Travel>();
        CatchBug(bug.settings.bugName);
        Destroy(other.gameObject);
    }

    public void CatchBug(string bug)
    {
        print("Gotcha!");
        inventory[bug]++;
        OnInventoryUpdate?.Invoke();
        CheckInventory();
    }

    public void CheckInventory()
    {
        foreach (var bugCount in inventory.Values)
        {
            if (bugCount < bugsNeeded)
            {
                return;
            }
        }
        print("You have enough bugs!");
        UnityEngine.SceneManagement.SceneManager.LoadScene("WinScene"); // Load the win scene when all bugs are collected
    }
}
