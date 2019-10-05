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

    // The id of the current drawing
    private int _drawingId = 1;

    // The type of drawing the current/next drawing will be of
    private DrawingType _selectedDrawingType = DrawingType.Standard;

    // The main camera, for finding the world space position when drawing
    private Camera _mainCamera = null;

    // Whether an instance of this object exists.
    private static bool _instantiated = false;

    void Awake()
    {
        // Make sure we only got one DrawingInputHandler
        if (!_instantiated)
        {
            _mainCamera = Camera.main;
            _instantiated = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Don't do anything if game is paused
        if (GameState.Paused)
        {
            return;
        }

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

            // Apply the immediate effects on the drawing based on type
            // Immediate effects are:
            //     1. Collision
            //     2. Colour (if we get to the point of changing that)
            //     3. Name
            GameObject drawingGO = _drawing.gameObject;

            switch (_selectedDrawingType)
            {
                case DrawingType.Doodle:
                    // Don't need anything
                    drawingGO.name = _drawingId++ + " Doodle";
                    break;
                case DrawingType.Standard:
                    // Collision enabled
                    drawingGO.AddComponent<DrawingCollider>();
                    drawingGO.name = _drawingId++ + " Standard drawing";
                    break;
                case DrawingType.Gravity:
                    // Collision enabled
                    drawingGO.AddComponent<DrawingCollider>();
                    drawingGO.name = _drawingId++ + " Gravity drawing";
                    break;
                default:
                    // Should never happen
                    Debug.LogError("Non-existing enum value selected?", this);
                    break;
            }

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

            // Set up the drawing based on the selected type
            // Effects that should apply when finished drawing are:
            //     1. Gravity (probably it tbh)
            GameObject drawingGO = _drawing.gameObject;
            switch (_selectedDrawingType)
            {
                case DrawingType.Doodle:
                    // Don't need anything
                    break;
                case DrawingType.Standard:
                    break;
                case DrawingType.Gravity:
                    drawingGO.AddComponent<Rigidbody2D>();
                    break;
                default:
                    // Should never happen
                    Debug.LogError("Non-existing enum value selected?", this);
                    break;
            }

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

    /// <summary>
    /// Sets the selected drawing mode.
    /// </summary>
    /// <param name="type">The drawing type to switch to.</param>
    public void SetSelectedDrawingMode(DrawingType type)
    {
        _selectedDrawingType = type;
    }

    // The types of drawings available.
    public enum DrawingType
    {
        Doodle,     // A drawing with no effect on the game
        Standard,   // A static drawing with collision
        Gravity,    // A physics-enabled drawing with collision
    }
}
