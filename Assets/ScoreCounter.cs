using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Counts score.
/// </summary>
public class ScoreCounter : MonoBehaviour
{
    private Text _text;
    // Start is called before the first frame update
    void Awake()
    {
        _text = GetComponent<Text>();
    }

    void Update()
    {
        _text.text = "" + GameState.Score;
    }
}
