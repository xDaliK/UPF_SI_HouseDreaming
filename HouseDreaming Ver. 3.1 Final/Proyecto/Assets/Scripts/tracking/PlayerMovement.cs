using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public Quaternion q;
    public bool manual;
    public float speed = 1.0f; // Velocidad de subida/bajada

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Si se presiona 'Z', sube de altura
        if (Input.GetKey(KeyCode.Z))
        {
            transform.position = transform.position + new Vector3(0, speed * Time.deltaTime + 0.5f, 0);
        }

        // Si se presiona 'X', baja de altura
        if (Input.GetKey(KeyCode.X))
        {
            transform.position = transform.position - new Vector3(0, speed * Time.deltaTime + 0.5f, 0);
        }
    }

    public void setPosition(Vector3 pos)
    {
        //swith playerIndex
        transform.position = pos;

        /*
         Vector3 newPos;
        swich (playerIndex)
        {
            case 1:
                newpos = new Vector3(Math.clamp( ... ) , pos... , pos... );
         */
    }

    public void setRotation(Quaternion quat)
    {
        //Matrix4x4 mat = Matrix4x4.Rotate(quat);
        //transform.localRotation = quat;
    }
}
