using UnityEngine;

public class MenuNavigator : MonoBehaviour
{
    public void Play()
    {
        // Load the game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("CutsceneScene");
    }

    public void Quit()
    {
        // Quit the application
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
}
