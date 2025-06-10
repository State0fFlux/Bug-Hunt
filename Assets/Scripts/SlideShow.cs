using UnityEngine;
using System.Collections.Generic;

// this script expects to be attached to a GameObject with child GameObjects representing slides
// each child GameObject should be a canvas slide that can be activated or deactivated
public class SlideShow : MonoBehaviour
{

    public GameObject sfx; // reference to the GameObject with the AudioSource component
    public AudioClip nextSound;
    public AudioClip endSound;
    private List<GameObject> slides = new List<GameObject>();
    private int currentSlide = 0;
    private AudioSource audio;

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
        audio = sfx.GetComponent<AudioSource>();
        ShowSlide(0);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            NextSlide();
        }
    }

    void ShowSlide(int index)
    {
        for (int i = 0; i < slides.Count; i++)
        {
            slides[i].SetActive(i == index);
        }
    }

    public void NextSlide()
    {
        currentSlide++;
        if (currentSlide < slides.Count)
        {
            audio.PlayOneShot(nextSound); // Play slide change sound
            ShowSlide(currentSlide);
        }
        else
        {
            audio.PlayOneShot(endSound); // Play end sound
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
        }
    }
}
