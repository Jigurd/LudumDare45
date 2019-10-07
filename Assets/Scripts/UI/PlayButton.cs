using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles behaviour for the main menu play button.
/// </summary>
public class PlayButton : MonoBehaviour
{
    // The pause menu
    [SerializeField] PauseMenu _pauseMenu = null;

    private void Awake()
    {
        // Load scene when button is pressed.
        GetComponent<Button>().onClick.AddListener(
            delegate
            {
                GameState.Paused = false;
                _pauseMenu.SetPauseMenuEnabled(false);
            }
        );
    }
}
