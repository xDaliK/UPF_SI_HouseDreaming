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
                Debug.Log(renderer.material.color);
                renderer.material.color = originalColor * 0.5f; //el material cambia de color en el inspector pero no en la renderización TODO
                Debug.Log(renderer.material.color);

                playerTransform = other.transform;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.name.StartsWith("Player"))
            {
                renderer.material.color = originalColor;
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
                    offset = transform.position - playerTransform.position;
                    timeOverObject = 0f; // Resetea el contador cuando el objeto es agarrado
                }
            }

            if (isHeld)
            {
                // Si el jugador está a una altura de 1.8 metros, suelta el objeto
                Debug.Log(transform.position);
                Vector3 closestGridPosition1 = gridManager.FindClosestGridPosition(transform.position);
                Debug.Log(closestGridPosition1);
                if (playerTransform.position.y >= releaseHeight)
                {
                    isHeld = false;

                    // Encuentra la celda de la cuadrícula más cercana
                    Debug.Log("find -1");
                    Vector3 closestGridPosition2 = gridManager.FindClosestGridPosition(transform.position);
                    Debug.Log(closestGridPosition2);

                    // Mueve el mueble a la posición de la celda más cercana
                    transform.position = transform.position; //deberia ser hacia closestGridPostion2 TODO (closestGridPosition2 siempre es 0,0,0).
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
