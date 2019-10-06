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

    // The layermask used to detect if we're drawing on paper
    [SerializeField] private LayerMask _layerMask = 0;

    // The papers that we can draw on
    //[SerializeField] private Paper[] _papers = null;

    // Whether the player is currently working on a masterpiece
    private bool _isDrawing = false;

    // Whether the player is allowed to draw
    private bool _canDraw = true;

    // The drawing we are currently working on
    private Drawing _drawing = null;

    // The id of the current drawing
    private int _drawingId = 1;

    // The type of drawing the current/next drawing will be of
    private DrawingType _selectedDrawingType = DrawingType.Standard;

    // The mouse position where we started drawing
    private Vector2 _drawingStartPoint = Vector2.zero;

    // The main camera, for finding the world space position when drawing
    private Camera _mainCamera = null;

    // Whether an instance of this object exists.
    private static bool _instantiated = false;

    // The previous mouse position
    private Vector3 _previousMousePosition = Vector2.zero;

    void Awake()
    {
        // Make sure we only got one DrawingInputHandler
        if (!_instantiated)
        {
            _mainCamera = Camera.main;
            _instantiated = true;
            _previousMousePosition = Input.mousePosition;
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

        // Check if drawing is allowed
        // i.e. at least one paper has the mouse over it
        _canDraw = false;
        //foreach (Paper paper in _papers)
        //{
        //    if (paper.MouseOver)
        //    {
        //        _canDraw = true;
        //        break;
        //    }
        //}

        // The above doesn't work because colliders on top
        // of the paper ruin everything

        // Use a raycast to check if there'e paper beneath the cursor
        Ray ray =
            _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(
            _mainCamera.ScreenToWorldPoint(Input.mousePosition),
            ray.direction,
            Mathf.Infinity,
            _layerMask
        );
        if (hit)
        {
            _canDraw = true;
        }

        // User starts holding left mouse button
        if (!_isDrawing && Input.GetMouseButtonDown(0))
        {
            // Start drawing
            _isDrawing = true;

            // Get the point where the user clicked
            Vector2 point = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            _drawingStartPoint = point;

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
            // Actually we'll do this later this frame anyway so don't
            // Actually nevermind
            // Draw a small line
            // Actually don't, it just doesn't work

            //float directionAngle = Random.Range(0, 360f);
            //Vector3 directionVector =
            //    Quaternion.Euler(0, 0, directionAngle) * Vector2.up;
            //directionVector *= _drawing.LineThickness / 2.0f;

            _drawing.AddPoint(
                point// - new Vector2(directionVector.x, directionVector.y)
            );
            //_drawing.AddPoint(
            //    point + new Vector2(directionVector.x, directionVector.y)
            //);

            if (_debugLogEnabled)
            {
                Debug.Log(
                    _drawing.name + ": " +
                    "Start drawing at " + point + ".", this
                );
            }
        }

        // User stops holding left mouse button
        if (_isDrawing && Input.GetMouseButtonUp(0))
        {
            Vector2 point =
                _mainCamera.ScreenToWorldPoint(Input.mousePosition);

            if (_debugLogEnabled)
            {
                Debug.Log(
                    _drawing.name + ": " +
                    "Stop drawing at " + point + ".", this
                );
            }
            
            // Check if the user actually drew something
            // or just clicked somewhere
            if (point != _drawingStartPoint)
            {
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
                        Rigidbody2D rb = drawingGO.AddComponent<Rigidbody2D>();
                        break;
                    default:
                        // Should never happen
                        Debug.LogError(
                            "Non-existing enum value selected?", this
                        );
                        break;
                }

            }
            else
            {
                // The user just clicked, delete the drawing
                Destroy(_drawing.gameObject);

                if (_debugLogEnabled)
                {
                    Debug.Log(
                        _drawing.name + ": " +
                        "Stopped drawing at the same point as start, " +
                        "delete drawing.",
                        this
                    );
                }
            }

            // Stop drawing
            _isDrawing = false;
            _drawing = null;
        }

        // The player is currently drawing
        if (_isDrawing)
        {
            // ... and they're staying within the allowed area
            if (_canDraw)
            {
                // ... and moving their cursor
                if (Input.mousePosition != _previousMousePosition)
                {
                    // Add to the drawing / line renderer
                    Vector2 point =
                        _mainCamera.ScreenToWorldPoint(Input.mousePosition);

                    _drawing.AddPoint(point);
                    _previousMousePosition = Input.mousePosition;
                }
            }
            else
            {
                // Went outside bounds, destroy drawing
                Destroy(_drawing.gameObject);
                _isDrawing = false;
                _drawing = null;
            }
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

    /*
    // Called when the mouse enters collider.
    private void OnMouseEnter()
    {
        // We use the collider to check where we can draw
        // Anywhere within the collider is forbidden
        // Anywhere else is ok
        _canDraw = false;
    }

    // Called when the mouse exits collider.
    private void OnMouseExit()
    {
        // We use the collider to check where we can draw
        // Anywhere within the collider is forbidden
        // Anywhere else is ok
        _canDraw = true;

        if (_debugLogEnabled)
        {
            Debug.Log("Drawing enabled", this);
        }
    }
    */

    // Called when the game object is destroyed
    private void OnDestroy()
    {
        // We can now make a new DrawingInputHandler
        _instantiated = false;

        if (_debugLogEnabled)
        {
            Debug.Log("Drawing disabled", this);
        }
    }
}
