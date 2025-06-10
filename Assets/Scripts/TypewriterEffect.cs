using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    public float typeTime = 2f; // time in seconds to type the full text
    private TextMeshProUGUI textComponent;
    private string fullText;
    private Coroutine typingCoroutine;

    void Awake()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        fullText = textComponent.text;
        textComponent.text = ""; // Start empty
    }

    public void StartTyping()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        textComponent.text = "";
        foreach (char c in fullText)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(typeTime / fullText.Length);
        }
    }
}
