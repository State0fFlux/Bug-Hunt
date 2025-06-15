using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

// this script expects to be attached to a GameObject with child GameObjects representing slides
// each child GameObject should be a canvas slide that can be activated or deactivated
public class SlideShow : MonoBehaviour
{
    public AudioClip nextSound;
    public AudioClip endSound;
    private List<GameObject> slides = new List<GameObject>();
    private int currentSlide = 0;

    void Awake()
    {
        // Get all children in hierarchy order
        for (int i = 0; i < transform.childCount; i++)
        {
            slides.Add(transform.GetChild(i).gameObject);
        }
    }

    void Start()
    {
        ShowSlide(0);
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            NextSlide();
        }
    }

    void ShowSlide(int index)
    {
        for (int i = 0; i < slides.Count; i++)
        {
            slides[i].SetActive(i == index);
            if (i == index)
            {
                slides[i].GetComponentInChildren<TypewriterEffect>()?.StartTyping(); // Start typing effect if available
            }
        }
    }

    public void NextSlide()
    {
        currentSlide++;
        if (currentSlide < slides.Count)
        {
            AudioManager.Instance.PlaySFX(nextSound); // Play next sound
            ShowSlide(currentSlide);
        }
        else
        {
            AudioManager.Instance.PlaySFX(endSound); // Play end sound
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
        }
    }
}
