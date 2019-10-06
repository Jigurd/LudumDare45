using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// You can draw on paper.
/// </summary>
public class Paper : MonoBehaviour
{
    // Whether to print debug log messages
    [SerializeField] private bool _debugLogEnabled = false;

    // Whether the mouse is over this piece of paper
    public bool MouseOver { get; protected set; }

    private void Awake()
    {
        MouseOver = false;
        PolygonCollider2D collider =
            gameObject.AddComponent<PolygonCollider2D>();
        collider.isTrigger = true;
    }

    private void OnMouseOver()
    {
        MouseOver = true;
    }

    private void OnMouseEnter()
    {
        MouseOver = true;
        if (_debugLogEnabled)
        {
            Debug.Log(name + ": mouse over true", this);
        }
    }

    private void OnMouseExit()
    {
        MouseOver = false;
        if (_debugLogEnabled)
        {
            Debug.Log(name + ": mouse over false", this);
        }
    }
}
