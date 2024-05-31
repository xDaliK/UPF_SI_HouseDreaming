using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinController : MonoBehaviour
{
    public static WinController instance; // Referencia estática al controlador de juego

    public Dictionary<string, bool> isCorrectlyPlaced = new Dictionary<string, bool>();
    private GameObject[] winTextObjects;

    public AudioSource winAudio;
    private bool hasWon = false;

    void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        // Inicializa el diccionario
        isCorrectlyPlaced["sofa"] = false;
        isCorrectlyPlaced["bed"] = false;
        isCorrectlyPlaced["plant"] = false;
        isCorrectlyPlaced["cabinet"] = false;
        isCorrectlyPlaced["bookcase"] = false;
        isCorrectlyPlaced["television"] = false;

        winTextObjects = GameObject.FindGameObjectsWithTag("winText");

        foreach (GameObject winTextObject in winTextObjects)
        {
            winTextObject.SetActive(false);
        }

        winAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if ((!hasWon) && AllObjectsPlacedCorrectly())
        {
            Debug.Log("¡Has ganado!");
            foreach (GameObject winTextObject in winTextObjects)
            {
                winTextObject.SetActive(true);
            }
            winAudio.Play();

            hasWon = true;

            Invoke("LoadScene", 4f);
        }
    }

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



    void LoadScene()
    {
        SceneManager.LoadScene("SceneTitle");
    }
}
