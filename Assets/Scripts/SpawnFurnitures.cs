using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class SpawnFurnitures : MonoBehaviour
{
    public List<GameObject> furniturePrefabs; // Lista de prefabs de muebles
    public Vector3 spawnAreaMin; // Esquina inferior izquierda del área de spawn
    public Vector3 spawnAreaMax; // Esquina superior derecha del área de spawn

    public GameObject currentFurniture;

    public void SpawnFurniture()
    {
        if (currentFurniture != null && !WinController.instance.isCorrectlyPlaced[currentFurniture.tag]) return;

        if (furniturePrefabs.Count > 0) // Solo intenta spawnear un mueble si hay muebles restantes
        {
            // Elige un índice de mueble aleatorio
            int randomIndex = UnityEngine.Random.Range(0, furniturePrefabs.Count);

            // Obtiene el prefab de mueble en el índice aleatorio
            GameObject furniturePrefab = furniturePrefabs[randomIndex];

            // Calcula una posición aleatoria dentro del área de spawn
            Vector3 spawnPosition = new Vector3(
                UnityEngine.Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                UnityEngine.Random.Range(spawnAreaMin.y, spawnAreaMax.y),
                UnityEngine.Random.Range(spawnAreaMin.z, spawnAreaMax.z)
            );

            // Instancia el mueble en la posición aleatoria
            currentFurniture = Instantiate(furniturePrefab, spawnPosition, furniturePrefab.transform.rotation);

            // Ajusta la posición y del objeto instanciado
            Vector3 position = currentFurniture.transform.position;
            position.y = 0; // Ajusta esto al valor que necesites
            currentFurniture.transform.position = position;

            // Elimina el prefab de mueble de la lista para que no se repita
            furniturePrefabs.RemoveAt(randomIndex);
        }

    }
}
