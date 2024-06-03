using UnityEngine;

public class SoundButton : MonoBehaviour
{
    // Public variable to hold a reference to the new material.
    public Material newMaterial;

    // Private variables to hold the mute state, a reference to the Renderer component, and the original material of the Renderer.
    private bool isMuted = false;
    private Renderer renderer;
    private Material originalMaterial;

    // Initializes the Renderer and original material.
    void Start()
    {
        renderer = GetComponent<Renderer>();
        originalMaterial = renderer.material;
    }

    // Toggles the mute state and the material of the Renderer.
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.StartsWith("Player"))
        {
            ToggleMuteAndMaterial();
        }
    }

    // ToggleMuteAndMaterial is called to toggle the mute state and the material of the Renderer.
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
