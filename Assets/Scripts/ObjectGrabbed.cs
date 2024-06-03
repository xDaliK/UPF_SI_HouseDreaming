using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ObjectGrabbed : MonoBehaviour
{
    // Private variables to hold references to the AudioSource and Renderer components, the original color of the Renderer, the Transform of the player, and the offset between the player and the object.
    private AudioSource audioSource;
    private Renderer renderer;
    private Color originalColor;
    private Transform playerTransform;
    private bool isHeld = false;
    private Vector3 offset;
    private float timeOverObject = 0f; // Time that the player has been over the object
    private float releaseHeight = 1.25f; // Height at which the object is released
    private string objectHeld;
    private Vector3 lastKnownPlayerPosition;

    // Private variables to hold references to the win text objects, the interacting player, the number of players interacting, and the SpawnFurnitures script.
    private GameObject[] winTextObjects;
    private GameObject interactingPlayer = null;
    private int playersInteracting = 0;
    private SpawnFurnitures spawnFurnitures;

    // Public variables to hold references to the correct placement audio, incorrect placement audio, loading audio, and held audio.
    public AudioSource correctPlacementAudio;
    public AudioSource incorrectPlacementAudio;
    public AudioSource loadingAudio;
    public AudioSource heldAudio;

    // Initializes the AudioSource, Renderer, original color, win text objects, and audio sources.
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        renderer = GetComponent<Renderer>();
        originalColor = renderer.material.color;
        GameObject[] winTextObjects = GameObject.FindGameObjectsWithTag("winText");

        AudioSource[] audioSources = GetComponents<AudioSource>();
        incorrectPlacementAudio = audioSources[0];
        correctPlacementAudio = audioSources[1];
        loadingAudio = audioSources[2];
        heldAudio = audioSources[3];

    }


    // Finds the SpawnFurnitures script in the scene and stores the reference.
    void Start()
    {

        spawnFurnitures = GameObject.FindObjectOfType<SpawnFurnitures>();
    }


    // Changes the color of the Renderer when a player interacts with the furnitre (controlling the number of players on it),
    // plays the AudioSource, and invokes the LoadScene method.
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.StartsWith("Player") && !WinController.instance.isCorrectlyPlaced[gameObject.tag])
        {

            if (interactingPlayer != null) return;
            interactingPlayer = other.gameObject;
            Debug.Log("playerInteracting: " + interactingPlayer);
            if ((gameObject.tag == "sofa" || gameObject.tag == "bed") && !isHeld)
            {

                //playersInteracting++; // Controls the number of players interacting with the bed or sofa (to test)

                playersInteracting = 2; //for debug
                if (playersInteracting >= 2 && !WinController.instance.isCorrectlyPlaced[gameObject.tag])
                {
                    loadingAudio.Play();
                    renderer.material.color = originalColor * 0.75f;
                    playerTransform = other.transform;
                    Debug.Log("Player ha entrado en el trigger.");
                    lastKnownPlayerPosition = playerTransform.position;
                }
            }
            else if (!isHeld && !WinController.instance.isCorrectlyPlaced[gameObject.tag])
            {
                loadingAudio.Play();
                renderer.material.color = originalColor * 0.75f;
                playerTransform = other.transform;
                Debug.Log("Player ha entrado en el trigger.");
                lastKnownPlayerPosition = playerTransform.position;
            }
        }
    }

    // Restores the original color of the Renderer, controls the players left interacting with the furnitures and the last position of the height registered.
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.StartsWith("Player"))
        {
            if (other.gameObject != interactingPlayer) return;
            interactingPlayer = null;
            Debug.Log("playerInteracting: " + interactingPlayer);
            if (gameObject.tag == "sofa" || gameObject.tag == "bed")
            {
                //playersInteracting--; // Decrease the number of players interacting with the sofa or bed (to test)
                playersInteracting = 0; //for debug
                if (playersInteracting == 0)
                {
                    renderer.material.color = originalColor;
                    timeOverObject = 0f; // Reset the counter when the players exit the furniture
                    Debug.Log("Player ha salido del trigger.");
                    if (playerTransform.position != null)
                        lastKnownPlayerPosition = playerTransform.position;
                    playerTransform = null;

                }
            }
            else
            {
                renderer.material.color = originalColor;
                timeOverObject = 0f; // Reset the counter when the players exit the furniture
                Debug.Log("Player ha salido del trigger.");
                if (playerTransform.position != null)
                    lastKnownPlayerPosition = playerTransform.position;
                playerTransform = null;

            }
        }
    }

    // Checks if the player has won and, if so, activates the win text objects, plays the win audio, and loads the next scene.
    // Also, it checks if the player is holding an object and, if so, whether the player is at a certain height or has pressed a certain key (debug).
    // If either condition is met, the object is released and its position is checked to see if it has been correctly placed.
    void Update()
    {
        Debug.Log("Cogido: " + isHeld);

        string dictionaryString = "";
        foreach (KeyValuePair<string, bool> entry in WinController.instance.isCorrectlyPlaced)
        {
            dictionaryString += "Key: " + entry.Key + " Value: " + entry.Value + "\n";
        }

        //Debug of the state of the furnitures
        Debug.Log(dictionaryString);


        if (playerTransform != null && !isHeld) // Only count the time if the object is not being held
        {
            if (WinController.instance.isCorrectlyPlaced[gameObject.tag]) return;

            timeOverObject += Time.deltaTime; // Increment the time counter

            Debug.Log("Time over object: " + timeOverObject);

            if (timeOverObject >= 1.5f) // If the player has been over the object for 2 seconds
            {

                heldAudio.Play();
                isHeld = true; // Grab the object
                // Calculate the offset between the player and the object
                objectHeld = gameObject.tag; //Name of the furniture grabbed ( its tag )
                offset = transform.position - playerTransform.position;
                timeOverObject = 0f; // Reset the counter when the object is grabbed
                Debug.Log("Object grabbed: " + objectHeld);
            }
        }

        if (isHeld)
        {
            // If the player is at a height of 1.25 meters, release the object
            Debug.Log("Height: " + lastKnownPlayerPosition.y);
            if (lastKnownPlayerPosition.y >= releaseHeight || Input.GetKeyDown(KeyCode.Q)) // Q key for debugging
            {
                isHeld = false;

                Debug.Log("Object Released");

                // Perform the position check only when the object is released. The WinController instance of the dictionary controls
                //the correct placement of all the furnitures and when they are in the correct place (with a little of margin) it puts the furniture to true.
                if (objectHeld == "sofa" && Mathf.Abs(transform.position.x - 30.02f) <= 5 && Mathf.Abs(transform.position.z - 21.63f) <= 5)
                {
                    Debug.Log("Sofa correctly placed");
                    WinController.instance.isCorrectlyPlaced["sofa"] = true;
                    correctPlacementAudio.Play();
                }
                else if (objectHeld == "sofa")
                {
                    Debug.Log("Sofa incorrectly placed");
                    incorrectPlacementAudio.Play();
                }

                if (objectHeld == "bed" && Mathf.Abs(transform.position.x - 84.17352f) <= 7.5f && Mathf.Abs(transform.position.z - 54.87921f) <= 7.5f)
                {
                    Debug.Log("Bed correctly placed");
                    WinController.instance.isCorrectlyPlaced["bed"] = true;
                    correctPlacementAudio.Play();
                }
                else if (objectHeld == "bed")
                {
                    Debug.Log("Bed incorrectly placed");
                    incorrectPlacementAudio.Play();
                }

                if (objectHeld == "plant" && Mathf.Abs(transform.position.x - (35.7f)) <= 10 && Mathf.Abs(transform.position.z - 83.16f) <= 10)
                {
                    Debug.Log("Planta correctamente colocada");
                    WinController.instance.isCorrectlyPlaced["plant"] = true;
                    correctPlacementAudio.Play();
                }
                else if (objectHeld == "plant")
                {
                    Debug.Log("Planta mal colocada");
                    incorrectPlacementAudio.Play();
                }

                if (objectHeld == "cabinet" && Mathf.Abs(transform.position.x - (51.3f)) <= 5 && Mathf.Abs(transform.position.z - 52.72921f) <= 5)
                {
                    Debug.Log("Cabinet correctamente colocado");
                    WinController.instance.isCorrectlyPlaced["cabinet"] = true;
                    correctPlacementAudio.Play();
                }
                else if (objectHeld == "cabinet")
                {
                    Debug.Log("Cabinet mal colocado");
                    incorrectPlacementAudio.Play();
                }

                if (objectHeld == "bookcase" && Mathf.Abs(transform.position.x - (19.77352f)) <= 5 && Mathf.Abs(transform.position.z - 47.37921f) <= 5)
                {
                    Debug.Log("Bookcase correctamente colocado");
                    WinController.instance.isCorrectlyPlaced["bookcase"] = true;
                    correctPlacementAudio.Play();
                }
                else if (objectHeld == "bookcase")
                {
                    Debug.Log("Bookcase mal colocado");
                    incorrectPlacementAudio.Play();

                }

                if (objectHeld == "television" && Mathf.Abs(transform.position.x - (32.5f)) <= 7.5f && Mathf.Abs(transform.position.z - 23.4f) <= 7.5f)
                {
                    Debug.Log("Television correctamente colocado");
                    WinController.instance.isCorrectlyPlaced["television"] = true;
                    correctPlacementAudio.Play();


                }
                else if (objectHeld == "television")
                {
                    Debug.Log("Television mal colocado");

                    incorrectPlacementAudio.Play();
                }
            }

            // Move the object to the player's position plus the offset
            else if (playerTransform != null)
            {
                Vector3 newPosition = playerTransform.position + offset;
                newPosition.y = transform.position.y; // Keep the height constant
                transform.position = newPosition;
                Debug.Log("Posición del objeto actualizada a: " + newPosition);
            }
        }






        // Debug mode: if the E key is pressed, it automatically places the furniture in the correct position and puts the WinController instance of the
        // dictionary to true.
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameObject currentFurniture = spawnFurnitures.currentFurniture;
            if (currentFurniture.tag == "sofa" && !WinController.instance.isCorrectlyPlaced["sofa"])
            {
                currentFurniture.transform.position = new Vector3(30.02f, transform.position.y, 21.63f);
                Debug.Log("Sofá colocado correctamente (modo de depuración)");
                WinController.instance.isCorrectlyPlaced["sofa"] = true;
                correctPlacementAudio.Play();
            }
            else if (currentFurniture.tag == "bed" && !WinController.instance.isCorrectlyPlaced["bed"])
            {
                currentFurniture.transform.position = new Vector3(84.17352f, transform.position.y, 54.87921f);
                Debug.Log("Cama colocada correctamente (modo de depuración)");
                WinController.instance.isCorrectlyPlaced["bed"] = true;
                correctPlacementAudio.Play();
            }
            else if (currentFurniture.tag == "plant" && !WinController.instance.isCorrectlyPlaced["plant"])
            {
                currentFurniture.transform.position = new Vector3(35.7f, transform.position.y, 83.16f);
                Debug.Log("Planta colocada correctamente (modo de depuración)");
                WinController.instance.isCorrectlyPlaced["plant"] = true;
                correctPlacementAudio.Play();
            }
            else if (currentFurniture.tag == "cabinet" && !WinController.instance.isCorrectlyPlaced["cabinet"])
            {
                currentFurniture.transform.position = new Vector3(51.3f, transform.position.y, 52.72921f);
                Debug.Log("Cabinet colocado correctamente (modo de depuración)");
                WinController.instance.isCorrectlyPlaced["cabinet"] = true;
                correctPlacementAudio.Play();
            }
            else if (currentFurniture.tag == "bookcase" && !WinController.instance.isCorrectlyPlaced["bookcase"])
            {
                currentFurniture.transform.position = new Vector3(19.77352f, transform.position.y, 47.37921f);
                Debug.Log("Bookcase colocado correctamente (modo de depuración)");
                WinController.instance.isCorrectlyPlaced["bookcase"] = true;
                correctPlacementAudio.Play();
            }
            else if (currentFurniture.tag == "television" && !WinController.instance.isCorrectlyPlaced["television"])
            {
                currentFurniture.transform.position = new Vector3(32.5f, transform.position.y, 23.4f);
                Debug.Log("Television colocada correctamente (modo de depuración)");
                WinController.instance.isCorrectlyPlaced["television"] = true;
                correctPlacementAudio.Play();
            }
        }
    }
}


