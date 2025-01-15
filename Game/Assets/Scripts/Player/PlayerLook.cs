using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{

    public Camera cam;
    public float xRotation = 0f;

    public float xSensitivity = 30f;
    public float ySensitivity = 30f;

    public float smoothTime = 0.05f; // Smoothing factor for rotation

    private float currentMouseX;
    private float currentMouseY;
    private float mouseXVelocity; // Velocity for horizontal smoothing
    private float mouseYVelocity; // Velocity for vertical smoothing

    public float targetYRotation;
    public void ProcessLook(Vector2 input)
    {
        if (Time.timeScale == 0)
            return;

        // Get target mouse input
        float targetMouseX = input.x;
        float targetMouseY = input.y;
        targetYRotation = targetMouseY;

        // Smooth the mouse input
        currentMouseX = Mathf.SmoothDamp(currentMouseX, targetMouseX, ref mouseXVelocity, smoothTime);
        currentMouseY = Mathf.SmoothDamp(currentMouseY, targetYRotation, ref mouseYVelocity, smoothTime);

        // Calculate camera rotation for looking up and down
        xRotation -= (currentMouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        // Apply this to our camera transform
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotate player to look left and right
        transform.Rotate(Vector3.up * (currentMouseX * Time.deltaTime) * xSensitivity);
    }

    /*

    //[HideInInspector] public float zRotation;
    //public float mouseSensitivity = 0;

    //private float rotationYVelocity, cameraXVelocity;
    //public float yRotationSpeed, xCameraSpeed;

    //[HideInInspector] public float wantedYRotation;
    //[HideInInspector] public float currentYRotation;

    //[HideInInspector] public float wantedCameraXRotation;
    //[HideInInspector] public float currentCameraXRotation;

    //public float topAngleView = 60;
    //public float bottomAngleView = -45;

    public void ProcessLook(Vector2 input)
    {

        //wantedYRotation += input.x * mouseSensitivity;
        //wantedCameraXRotation -= input.y * mouseSensitivity;
        //wantedCameraXRotation = Mathf.Clamp(wantedCameraXRotation, bottomAngleView, topAngleView);
        //currentYRotation = Mathf.SmoothDamp(currentYRotation, wantedYRotation, ref rotationYVelocity, yRotationSpeed);
        //currentCameraXRotation = Mathf.SmoothDamp(currentCameraXRotation, wantedCameraXRotation, ref cameraXVelocity, xCameraSpeed);

        //transform.rotation = Quaternion.Euler(0, currentYRotation, 0);
        //cam.transform.localRotation = Quaternion.Euler(currentCameraXRotation, 0f, 0f);


        float mouseX = input.x;
        float mouseY = input.y;

        //calculate camera rotation for looking up and down
        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        //apply this to our camera transform
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        //rotate player to look left and right
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
    }
    */
}
