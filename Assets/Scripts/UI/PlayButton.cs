using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles behaviour for the main menu play button.
/// </summary>
public class PlayButton : MonoBehaviour
{
    // The scene to load when the play button is pressed.
    [SerializeField] private int _indexOfSceneToLoad = 1;


    private void Awake()
    {
        // Load scene when button is pressed.
        GetComponent<Button>().onClick.AddListener(
            delegate { SceneManager.LoadScene(_indexOfSceneToLoad); }
        );
    }
}
