using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Actor actor;
    Controller controller;

    [SerializeField]
    public GameObject SpawnPoint;

    //door stuff
    [SerializeField]
    public GameObject Door; //the level exit
    public LayerMask DoorMask;  //the layermask that looks for it

    //sprite stuff
    public SpriteRenderer sprite;

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
        HandleMovement();
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("I died.");
        }
    }
}
