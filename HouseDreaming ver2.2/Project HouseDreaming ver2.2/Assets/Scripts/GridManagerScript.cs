using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace InteractiveSystemsTemplate

{



    public class GridManagerScript : MonoBehaviour
    {
        public int gridWidth = 10;
        public int gridHeight = 10;
        public float gridSize = 10;
        public Vector3[,] grid;

        // public GameObject gridCellPrefab; // El prefab de la celda de la cuadrícula




        void Awake()
        {
            // Inicializa la cuadrícula
            grid = new Vector3[gridWidth, gridHeight];
            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    Vector3 cellPosition = new Vector3(x * gridSize, 0, y * gridSize);
                    grid[x, y] = cellPosition;

                    // No necesitas instanciar un prefab para cada celda de la cuadrícula
                    // Instantiate(gridCellPrefab, cellPosition, Quaternion.identity);
                }
            }
        }





        public void DropObject(GameObject obj, Vector3 position)
        {
            // Encuentra el cuadrado más cercano en la cuadrícula
            Vector3 closestGridPosition = FindClosestGridPosition(position);

            // Mueve el objeto a la posición del cuadrado más cercano
            obj.transform.position = closestGridPosition;
        }


        public Vector3 FindClosestGridPosition(Vector3 position)
        {
            if (grid == null)
            {
                Debug.LogError("La cuadrícula no está inicializada");
                Awake();
            }


            //Debug.Log("holafind 0");
            Vector3 closestGridPosition = grid[0, 0];
            //Debug.Log("holafind 1");
            float closestDistance = Vector3.Distance(position, closestGridPosition);
            //Debug.Log("find 1");

            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    //Debug.Log("find 2");
                    float distance = Vector3.Distance(position, grid[x, y]);
                    if (distance < closestDistance)
                    {
                        //Debug.Log("find 3");
                        closestGridPosition = grid[x, y];
                        closestDistance = distance;
                    }
                }
            }

            return closestGridPosition;
        }


        void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            for (int x = 0; x <= gridWidth; x++)
            {
                for (int y = 0; y <= gridHeight; y++)
                {
                    Gizmos.DrawLine(new Vector3(x * gridSize, 0, 0), new Vector3(x * gridSize, 0, gridHeight * gridSize));
                    Gizmos.DrawLine(new Vector3(0, 0, y * gridSize), new Vector3(gridWidth * gridSize, 0, y * gridSize));
                }
            }
        }



    }
}