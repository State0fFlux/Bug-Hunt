using UnityEngine;

public class ScoutLeader : MonoBehaviour
{
    public GameObject canvas;
    private CapsuleCollider trigger;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        trigger = GetComponentInChildren<CapsuleCollider>();
        canvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        print("A");
        if (other.CompareTag("Player"))
        {
            print("B");
            canvas.SetActive(true);
            //trigger.enabled = false; // Disable the trigger after the player enters
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canvas.SetActive(false);
            //trigger.enabled = true; // Re-enable the trigger when the player exits
        }
    }
}
