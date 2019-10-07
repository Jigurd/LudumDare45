using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    // Whether the pause menu is enabled or not
    private bool _enabled = true;

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

        if (_enabled)
        {
            // Pause the game
            GameState.Paused = true;
        }
        else
        {
            // Resume the game
            GameState.Paused = false;
        }
    }
}
