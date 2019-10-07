using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Updates the collider of a drawing
/// </summary>
[RequireComponent(typeof(Drawing))]
[RequireComponent(typeof(PolygonCollider2D))]
public class DrawingCollider : MonoBehaviour
{
    // The collider of the drawing
    private PolygonCollider2D _collider = null;

    // The drawing for which we need the collider
    private Drawing _drawing = null;

    // Start is called before the first frame update
    void Awake()
    {
        // Register listener for when our drawing gets a new point
        _drawing = GetComponent<Drawing>();
        _drawing.OnPointAddedEvent += OnPointAdded;

        _collider = GetComponent<PolygonCollider2D>();
        _collider.points = null;
    }

    /// <summary>
    /// Updates the collider upon adding a point to the drawing
    /// </summary>
    /// <param name="point"></param>
    private void OnPointAdded(Vector2[] points)
    {
        // p1 = points[i - 1]
        // p2 = points[i]
        // v = distance from point i - 1 to i normalised    // both len = 1
        // n = normal(v)                                    //
        // c = collision shape
        // c1 = p1 - r * v - r * n
        // c2 = p2 + r * v - r * n
        // c3 = p2 + r * v + r * n
        // c4 = p1 - r * v + r * n
        // This should be correct, I did a whiteboard thing.
        // Actually this is not correct since the line radius doesn't
        // affect the length, thus v should be ignored
        if (points.Length > 1)
        {
            // Create a path for each line segment
            // Better way might be to travel all
            // around the line but time constraints yo

            float r = _drawing.LineThickness / 2.0f;
            //HashSet<Vector2> collisionPoints = new HashSet<Vector2>();
            Vector2 position = transform.position;
            _collider.pathCount = points.Length - 1;
            for (int i = 1; i < points.Length; i++)
            {
                // Set up our variables
                //int i = points.Length - 1;
                Vector2 p1 = points[i - 1];
                Vector2 p2 = points[i];
                Vector2 v = (p2 - p1).normalized;
                Vector2 n = Vector2.Perpendicular(v);

                // Create a path
                Vector2[] path = new Vector2[4];
                path[0] = p1 /*- r * v*/ - r * n - position;
                path[1] = p2 /*+ r * v*/ - r * n - position;
                path[2] = p2 /*+ r * v*/ + r * n - position;
                path[3] = p1 /*- r * v*/ + r * n - position;

                _collider.SetPath(i - 1, path);
            }

            /*
            for (int i = 1; i < points.Length; i++)
            {

            }
            */
            //_collider.points = something;//collisionPoints.ToArray();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision");
    }
}
