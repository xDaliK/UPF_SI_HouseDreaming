using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour
{
    // Private variables to hold references to the AudioSource and Renderer components, and the original color of the Renderer.
    private AudioSource audioSource;
    private Renderer renderer;
    private Color originalColor;

    // Initializes the AudioSource, Renderer, and original color.
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        renderer = GetComponent<Renderer>();
        originalColor = renderer.material.color;
    }

    // Changes the color of the Renderer, plays the AudioSource, and invokes the LoadScene method.
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.StartsWith("Player"))
        {
            renderer.material.color = originalColor * 0.5f;
            audioSource.Play();
            Invoke("LoadScene", 2f);
        }
    }

    // LoadScene is called to load the "SceneRestartTitle" scene.
    void LoadScene()
    {
        SceneManager.LoadScene("SceneRestartTitle");
    }
}
