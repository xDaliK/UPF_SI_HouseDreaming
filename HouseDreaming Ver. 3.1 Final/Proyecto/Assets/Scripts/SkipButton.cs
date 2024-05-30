using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipButton : MonoBehaviour
{
    public float delayBeforeLoading = 10f;
    public string sceneToLoad = "EmptyHouse";

    private AudioSource audioSource;
    private Renderer renderer;
    private Color originalColor;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        renderer = GetComponent<Renderer>();
        originalColor = renderer.material.color;


        StartCoroutine(LoadSceneAfterDelay(delayBeforeLoading));
    }

    IEnumerator LoadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneToLoad);
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

