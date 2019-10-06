using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sets the camera to the desired size
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraResizer : MonoBehaviour
{
    // The desired half width of the camera view
    [SerializeField] private float _orthographicWidth = 5.0f;

    // The current width of the screen
    private float _screenWidth = 0.0f;

    // The current height of the screen
    private float _screenHeight = 0.0f;

    // The camera whose size we want to set
    private Camera _camera = null;

    void Awake()
    {
        _camera = GetComponent<Camera>();
        if (_camera == null)
        {
            Debug.LogError(
                "CameraResizer on GameObject without a Camera", this
            );
        }

        // Set camera's orthographic size according to the width
        UpdateOrthographicSize();
    }

    private void Update()
    {
        // Check if the resolution has changed
        if (_screenWidth != Screen.width || _screenHeight != Screen.height)
        {
            UpdateOrthographicSize();
        }
    }

    /// <summary>
    /// Sets the camera's orthographic size
    /// according to resolution and desired width.
    /// </summary>
    private void UpdateOrthographicSize()
    {
        _screenWidth = Screen.width;
        _screenHeight = Screen.height;

        // The orthographic size is 1/2 the height of the view
        // in Unity units. Apply some maths and:
        _camera.orthographicSize =
            (_orthographicWidth * _screenHeight) /
            (_screenWidth);
    }
}
