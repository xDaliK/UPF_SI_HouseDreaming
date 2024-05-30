using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InteractiveSystemsTemplate
{
    public class ObjectGrabbed : MonoBehaviour
    {
        private AudioSource audioSource;
        private Renderer renderer;
        private Color originalColor;
        private Transform playerTransform;
        private bool isHeld = false;
        private Vector3 offset;
        private float timeOverObject = 0f; // Tiempo que el jugador ha estado sobre el objeto
        private float releaseHeight = 0.2f; // Altura a la que se suelta el objeto

        private string objectHeld;

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
                renderer.material.color = originalColor * 0.5f; // Cambia el color del material
                playerTransform = other.transform;
                Debug.Log("Player ha entrado en el trigger.");
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.name.StartsWith("Player"))
            {
                renderer.material.color = originalColor;
                playerTransform = null;
                timeOverObject = 0f; // Resetea el contador cuando el jugador sale del objeto
                Debug.Log("Player ha salido del trigger.");
            }
        }

        void Update()
        {
            if (playerTransform != null)
            {
                if (!isHeld) // Solo cuenta el tiempo si el objeto no está siendo agarrado
                {
                    timeOverObject += Time.deltaTime; // Incrementa el contador de tiempo
                    Debug.Log("Tiempo sobre objeto: " + timeOverObject);
                }
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
                // Si el jugador está a una altura de releaseHeight o más, suelta el objeto
                float positionplayery = playerTransform.position.y;
                Debug.Log("Altura del jugador: " + positionplayery);

                if (positionplayery >= releaseHeight)
                {
                    isHeld = false;
                    Debug.Log("Soltado1");

                    // Realiza la comprobación de posición solo cuando el objeto se suelta
                    if (objectHeld == "sofa" && Mathf.Abs(transform.position.x - 48.7f) <= 5 && Mathf.Abs(transform.position.z - 21.8f) <= 5)
                    {
                        Debug.Log("Sofá correctamente colocado");
                    }
                    else if (objectHeld == "sofa")
                    {
                        Debug.Log("Sofá mal colocado");
                    }

                    Debug.Log("Tag del objeto: " + gameObject.tag);
                }
                else
                {
                    // Mueve el objeto a la posición del jugador más el desplazamiento, manteniendo la altura constante
                    Vector3 newPosition = playerTransform.position + offset;
                    newPosition.y = transform.position.y; // Mantén la altura constante
                    transform.position = newPosition;
                    Debug.Log("Posición del objeto actualizada a: " + newPosition);
                }
            }
        }
    }
}
