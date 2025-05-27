using UnityEngine;
using UnityEngine.UIElements;

public class DayCycle : MonoBehaviour
{
    public float timeFactor = 2.0f; // Speed of rotation, adjust as needed
    private Light sunLight;

    void Start()
    {
        sunLight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation *= Quaternion.Euler(timeFactor * Time.deltaTime, 0, 0);
        var currentRotation = transform.rotation.eulerAngles;
        if (currentRotation.x >= 180.0f || currentRotation.x <= 0)
        {
            sunLight.enabled = false; // Disable light when the sun is below the horizon
        }
        else
        {
            sunLight.enabled = true; // Enable light when the sun is above the horizon
        }
    }
}
