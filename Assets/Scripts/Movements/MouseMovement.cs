using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{

    public float mouseSensitivity = 500f;


    private float xRotation = 0f;
    private float yRotation = 0f;


    public float topClamp = -90f;
    public float bottomClamp = 90f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Getting mouse inputs
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotation around the X Axis 
        xRotation -= mouseY;

        // Clamp the rotation - 90 to 90
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);


        // Rotation around the Y Axis
        yRotation += mouseX;

        // Applying the rotation to the camera
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

    }
}
