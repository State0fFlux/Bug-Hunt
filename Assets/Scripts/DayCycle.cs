using System;
using UnityEngine;
using UnityEngine.UIElements;

public class DayCycle : MonoBehaviour
{
    public float timeFactor = 2.0f; // Speed of rotation, adjust as needed
    public Color dayFogColor = Color.white;
    public Color nightFogColor = Color.black;
    private Light sunLight;

    void Start()
    {
        sunLight = GetComponent<Light>();
        RenderSettings.fog = true; // Ensure fog is enabled
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation *= Quaternion.Euler(timeFactor * Time.deltaTime, 0, 0);
        float sunAngle = transform.rotation.eulerAngles.x;

        if (sunAngle > 180 || sunAngle < 0) {
            sunLight.enabled = false;
        } else {
            sunLight.enabled = true;
            float blendFactor = Math.Abs(90 - sunAngle) / 90;
            RenderSettings.fogColor = Color.Lerp(dayFogColor, nightFogColor, blendFactor);
        }
    }
}
