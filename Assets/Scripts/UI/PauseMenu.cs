using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    // Whether the pause menu is enabled or not
    private bool _enabled = true;

    [SerializeField] private Text _text;

    private void Awake()
    {
        // Set the pause menu to its desired inital state.
        // Might as well hardcode this to false really.
        SetPauseMenuEnabled(_enabled);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            // Toggle the pause menu
            SetPauseMenuEnabled(!_enabled);
        }
    }

    /// <summary>
    /// Toggles the pause menu.
    /// </summary>
    /// <param name="enabled">The new state of the pause menu.</param>
    public void SetPauseMenuEnabled(bool enabled)
    {
        _enabled = enabled;
        // Set each child to the desired state
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(enabled);
        }

        if (GameState.Lost)
        {
            // Say you lose and ask if player wants to try again
            _text.text = "You lost.\nHigh Score: " + GameState.HighScore;
        }
        else
        {
            _text.text = "Game is paused.";
        }

        if (_enabled)
        {
            // Pause the game
            GameState.CanDraw = false;
            GameState.Paused = true;
            GameState.PauseMenuEnabled = true;
        }
        else
        {
            // Resume the game
            GameState.CanDraw = true;
            GameState.Paused = true;
            GameState.PauseMenuEnabled = false;
            GameState.Lost = false;
        }
    }
}
