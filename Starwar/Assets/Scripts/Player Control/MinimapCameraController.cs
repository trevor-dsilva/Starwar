using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraController : MonoBehaviour
{
    public Camera_Control mainCameraController;
    public float heightOffset = 20f;
    public Vector3 positionOffset = new Vector3(0, 0, 0);

    private Camera minimapCamera;
    private Vector3 targetPosition;

    void Start()
    {
        // Check if mainCameraController is assigned
        if (mainCameraController == null)
        {
            Debug.LogError("MinimapCameraController: Main Camera Controller not assigned.");
            return;
        }

        // Get the camera component on this GameObject
        minimapCamera = GetComponent<Camera>();

        if (minimapCamera == null)
        {
            Debug.LogError("MinimapCameraController: Camera component not found.");
            return;
        }

        // Set the camera to orthographic
        minimapCamera.orthographic = true;
    }

    void Update()
    {
        // Calculate the new camera position based on main camera's position
        targetPosition = new Vector3(mainCameraController.transform.position.x + positionOffset.x, mainCameraController.transform.position.y + heightOffset, mainCameraController.transform.position.z + positionOffset.z);

        // Update the camera position
        transform.position = targetPosition;

        // Make sure the camera is always looking down
        transform.rotation = Quaternion.Euler(90, 0, 0);
    }
}
