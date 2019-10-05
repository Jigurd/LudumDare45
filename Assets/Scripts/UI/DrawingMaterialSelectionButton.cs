using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the behaviour of the drawing material selection buttons
/// </summary>
public class DrawingMaterialSelectionButton : MonoBehaviour
{
    // The drawing type that the button enables
    [SerializeField] private DrawingInputHandler.DrawingType _type;

    // Reference to the drawing input handler
    [SerializeField] private DrawingInputHandler _drawingInputHandler;

    private void Awake()
    {
        // Set the selected drawing mode on click
        GetComponent<Button>().onClick.AddListener(
            delegate
            {
                _drawingInputHandler.SetSelectedDrawingMode(_type);
            }
        );
    }
}
