using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A standard drawing with no physics or fancy stuff.
/// </summary>
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(EdgeCollider2D))]
public class Drawing : MonoBehaviour
{
    // The previous position of the drawing
    private Vector3 _previousPosition = Vector3.zero;

    // The line renderer representing the drawing
    private LineRenderer _lineRenderer = null;

    // The collider of the drawing
    private EdgeCollider2D _collider = null;

    // The points of the drawing
    private List<Vector2> _collisionPoints = null;

    private void Awake()
    {
        // Set up
        _lineRenderer = GetComponent<LineRenderer>();
        _collider = GetComponent<EdgeCollider2D>();
        _previousPosition = transform.position;
        _collisionPoints = new List<Vector2>();
        _collisionPoints.Add(Vector2.zero);
    }

    // Runs every frame
    void Update()
    {
        // Get amount the transform has moved this frame
        Vector3[] points = new Vector3[_lineRenderer.positionCount];
        _lineRenderer.GetPositions(points);
        Vector3 movementSinceLastFrame = transform.position - _previousPosition;

        // If we have moved
        if (movementSinceLastFrame != Vector3.zero)
        {
            // Set line renderer positions to its original positions
            // plus the amount we have moved
            for (int i = 0; i < _lineRenderer.positionCount; i++)
            {
                points[i] += movementSinceLastFrame;
            }
            _lineRenderer.SetPositions(points);

            // Update our previous position
            _previousPosition = transform.position;
        }
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

        // Update the collider
        Vector2 collisionPoint = new Vector2(
            point.x - transform.position.x,
            point.y - transform.position.y
        );

        _collisionPoints.Add(collisionPoint);

        if (_collisionPoints.Count > 1)
        {
            _collider.points = _collisionPoints.ToArray();
        }
    }
}
