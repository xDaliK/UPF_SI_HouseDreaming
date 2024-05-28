using System.Collections.Generic;
using UnityEngine;

public class WinController : MonoBehaviour
{
    public static WinController instance; // Referencia estática al controlador de juego

    public Dictionary<string, bool> isCorrectlyPlaced = new Dictionary<string, bool>();

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
    }
}
