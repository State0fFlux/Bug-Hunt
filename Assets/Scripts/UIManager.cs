using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    private TextMeshProUGUI inventoryText;
    private string[] bugs = { "Caterpillar", "Ladybug", "Snail", "Spider" };

    void Awake()
    {
        inventoryText = transform.Find("InventoryText").GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        Player.OnInventoryUpdate += UpdateText;
    }

    private void OnDisable()
    {
        Player.OnInventoryUpdate -= UpdateText;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void UpdateText()
    {
        inventoryText.text = "";
        foreach (string bug in bugs)
        {
            print(bug);
            inventoryText.text += bug + "s: " + Player.inventory[bug] + "/" + Player.bugsNeeded + "\n";
        }
    }
}
