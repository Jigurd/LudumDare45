using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Actor actor;
    Controller controller;
    //door stuff
    [SerializeField]
    public GameObject Door; //the level exit
    public LayerMask DoorMask;  //the layermask that looks for it

    [SerializeField] private LayerMask _paperMask = 0;

    [SerializeField] private PauseMenu _pauseMenu = null;

    Vector3 _startPosition;

    //sprite stuff
    public SpriteRenderer sprite;

    void Awake()
    {
        _startPosition = transform.position;
    }

    void Start()
    {
        controller = GetComponent<Controller>();
        actor = GetComponent<Actor>();
        sprite = GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        if (GameState.Paused)
        {
            return;
        }
        actor.jumpHeight += Time.deltaTime * 0.01f;
        actor.timeToJumpApex += Time.deltaTime * 0.01f;
        HandleMovement();

        // Set our parent to a sheet of paper because we don't want to be all jittery when it scrolls with its drawings and stuff
        Ray ray = new Ray(transform.position + Vector3.back, Vector3.forward);
        RaycastHit2D hit = Physics2D.Raycast(
            ray.origin,
            ray.direction,
            Mathf.Infinity,
            _paperMask
        );
        if (hit)
        {
            transform.parent = hit.collider.transform;
        }

        // Update score
        GameState.Score += (int)(Time.deltaTime * 123.4f);

        if (transform.position.y <= -50.0f)
        {
            Lose();
        }
    }

    void HandleMovement()
    {
        //reset horizontal speed
        actor.velocity.x = 0;


        //if the player is jumping, add jump velocity to jump
        if (Input.GetButtonDown("Jump") && controller.collisions.below)
        {
            actor.velocity.y += actor.jumpVelocity;
        }
        //move left
        if (Input.GetButton("Left"))
        {
            actor.velocity.x -= actor.moveSpeed;
            sprite.flipX = false;
        }
        //move right
        if (Input.GetButton("Right"))
        {
            actor.velocity.x += actor.moveSpeed;
            sprite.flipX = true;
        }

        //
        //if (Input.GetButtonDown("Interact"))
        //{
        //    CheckIfWon();
        //}
    }

    //checks if the player is standing by the exit
    //void CheckIfWon()
    //{
    //    //cast a ray from player's position toward door's position, with a length of half the player's size
    //    Vector3 pos = transform.position;
    //    RaycastHit2D hit = Physics2D.Raycast(pos, (Door.transform.position - pos).normalized, 0.5f, DoorMask);
    //    Debug.DrawRay(pos, (Door.transform.position - pos).normalized * 0.5f, Color.red);
    //
    //    if (hit)
    //    {
    //        WinLevel();
    //    }
    //    //if it hits, win
    //}


    private void Lose()
    {
        GameState.Lost = true;
        if (GameState.Score > GameState.HighScore)
        {
            GameState.HighScore = GameState.Score;
        }
        GameState.Score = 0;
        _pauseMenu.SetPauseMenuEnabled(true);
        transform.position = _startPosition;
        actor.velocity = Vector3.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Got Hit!");
            Lose();
        }
    }
}
