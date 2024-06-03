using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinController : MonoBehaviour
{
    // Static reference to the game controller
    public static WinController instance;

    // Dictionary to keep track of whether each furniture item is correctly placed
    public Dictionary<string, bool> isCorrectlyPlaced = new Dictionary<string, bool>();
    private GameObject[] winTextObjects;

    // AudioSource to play when the player wins
    public AudioSource winAudio;
    private bool hasWon = false;

    // Initializes the game controller, the dictionary, the win text objects, and the win audio.
    void Awake()
    {
        // Singleton pattern for the game controller
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        // Initialize the dictionary of furnitures of your deram house
        isCorrectlyPlaced["sofa"] = false;
        isCorrectlyPlaced["bed"] = false;
        isCorrectlyPlaced["plant"] = false;
        isCorrectlyPlaced["cabinet"] = false;
        isCorrectlyPlaced["bookcase"] = false;
        isCorrectlyPlaced["television"] = false;

        // Find all win text objects and deactivate them (the win screen)
        winTextObjects = GameObject.FindGameObjectsWithTag("winText");
        foreach (GameObject winTextObject in winTextObjects)
        {
            winTextObject.SetActive(false);
        }

        // Get the AudioSource component of the win screen
        winAudio = GetComponent<AudioSource>();
    }

    // Checks if the player has won and, if so, activates the win text objects (win screen), plays the win audio, and loads the next scene.
    void Update()
    {
        if ((!hasWon) && AllObjectsPlacedCorrectly())
        {
            Debug.Log("You have won!");
            foreach (GameObject winTextObject in winTextObjects)
            {
                winTextObject.SetActive(true);
            }
            winAudio.Play();
            hasWon = true;
            Invoke("LoadScene", 4f);
        }
    }

    // AllObjectsPlacedCorrectly checks if all furniture items are correctly placed.
    public bool AllObjectsPlacedCorrectly()
    {
        foreach (KeyValuePair<string, bool> entry in isCorrectlyPlaced)
        {
            if (!entry.Value)
            {
                return false;
            }
        }
        return true;
    }

    // LoadScene is called to load the "SceneRestartTitle" scene, after 4 seconds.
    void LoadScene()
    {
        SceneManager.LoadScene("SceneRestartTitle");
    }
}
