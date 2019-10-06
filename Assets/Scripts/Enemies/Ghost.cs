using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    private GameObject _player;
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    public float MoveSpeed;
    [SerializeField]
    private float _distanceToPlayer;
    private Vector3 _targetVector;
    private Vector3 _targetPosition;
    private Vector3 _divePosition;

    [SerializeField]
    private LayerMask _layerMask;
    State state = State.Hover;

    void Start()
    {
        _player = GameObject.Find("Player");
        _spriteRenderer = GetComponent<SpriteRenderer>();

        //set a random point for the ghost to aim to stay at
        float angle = Random.Range(0, Mathf.PI * 2);
        _targetVector = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)).normalized * _distanceToPlayer;

    }


    void Update()
    {
        //update target position
        _targetPosition = _player.transform.position + _targetVector;
        Vector3 direction = (_targetPosition - transform.position).normalized;

        switch (state)
        {
            case (State.Hover):
                {
                    //move speed towards target, until we reach the position

                    if (Vector3.Distance(transform.position, _targetPosition) > MoveSpeed/100)
                    {
                        transform.Translate(direction * MoveSpeed * Time.deltaTime);
                    }
                    break;
                }
            default: break;
        }
    }

    enum State
    {
        Hover,
        StartDive,
        Dive
    }
}
