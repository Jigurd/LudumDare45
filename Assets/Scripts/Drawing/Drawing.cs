using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A standard drawing with no physics or fancy stuff.
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class Drawing : MonoBehaviour
{
    // The line renderer representing the drawing
    LineRenderer _lineRenderer = null;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }


    /// <summary>
    /// Adds a point to the drawing.
    /// </summary>
    /// <param name="point">The point to be added.</param>
    public void AddPoint(Vector3 point)
    {
        // Increase the line renderer's count;
        // Set the last point to be the new point.
        _lineRenderer.positionCount++;
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, point);
    }
}
