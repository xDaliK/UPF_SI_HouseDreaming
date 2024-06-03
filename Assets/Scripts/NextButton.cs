using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class NextButton : MonoBehaviour
{
    // Public variable to hold a reference to the SpawnFurnitures script.
    public SpawnFurnitures furnitureSpawner;

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

    // Cchanges the color of the Renderer, spawns the furniture, and plays the AudioSource.
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.StartsWith("Player"))
        {
            renderer.material.color = originalColor * 0.5f;
            furnitureSpawner.SpawnFurniture();
            audioSource.Play();
        }
    }

    // Rrestores the original color of the Renderer on exit.
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.StartsWith("Player"))
        {
            renderer.material.color = originalColor;
        }
    }
}
