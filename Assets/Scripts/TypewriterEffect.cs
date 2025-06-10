using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    public float typeTime = 2f; // time in seconds to type the full text
    public bool isQuote = true; // whether the text is a quote
    private TextMeshProUGUI textComponent;
    private string baseText;
    private string addedText;
    private Coroutine typingCoroutine;

    // Expects text to be formatted as "StartText|AddedText", or "AddedText" if no start text is provided
    void Awake()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        string[] textParts = textComponent.text.Split('|');
        baseText = textParts.Length > 1 ? textParts[0] : "";
        addedText = textParts.Length > 1 ? textParts[1] : textParts[0];
        textComponent.text = baseText;
    }

    public void StartTyping()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        int totalChars = addedText.Length;
        float delay = typeTime / totalChars;

        for (int i = 0; i <= totalChars; i++)
        {
            string typedPortion = addedText.Substring(0, i);
            if (isQuote)
                textComponent.text = $"\"{baseText + typedPortion}\"";
            else
                textComponent.text = baseText + typedPortion;

            yield return new WaitForSeconds(delay);
        }
    }
}
