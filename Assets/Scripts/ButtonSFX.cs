using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSFX : MonoBehaviour
{
    public AudioClip clickSound;
    public float volume = 1f;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (AudioManager.Instance != null && clickSound != null)
            {
                AudioManager.Instance.PlaySFX(clickSound, volume);
            }
        });
    }
}
