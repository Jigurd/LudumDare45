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
    private float _timeEnteredState;

    [SerializeField]
    private float _minHoverTime = 3;
    [SerializeField]
    private float _maxHoverTime = 6;
    private float _hoverTime;

    private int _numOfDives;

    void Start()
    {
        _player = GameObject.Find("Player");
        _spriteRenderer = GetComponent<SpriteRenderer>();

        //set a random point for the ghost to aim to stay at
        GenerateTargetVector();

        _hoverTime = Random.Range(_minHoverTime, _maxHoverTime);
        _timeEnteredState = Time.time;

        _numOfDives = Random.Range(2, 7);
    }


    void Update()
    {
        if (GameState.Paused)
        {
            return;
        }

        switch (state)
        {
            case (State.Hover):
                {
                    //update target position
                    _targetPosition = _player.transform.position + _targetVector;
                    //move speed towards target, until we reach the position

                    Move(_targetPosition, MoveSpeed);

                    //if we've hovered for long enoguh
                    if (Time.time > _timeEnteredState + _hoverTime)
                    {
                        //reset time
                        _timeEnteredState = Time.time;
                        state = State.StartDive;
                    }
                    break;
                }
            case (State.StartDive):
                {
                    //hover for a second, then dive for the player
                    if (Time.time > _timeEnteredState + 0.5)
                    {
                        //if we have to return for more dives after this
                        if (--_numOfDives > 0)
                        {
                            //set a target through player, plus twice their hover distance
                            _targetPosition = (_player.transform.position - transform.position).normalized * _distanceToPlayer * 3;

                        } else
                        {
                            _targetPosition = (_player.transform.position - transform.position).normalized * _distanceToPlayer * 999;
                        }
                            state = State.Dive;
                    }
                        break;
                }
            case (State.Dive):
                {
                    //dive for target until we reach it
                    Debug.Log(_targetPosition + " And Player Pos: " + _player.transform.position);
                    if (Move(_targetPosition, MoveSpeed * 2))
                    {
                        //we reached it
                        //got to return state
                        state = State.Return;
                    }
                    break;
                }
            case (State.Return):
                {
                    break;
                }
            default: break;
        }
    }

    private bool Move(Vector3 targetPos, float speed)
    {
        Vector3 direction = (targetPos - transform.position).normalized;
        if (Vector3.Distance(transform.position, targetPos) > speed / 100)
        {
            transform.Translate(direction * speed * Time.deltaTime);
            return false;
        }
        else
        {
            return true;
        }
    }

    //returns true if we're within 1% of speed's movement of the destination. Basically, if we're there
    private bool Move(Vector3 targetPos)
    {
        Vector3 direction = (targetPos).normalized;
        if (Vector3.Distance(transform.position, targetPos) > MoveSpeed / 100)
        {
            transform.Translate(direction * MoveSpeed * Time.deltaTime);
            return false;
        }
        else
        {
            return true;
        }
    }

    private void GenerateTargetVector()
    {
        float angle = Random.Range(0, Mathf.PI * 2);
        _targetVector = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)).normalized * _distanceToPlayer;
    }




    enum State
    {
        Hover,
        StartDive,
        Dive,
        Return
    }
}
