using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipButton : MonoBehaviour
{
    // Public variables to hold the delay before loading and the name of the scene to load.
    public float delayBeforeLoading = 10f;
    public string sceneToLoad = "EmptyHouse";

    // Private variables to hold references to the AudioSource and Renderer components, and the original color of the Renderer.
    private AudioSource audioSource;
    private Renderer renderer;
    private Color originalColor;

    // Initializes the AudioSource, Renderer, and original color, and starts the coroutine to load the scene after a delay.
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        renderer = GetComponent<Renderer>();
        originalColor = renderer.material.color;
        StartCoroutine(LoadSceneAfterDelay(delayBeforeLoading));
    }

    // Loads the scene after a specified delay.
    IEnumerator LoadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneToLoad);
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

    // LoadScene is called to load the "EmptyHouse" scene.
    void LoadScene()
    {
        SceneManager.LoadScene("EmptyHouse");
    }
}
