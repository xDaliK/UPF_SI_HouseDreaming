using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMusicScript : MonoBehaviour
{
    private static bool created = false;
    private AudioSource audioSource;

    void Awake()
    {
        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {

        string sceneName = SceneManager.GetActiveScene().name;

        // Ajusta el volumen dependiendo de la escena
        if (sceneName == "SceneRestartTitle")
        {
            audioSource.volume = 0.1f;
        }
        else
        {
            audioSource.volume = 0.125f;
        }
    }
}
