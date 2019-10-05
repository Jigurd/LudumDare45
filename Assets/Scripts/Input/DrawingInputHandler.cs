using UnityEngine;

/// <summary>
/// Handles the player input related to drawing.
/// </summary>
public class DrawingInputHandler : MonoBehaviour
{
    // Whether we want to print Debug.Log messages
    [SerializeField] private bool _debugLogEnabled = false;

    // The prefab to instantiate when creating a drawing
    [SerializeField] private GameObject _drawingPrefab = null;

    // Whether the player is currently working on a masterpiece
    private bool _isDrawing = false;

    // The drawing we are currently working on
    private Drawing _drawing = null;

    // The main camera, for finding the world space position when drawing
    private Camera _mainCamera = null;

    void Awake()
    {
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // User starts holding left mouse button
        if (Input.GetMouseButtonDown(0))
        {
            // Start drawing
            _isDrawing = true;

            // Get the point where the user clicked
            Vector2 point = _mainCamera.ScreenToWorldPoint(Input.mousePosition);

            // Instantiate the drawing at the point
            _drawing = Instantiate(
                _drawingPrefab,
                point,
                Quaternion.identity,
                transform
            ).GetComponent<Drawing>();

            // Set the first point of the drawing to the point
            _drawing.AddPoint(point);

            if (_debugLogEnabled)
            {
                Debug.Log("Start drawing at " + point + ".", this);
            }
        }

        // User stops holding left mouse button
        if (Input.GetMouseButtonUp(0))
        {
            // Stop drawing
            _isDrawing = false;
            // Make the drawing behave according to the chosen material

            if (_debugLogEnabled)
            {
                Vector2 point =
                    _mainCamera.ScreenToWorldPoint(Input.mousePosition);
                Debug.Log("Stop drawing at " + point + ".", this);
            }
        }

        // The player is currently drawing
        if (_isDrawing)
        {
            // Add to the drawing / line renderer
            Vector2 point = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            _drawing.AddPoint(point);
        }
    }
}
