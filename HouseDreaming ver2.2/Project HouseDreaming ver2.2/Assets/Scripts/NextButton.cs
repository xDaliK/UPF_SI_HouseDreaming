using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class NextButton : MonoBehaviour
{
    public SpawnFurnitures furnitureSpawner;

    private AudioSource audioSource;
    private Renderer renderer;
    private Color originalColor;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        renderer = GetComponent<Renderer>();
        originalColor = renderer.material.color;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.StartsWith("Player"))
        {
            renderer.material.color = originalColor * 0.5f;
            furnitureSpawner.SpawnFurniture();
            audioSource.Play();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.StartsWith("Player"))
        {
            renderer.material.color = originalColor;
        }
    }
}
