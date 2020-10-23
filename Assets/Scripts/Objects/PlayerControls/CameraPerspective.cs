//Attached to the main camera
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraPerspective : MonoBehaviour
{
    bool disabled = false;
    public float speedH = 3f;
    public float speedV = 3f;

    public bool truckMode = false;

    private float yaw = 0.0f;
    private float pitch = 0.0f;


    // Use this for initialization
    void Start()
    {

        //Cursor.lockState = CursorLockMode.Locked;

    }
    void OnDisable()
    {
        disabled = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (disabled){
            disabled = false;
            yaw = 0.0f;
            pitch = 0.0f;
        }
        if (truckMode)
        {
            yaw += speedH * Input.GetAxis("Mouse X") / 100;
            pitch -= speedV * Input.GetAxis("Mouse Y") / 100;
            transform.Rotate(pitch, yaw, 0);
        }
        else
        {
            yaw += speedH * Input.GetAxis("Mouse X");
            pitch -= speedV * Input.GetAxis("Mouse Y");
            transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        }
    }
}


