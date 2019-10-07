using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles scrolling and generation of level
/// </summary>
public class LevelScrolling : MonoBehaviour
{
    // The speed at which to scroll
    [SerializeField] private float _speed = 1.0f;

    // The prefab to instant when generating level
    [SerializeField] private GameObject _paperPrefab = null;

    // Currently instantiated papers
    private List<Transform> _papers = null;


    private void Awake()
    {
        // Instantiate one sheet of paper
        _papers = new List<Transform>();
    }

    private void Start()
    {
        _papers.Add(Instantiate(
            _paperPrefab,
            Vector3.zero,
            Quaternion.identity,
            transform
        ).transform);
    }

    private void Update()
    {
        if (GameState.Paused)
        {
            return;
        }

        // Scroll
        foreach (Transform paper in _papers)
        {
            paper.position += Vector3.down * _speed;
        }

        // Instantiate new papers
        if (_papers[_papers.Count - 1].position.y <= 50.0f)
        {
            // Get the height of the paper so we can instantiate
            // it at the right place
            Transform paper = _papers[_papers.Count - 1];
            Bounds bounds =
                paper.gameObject.GetComponent<Collider2D>().bounds;

            Vector3 position = paper.position
                + Vector3.up * (bounds.max.y - bounds.min.y);

            _papers.Add(Instantiate(
                _paperPrefab,
                position,
                Quaternion.identity,
                transform
            ).transform);
        }

        // Delete old papers
        // TODO: Get deletion point from bounds
        if (_papers[0].position.y < -100.0f)
        {
            Destroy(_papers[0].gameObject);
            _papers.RemoveAt(0);
        }
    }
}
