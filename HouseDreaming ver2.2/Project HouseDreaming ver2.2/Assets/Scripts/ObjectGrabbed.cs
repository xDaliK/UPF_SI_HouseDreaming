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
        private float releaseHeight = 0.5f; // Altura a la que se suelta el objeto

        public GridManagerScript gridManager;

        private float positionplayerx = -9999.9f;
        private float positionplayery = -9999.9f;
        private float positionplayerz = -9999.9f;
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
                //Debug.Log(renderer.material.color);
                renderer.material.color = originalColor * 0.5f; //el material cambia de color en el inspector pero no en la renderización TODO
                //Debug.Log(renderer.material.color);

                playerTransform = other.transform;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.name.StartsWith("Player"))
            {
                renderer.material.color = originalColor;
                positionplayerx = playerTransform.position.x;
                positionplayery = playerTransform.position.y;
                positionplayerz = playerTransform.position.z;
                playerTransform = null;
                timeOverObject = 0f; // Resetea el contador cuando el jugador sale del objeto
            }
        }

        void Update()
        {
            if (playerTransform != null && !isHeld) // Solo cuenta el tiempo si el objeto no está siendo agarrado
            {
                timeOverObject += Time.deltaTime; // Incrementa el contador de tiempo
                if (timeOverObject >= 2f) // Si el jugador ha estado sobre el objeto durante 2 segundos
                {
                    isHeld = true; // Agarra el objeto
                    // Calcula el desplazamiento entre el jugador y el objeto
                    objectHeld = gameObject.tag;
                    offset = transform.position - playerTransform.position;
                    timeOverObject = 0f; // Resetea el contador cuando el objeto es agarrado
                }
            }

            if (isHeld)
            {
                // Si el jugador está a una altura de 1.8 metros, suelta el objeto
                if (positionplayery >= releaseHeight)
                {
                    isHeld = false;
                    Debug.Log("Soltado1");


                    // Mueve el mueble a la posición de la celda más cercana
                    transform.position = transform.position;
                    Debug.Log("Soltado2");

                    Debug.Log(gameObject.tag);
                    // Comprueba si el objeto es un sofá y está en la posición correcta
                    if (objectHeld == "sofa" && Mathf.Abs(transform.position.x - 48.7f) <= 5 && Mathf.Abs(transform.position.z - 21.8f) <= 5)
                    {
                        Debug.Log("Sofá correctamente colocado");
                    }
                    else if (LayerMask.LayerToName(gameObject.layer) == "sofa")
                    {
                        Debug.Log("Sofá mal colocado");
                    }
                }
                else
                {
                    // Mueve el objeto a la posición del jugador más el desplazamiento
                    Vector3 newPosition = playerTransform.position + offset;
                    newPosition.y = transform.position.y; // Mantén la altura constante
                    transform.position = newPosition;
                }
            }

        }
    }

}