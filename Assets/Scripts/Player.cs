using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // player movement
    public float walkSpeed = 2f;
    public float sprintSpeed = 4f;
    public float gravity = -9.81f;
    public float mass = 15;
    public Transform cameraTransform; // reference to the camera transform for movement direction
    public Transform body; // reference to the player body transform for rotation

    // player inventory
    public static int bugsNeeded = 5;
    public static Dictionary<BugType, int> inventory = new Dictionary<BugType, int>();
    public static Action OnInventoryUpdate;

    private CharacterController controller;
    private bool sprinting;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        foreach (BugType bug in Enum.GetValues(typeof(BugType)))
        {
            inventory[bug] = 0;
        }
        OnInventoryUpdate?.Invoke();
    }

    void Update()
    {
        // Gravity application
        Vector3 velocity = controller.velocity;
        if (!controller.isGrounded)
        {
            velocity.y += gravity * mass * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }

        sprinting = Input.GetButton("Sprint");

        // Get camera-relative input directions
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        // Combine input axes
        Vector3 input = Input.GetAxis("Horizontal") * right + Input.GetAxis("Vertical") * forward;

        // Project onto ground if grounded
        Vector3 move = input;
        if (controller.isGrounded && input != Vector3.zero)
        {
            if (Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out RaycastHit hit, 1.5f))
            {
                move = Vector3.ProjectOnPlane(input, hit.normal).normalized;
            }
        }

        // Move the player
        controller.Move(move * (sprinting ? sprintSpeed : walkSpeed) * Time.deltaTime);

        // Rotate the player to face movement direction
        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            body.rotation = Quaternion.Slerp(body.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    public void CatchBug(Bug bug)
    {
        inventory[bug.type]++;
        OnInventoryUpdate?.Invoke();
    }
}
