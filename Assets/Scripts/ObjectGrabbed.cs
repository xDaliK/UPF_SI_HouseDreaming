using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ObjectGrabbed : MonoBehaviour
{
    private AudioSource audioSource;
    private Renderer renderer;
    private Color originalColor;
    private Transform playerTransform;
    private bool isHeld = false;
    private Vector3 offset;
    private float timeOverObject = 0f; // Tiempo que el jugador ha estado sobre el objeto
    private float releaseHeight = 1.25f; // Altura a la que se suelta el objeto
    private string objectHeld;
    private Vector3 lastKnownPlayerPosition;

    private GameObject[] winTextObjects;

    private GameObject interactingPlayer = null;
    private int playersInteracting = 0;

    private SpawnFurnitures spawnFurnitures;



    public AudioSource correctPlacementAudio;
    public AudioSource incorrectPlacementAudio;
    public AudioSource loadingAudio;
    public AudioSource heldAudio;


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

    void Start()
    {
        // Encuentra el script SpawnFurnitures en la escena y almacena la referencia
        spawnFurnitures = GameObject.FindObjectOfType<SpawnFurnitures>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.StartsWith("Player") && !WinController.instance.isCorrectlyPlaced[gameObject.tag])
        {

            if (interactingPlayer != null) return;
            interactingPlayer = other.gameObject;
            Debug.Log("playerInteracting: " + interactingPlayer);
            if ((gameObject.tag == "sofa" || gameObject.tag == "bed") && !isHeld)
            {

                //playersInteracting++; // Incrementa el número de jugadores interactuando con el sofa o la cama

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

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.StartsWith("Player"))
        {
            if (other.gameObject != interactingPlayer) return;
            interactingPlayer = null;
            Debug.Log("playerInteracting: " + interactingPlayer);
            if (gameObject.tag == "sofa" || gameObject.tag == "bed")
            {
                //playersInteracting--; // Decrementa el número de jugadores interactuando con el sfoa o cama
                playersInteracting = 0; //for debug
                if (playersInteracting == 0)
                {
                    renderer.material.color = originalColor;
                    timeOverObject = 0f; // Resetea el contador cuando el jugador sale del objeto
                    Debug.Log("Player ha salido del trigger.");
                    if (playerTransform.position != null)
                        lastKnownPlayerPosition = playerTransform.position;
                    playerTransform = null;

                }
            }
            else
            {
                renderer.material.color = originalColor;
                timeOverObject = 0f; // Resetea el contador cuando el jugador sale del objeto
                Debug.Log("Player ha salido del trigger.");
                if (playerTransform.position != null)
                    lastKnownPlayerPosition = playerTransform.position;
                playerTransform = null;

            }
        }
    }

    void Update()
    {
        Debug.Log("Cogido: " + isHeld);

        string dictionaryString = "";
        foreach (KeyValuePair<string, bool> entry in WinController.instance.isCorrectlyPlaced)
        {
            dictionaryString += "Key: " + entry.Key + " Value: " + entry.Value + "\n";
        }

        //Imprime la cadena
        Debug.Log(dictionaryString);


        if (playerTransform != null && !isHeld) // Solo cuenta el tiempo si el objeto no est� siendo agarrado
        {
            if (WinController.instance.isCorrectlyPlaced[gameObject.tag]) return;

            timeOverObject += Time.deltaTime; // Incrementa el contador de tiempo

            Debug.Log("Tiempo sobre objeto: " + timeOverObject);

            if (timeOverObject >= 1.5f) // Si el jugador ha estado sobre el objeto durante 2 segundos
            {

                heldAudio.Play();
                isHeld = true; // Agarra el objeto
                // Calcula el desplazamiento entre el jugador y el objeto
                objectHeld = gameObject.tag;
                offset = transform.position - playerTransform.position;
                timeOverObject = 0f; // Resetea el contador cuando el objeto es agarrado
                Debug.Log("Objeto agarrado: " + objectHeld);
            }
        }

        if (isHeld)
        {

            // Si el jugador est� a una altura de 1.8 metros, suelta el objeto
            Debug.Log("Altura: " + lastKnownPlayerPosition.y);
            if (lastKnownPlayerPosition.y >= releaseHeight || Input.GetKeyDown(KeyCode.Q)) //tecla q para debugar
            {
                isHeld = false;

                Debug.Log("Soltado Objeto");


                // Realiza la comprobación de posición solo cuando el objeto se suelta
                if (objectHeld == "sofa" && Mathf.Abs(transform.position.x - 30.02f) <= 5 && Mathf.Abs(transform.position.z - 21.63f) <= 5)
                {
                    Debug.Log("Sofá correctamente colocado");
                    WinController.instance.isCorrectlyPlaced["sofa"] = true;
                    correctPlacementAudio.Play();
                }
                else if (objectHeld == "sofa")
                {
                    Debug.Log("Sofá mal colocado");
                    incorrectPlacementAudio.Play();
                }

                if (objectHeld == "bed" && Mathf.Abs(transform.position.x - 84.17352f) <= 7.5f && Mathf.Abs(transform.position.z - 54.87921f) <= 7.5f)
                {
                    Debug.Log("Cama correctamente colocada");
                    WinController.instance.isCorrectlyPlaced["bed"] = true;
                    correctPlacementAudio.Play();
                }
                else if (objectHeld == "bed")
                {
                    Debug.Log("Cama mal colocada");
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

            else if (playerTransform != null)
            {
                // Mueve el objeto a la posici�n del jugador m�s el desplazamiento
                Vector3 newPosition = playerTransform.position + offset;
                newPosition.y = transform.position.y; // Mant�n la altura constante
                transform.position = newPosition;
                Debug.Log("Posición del objeto actualizada a: " + newPosition);
            }
        }






        // Modo de depuración: si se pulsa la tecla E, coloca el objeto en la posición correcta automáticamente
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


