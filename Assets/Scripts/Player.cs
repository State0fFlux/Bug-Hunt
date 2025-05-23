using System;
using System.Collections.Generic;
using UnityEditor.Rendering.BuiltIn.ShaderGraph;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    // player movement
    public static float moveSpeed = 10f;
    public static float turnSpeed = 200f;
    public static float gravity = -9.81f;

    // player inventory
    public static int bugsNeeded = 5; // number of each bug type needed to complete the quest
    public static Dictionary<BugType, int> inventory = new Dictionary<BugType, int>(); // diff bug types
    public static Action OnInventoryUpdate; // event to notify UI of inventory changes

    // other stuff
    private CharacterController controller;
    private Rigidbody rb;
    private Vector3 velocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Optional: Freeze rotation on X and Z to prevent tipping over
        rb.freezeRotation = true;
        
        controller = GetComponent<CharacterController>();
        foreach (BugType bug in System.Enum.GetValues(typeof(BugType)))
        {
            inventory[bug] = 0;
        }
        OnInventoryUpdate?.Invoke(); // initialize inventory UI
    }

    // Update is called once per frame
    void Update()
    {
        // NAIVE WASD CONTROLS
        /*
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Gravity handling
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        */


        // TANK CONTROLS
        /*
        float moveInput = Input.GetAxis("Vertical");
        float turnInput = Input.GetAxis("Horizontal");
        Vector3 move = transform.forward * moveInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);

        // Turn left/right
        float turn = turnInput * turnSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
        */
        
        // ORBIT CAM CONTROLS
    }

    public void CatchBug(Bug bug)
    {
        inventory[bug.type] = inventory[bug.type] + 1;
        OnInventoryUpdate?.Invoke();
    }
}
