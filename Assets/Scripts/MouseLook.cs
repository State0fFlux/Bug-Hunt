using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float sensitivityX = 15f;
    public float sensitivityY = 4f;
    public float minimumY = 0f;
    public float maximumY = 30f;

    private float rotationY = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Hide and lock the cursor
    }

    void Update()
    {
        float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
        transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
    }

    void OnDestroy()
    {
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor when the script is destroyed
    }
}