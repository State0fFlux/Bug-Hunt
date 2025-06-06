using UnityEngine;

public class ScoutLeader : MonoBehaviour
{
    private GameObject dialogue;
    private GameObject exclamation;
    private CapsuleCollider trigger;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialogue = transform.Find("Dialogue").gameObject;
        exclamation = transform.Find("Exclamation").gameObject;
        if (dialogue == null || exclamation == null)
        {
            Debug.LogError("Dialogue or Exclamation GameObject not found in character.");
            return;
        }

        trigger = GetComponentInChildren<CapsuleCollider>();
        dialogue.SetActive(false);
        exclamation.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        print("A");
        print(other);
        if (other.CompareTag("Player"))
        {
            print("B");
            dialogue.SetActive(true);
            exclamation.SetActive(false);
            //trigger.enabled = false; // Disable the trigger after the player enters
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialogue.SetActive(false);
            exclamation.SetActive(true);
            //trigger.enabled = true; // Re-enable the trigger when the player exits
        }
    }
}
