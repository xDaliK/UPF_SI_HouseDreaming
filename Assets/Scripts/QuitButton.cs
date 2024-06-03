using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;
using System;
using static System.Net.Mime.MediaTypeNames;

public class QuitButton : MonoBehaviour
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

    // Changes the color of the Renderer, plays the AudioSource, and invokes the ExitGame method.
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.StartsWith("Player"))
        {
            renderer.material.color = originalColor * 0.5f;
            audioSource.Play();
            Invoke("ExitGame", 1f);
        }
    }

    // Behaves differently depending on whether the game is running in the Unity editor or in the built HouseDreaming application.
    void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
