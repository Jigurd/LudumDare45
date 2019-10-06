using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private BoxCollider2D _collider;
    private MeshRenderer _renderer;

    // Start is called before the first frame update
    void Start()
    {
        _collider = transform.gameObject.GetComponent<BoxCollider2D>();
        _renderer = transform.gameObject.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Toggle()
    {
        _collider.enabled = !_collider.enabled;
        _renderer.enabled = !_renderer.enabled;
    }
}
