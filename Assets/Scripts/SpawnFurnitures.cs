using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class SpawnFurnitures : MonoBehaviour
{
    // Public variables to hold a list of furniture prefabs and the minimum and maximum coordinates of the spawn area.
    public List<GameObject> furniturePrefabs;
    public Vector3 spawnAreaMin;
    public Vector3 spawnAreaMax;

    // Public variable to hold the currently spawned furniture.
    public GameObject currentFurniture;

    // SpawnFurniture is called to spawn a piece of furniture.
    // It checks if there is currently spawned furniture that is not correctly placed,
    // and if there are remaining furniture prefabs. If both conditions are met,
    // it spawns a random furniture prefab at a random position within the spawn area of the house room.
    public void SpawnFurniture()
    {
        if (currentFurniture != null && !WinController.instance.isCorrectlyPlaced[currentFurniture.tag]) return;

        if (furniturePrefabs.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, furniturePrefabs.Count);
            GameObject furniturePrefab = furniturePrefabs[randomIndex];
            Vector3 spawnPosition = new Vector3(
                UnityEngine.Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                UnityEngine.Random.Range(spawnAreaMin.y, spawnAreaMax.y),
                UnityEngine.Random.Range(spawnAreaMin.z, spawnAreaMax.z)
            );
            currentFurniture = Instantiate(furniturePrefab, spawnPosition, furniturePrefab.transform.rotation);
            Vector3 position = currentFurniture.transform.position;
            position.y = 0; // Position of the spawned furniture to y = 0
            currentFurniture.transform.position = position;
            furniturePrefabs.RemoveAt(randomIndex);
        }
    }
}
