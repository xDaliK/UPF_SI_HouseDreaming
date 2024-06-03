using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMusicScript : MonoBehaviour
{
    // Private variables to hold the state of the music and a reference to the AudioSource component.
    private static bool created = false;
    private AudioSource audioSource;

    // Ensures that the music continues to play across different scenes.
    void Awake()
    {
        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }

        audioSource = GetComponent<AudioSource>();
    }

    // Adjusts the volume of the music depending on the current scene.
    void Update()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        // Adjusts the volume depending on the scene
        if (sceneName == "SceneRestartTitle" || sceneName == "SceneTitle")
        {
            audioSource.volume = 0.125f;
        }
        else
        {
            audioSource.volume = 0.025f;
        }
    }
}
