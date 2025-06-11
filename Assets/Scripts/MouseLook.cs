using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float sensitivityX = 15f;
    public float sensitivityY = 4f;
    public float minimumY = 0f;
    public float maximumY = 30f;

    private float rotationY = 0f;
    private bool cursorLocked = true;

    void Start()
    {
        LockCursor();
    }

    void Update()
    {
        HandleCursorToggle();

        if (cursorLocked)
        {
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }
    }

    void HandleCursorToggle()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnlockCursor();
        }
        else if (!cursorLocked && Input.GetMouseButtonDown(0))
        {
            LockCursor();
        }
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cursorLocked = true;
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        cursorLocked = false;
    }

    void OnDestroy()
    {
        UnlockCursor();
    }
}
