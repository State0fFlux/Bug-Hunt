using System.Collections;
using UnityEngine;

public class ForceField : MonoBehaviour
{
    // flash settings
    [Header("Flash Settings")]
    public float flashDuration = 2f;
    public Color flashColor = Color.red;
    public float emissionIntensity = 1f;

    // components
    private Collider forceFieldCollider;
    private Material mat;
    // routines
    private Coroutine flashRunning;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        forceFieldCollider = GetComponent<Collider>();
        mat = GetComponent<Renderer>().material;
        mat.SetColor("_EmissionColor", Color.black);
    }

    private void OnCollisionStay(Collision collision)
    {
        print(collision);
        if (collision.gameObject.CompareTag("Player") && flashRunning == null)
        {
            flashRunning = StartCoroutine(FlashPulse());
        }
    }

    private IEnumerator FlashPulse()
    {
        float time = 0f;
        while (time < flashDuration)
        {
            float t = time / (flashDuration / 2f);
            float intensity = Mathf.PingPong(t, 1f); // goes from 0→1→0
            mat.SetColor("_EmissionColor", flashColor * intensity * emissionIntensity);

            time += Time.deltaTime;
            yield return null;
        }

        mat.SetColor("_EmissionColor", Color.black); // ensure it ends dark
        StopCoroutine(flashRunning);
        flashRunning = null;
    }
}
