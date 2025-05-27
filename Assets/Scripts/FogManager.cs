using UnityEngine;

public class FogController : MonoBehaviour
{
    void Start()
    {
        // Enable fog
        RenderSettings.fog = true;

        // Set fog color
        RenderSettings.fogColor = Color.white;
    }

    void Update()
    {
        // Example: Animate fog color based on time
        float t = Mathf.PingPong(Time.time, 1f);
        RenderSettings.fogColor = Color.Lerp(Color.white, Color.black, t);
    }
}
