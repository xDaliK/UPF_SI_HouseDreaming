using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;
using System;

public class QuitButton : MonoBehaviour
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


            Invoke("ExitGame", 1f);

        }
    }

    void ExitGame()
    {

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else

        Application.Quit();
#endif


    }
}


