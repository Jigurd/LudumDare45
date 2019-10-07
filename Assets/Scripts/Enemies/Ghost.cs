using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    private GameObject _player;
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private float _moveSpeed = 20;
    [SerializeField]
    private float _hoverDistance;
    private Vector3 _targetVector;
    private Vector3 _targetPosition;

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

        //set a point for the ghost to aim to stay at
        GenerateTargetVector();

        _hoverTime = Random.Range(_minHoverTime, _maxHoverTime);
        _timeEnteredState = Time.time;

        _numOfDives = Random.Range(2, 7);
    }


    void Update()
    {
        if (GameState.Lost)
        {
            Destroy(gameObject);
        }
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
                    Move(_targetPosition, _moveSpeed);

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
                            _targetPosition = (_player.transform.position - transform.position).normalized * _hoverDistance * 3;

                        }
                        else
                        {
                            _targetPosition = (_player.transform.position - transform.position).normalized * _hoverDistance * 20;
                        }
                        state = State.Dive;
                    }
                    break;
                }
            case (State.Dive):
                {
                    //dive for target until we reach it
                    //Debug.Log(_targetPosition + " And Player Pos: " + _player.transform.position);
                    if (Move(_targetPosition, _moveSpeed))
                    {
                        //we reached it
                        //if we have more dives
                        if (_numOfDives>0)
                        {
                        //got to return state
                        state = State.Return;

                        }
                        else //go gently into that good night
                        {
                            Destroy(this);
                        }
                       
                    }
                    break;
                }
            case (State.Return):    //in this state we just try to get back to the player to go for another dive
                {
                    if (Vector3.Distance(transform.position, _player.transform.position) > _hoverDistance)
                    {
                        Move(_player.transform.position, _moveSpeed*2);
                    }
                    else
                    {
                        //if we've gotten back to the player

                        //find a new place to hover
                        GenerateTargetVector();
                        
                        //return to hover state
                        state = State.Hover;
                    }
                    break;
                }
            default: break;
        }
    }

    //returns true if we're at the destination
    private bool Move(Vector3 targetPos, float speed)
    {
        Vector3 direction = (targetPos - transform.position).normalized;
        Vector3 movement = direction * speed * Time.deltaTime;
        if (Vector3.Distance(transform.position, targetPos) > movement.magnitude)
        {
            transform.Translate(movement);
            return false;
        }
        else
        {
            transform.Translate(direction * Vector3.Distance(transform.position, targetPos));
            return true;
        }
    }



    private void GenerateTargetVector()
    {
        //take the vector from the player to me, normalize it
        //that's our new target vector
        _targetVector = (transform.position - _player.transform.position).normalized * _hoverDistance;
    }




    enum State
    {
        Hover,
        StartDive,
        Dive,
        Return
    }
}
