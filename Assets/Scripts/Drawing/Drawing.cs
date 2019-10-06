using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A standard drawing with no physics or fancy stuff.
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class Drawing : MonoBehaviour
{
    // The thickness of the drawing's lines
    [SerializeField] private float _lineThickness = 0.5f;

    // The colour of the drawing
    [SerializeField] private Color _color = Color.red;

    // The previous position of the drawing
    private Vector3 _previousPosition = Vector3.zero;

    // The previous rotation of the drawing
    private float _previousRotation = 0.0f;

    // The line renderer representing the drawing
    private LineRenderer _lineRenderer = null;

    // The points of the drawing
    private List<Vector2> _points = null;

    // Invoked when a point is added to the drawing
    public delegate void OnPointAdded(Vector2[] points);
    public event OnPointAdded OnPointAddedEvent;

    public float LineThickness
    {
        get => _lineThickness;
        protected set => _lineThickness = value;
    }


    private void Awake()
    {
        // Set up
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 0;

        // Apply line thickness
        _lineRenderer.startWidth = _lineThickness;
        _lineRenderer.endWidth = _lineThickness;

        // Apply line colour
        //_lineRenderer.material.color = _color;
        _lineRenderer.startColor = _color;
        _lineRenderer.endColor = _color;

        _previousPosition = transform.position;
        _points = new List<Vector2>();
    }

    // Runs every frame
    void Update()
    {
        // Get amount the transform has moved this frame
        Vector3 movementSinceLastFrame = transform.position - _previousPosition;

        // Get the amount we have rotated
        float rotationSinceLastFrame =
            transform.eulerAngles.z - _previousRotation;


        // If we have moved
        if (movementSinceLastFrame != Vector3.zero)
        {
            Vector3[] points = new Vector3[_lineRenderer.positionCount];
            _lineRenderer.GetPositions(points);
            // Set line renderer positions to its original positions
            // plus the amount we have moved and rotated
            for (int i = 0; i < _lineRenderer.positionCount; i++)
            {
                points[i] += movementSinceLastFrame;

                // The position of this point relative to points[0]
                Vector2 relativePosition = points[i] - points[0];
                points[i] =
                    points[0] + Quaternion.Euler(0, 0, rotationSinceLastFrame)
                    * relativePosition;
            }
            _lineRenderer.SetPositions(points);

            // Update our previous position
            _previousPosition = transform.position;
            _previousRotation = transform.eulerAngles.z;
        }
    }

    /// <summary>
    /// Adds a point to the drawing.
    /// </summary>
    /// <param name="point">The point to be added.</param>
    public void AddPoint(Vector3 point)
    {
        _points.Add(point);
        // Increase the line renderer's count;
        // Set the last point to be the new point.
        _lineRenderer.positionCount++;
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, point);

        // Update the collider
        Vector2 collisionPoint = new Vector2(
            point.x - transform.position.x,
            point.y - transform.position.y
        );

        OnPointAddedEvent?.Invoke(_points.ToArray());
    }
}
