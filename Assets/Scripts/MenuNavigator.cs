using System.Collections;
using UnityEngine;

public class MenuNavigator : MonoBehaviour
{
    public GameObject popupPanel; // Reference to the popup panel GameObject

    private void Start()
    {
        // Ensure the popup panel is inactive at the start
        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
        }
    }
    
    public void Play()
    {
        // Load the game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("IntroCutscene");
    }

    public void Quit()
    {
        StartCoroutine(WaitAndQuit());
    }

    IEnumerator WaitAndQuit()
    {
        yield return new WaitForSeconds(0.25f);
        Application.Quit();
        // If running in the editor, stop playing
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void SkipCutscene()
    {
        // Load the game scene directly, skipping the cutscene
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }

    public void OpenPopup()
    {
        popupPanel.SetActive(true); // Activate the popup panel
    }

    public void ClosePopup()
    {
        popupPanel.SetActive(false); // Deactivate the popup panel
    }
}
