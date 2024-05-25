using System.Collections;
using System.Collections.Generic;
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
    private float releaseHeight = 5f; // Altura a la que se suelta el objeto
    private string objectHeld;
    private Vector3 lastKnownPlayerPosition;

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
            renderer.material.color = originalColor * 0.75f;
            playerTransform = other.transform;
            Debug.Log("Player ha entrado en el trigger.");
            lastKnownPlayerPosition = playerTransform.position;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.StartsWith("Player"))
        {
            renderer.material.color = originalColor;
            timeOverObject = 0f; // Resetea el contador cuando el jugador sale del objeto
            Debug.Log("Player ha salido del trigger.");
            lastKnownPlayerPosition = playerTransform.position;
            playerTransform = null;
        }
    }

    void Update()
    {
        Debug.Log("Cogido: " + isHeld);

        if (playerTransform != null && !isHeld) // Solo cuenta el tiempo si el objeto no est� siendo agarrado
        {
            timeOverObject += Time.deltaTime; // Incrementa el contador de tiempo
            Debug.Log("Tiempo sobre objeto: " + timeOverObject);
            if (timeOverObject >= 2f) // Si el jugador ha estado sobre el objeto durante 2 segundos
            {
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
            if (lastKnownPlayerPosition.y >= releaseHeight)
            {
                isHeld = false;

                Debug.Log("Soltado Objeto");

                // Realiza la comprobación de posición solo cuando el objeto se suelta
                if (objectHeld == "sofa" && Mathf.Abs(transform.position.x - 48.7f) <= 5 && Mathf.Abs(transform.position.z - 21.8f) <= 5)
                {
                    Debug.Log("Sofá correctamente colocado");
                }
                else if (objectHeld == "sofa")
                {
                    Debug.Log("Sofá mal colocado");
                }

                if (objectHeld == "bed" && Mathf.Abs(transform.position.x - 84.17352f) <= 5 && Mathf.Abs(transform.position.z - 54.87921f) <= 5)
                {
                    Debug.Log("Cama correctamente colocada");
                }
                else if (objectHeld == "bed")
                {
                    Debug.Log("Cama mal colocada");
                }

                if (objectHeld == "plant" && Mathf.Abs(transform.position.x - (35.7f)) <= 10 && Mathf.Abs(transform.position.z - 83.16f) <= 10)
                {
                    Debug.Log("Planta correctamente colocada");
                }
                else if (objectHeld == "plant")
                {
                    Debug.Log("Planta mal colocada");
                }

                if (objectHeld == "cabinet" && Mathf.Abs(transform.position.x - (51.3f)) <= 5 && Mathf.Abs(transform.position.z - 52.72921f) <= 5)
                {
                    Debug.Log("Cabinet correctamente colocado");
                }
                else if (objectHeld == "cabinet")
                {
                    Debug.Log("Cabinet mal colocado");
                }

                if (objectHeld == "bookcase" && Mathf.Abs(transform.position.x - (19.77352f)) <= 5 && Mathf.Abs(transform.position.z - 47.37921f) <= 5)
                {
                    Debug.Log("Bookcase correctamente colocado");
                }
                else if (objectHeld == "bookcase")
                {
                    Debug.Log("Bookcase mal colocado");
                }

                if (objectHeld == "television" && Mathf.Abs(transform.position.x - (32.5f)) <= 5 && Mathf.Abs(transform.position.z - 23.4f) <= 5)
                {
                    Debug.Log("Television correctamente colocado");
                }
                else if (objectHeld == "television")
                {
                    Debug.Log("Television mal colocado");
                }
            }

            else
            {
                // Mueve el objeto a la posici�n del jugador m�s el desplazamiento
                Vector3 newPosition = playerTransform.position + offset;
                newPosition.y = transform.position.y; // Mant�n la altura constante
                transform.position = newPosition;
                Debug.Log("Posición del objeto actualizada a: " + newPosition);
            }
        }
    }
}
