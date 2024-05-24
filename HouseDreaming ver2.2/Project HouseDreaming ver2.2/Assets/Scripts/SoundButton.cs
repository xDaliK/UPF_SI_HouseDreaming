using UnityEngine;

public class SoundButton : MonoBehaviour
{
    public Material newMaterial;

    private bool isMuted = false;
    private Renderer renderer;
    private Material originalMaterial;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        originalMaterial = renderer.material;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.StartsWith("Player"))
        {
            ToggleMuteAndMaterial();
        }
    }

    public void ToggleMuteAndMaterial()
    {
        isMuted = !isMuted;
        AudioListener.volume = isMuted ? 0 : 1;


        if (isMuted)
        {
            renderer.material = newMaterial;
        }
        else
        {
            renderer.material = originalMaterial;
        }
    }
}
