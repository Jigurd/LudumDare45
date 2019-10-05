using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles behaviour for the quit button.
/// </summary>
public class QuitButton : MonoBehaviour
{
    private void Awake()
    {
        // Quit the game when button pressed.
        GetComponent<Button>().onClick.AddListener(
            delegate
            {
                Debug.Log("Quit the application");
                Application.Quit();
            }
        );
    }
}
