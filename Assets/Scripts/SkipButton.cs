using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SkipButton : MonoBehaviour
{

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

            audioSource.Play();


            Invoke("LoadScene", 2f);
        }
    }

    void LoadScene()
    {
        SceneManager.LoadScene("EmptyHouse");
    }
}
