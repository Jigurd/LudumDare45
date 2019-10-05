using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Actor actor;
    Controller controller;

    [SerializeField]
    public GameObject SpawnPoint;

    

    void Start()
    {
        controller = GetComponent<Controller>();
        actor = GetComponent<Actor>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameState.Paused)
        {
            return;
        }
        CheckInBounds();
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
        }
        //move right
        if (Input.GetButton("Right"))
        {
            actor.velocity.x += actor.moveSpeed;
        }
    }

    //checks if the player is still in the level
    void CheckInBounds()
    {
        //if character is far out of bounds
        if (Mathf.Abs(transform.position.x) > 40 || Mathf.Abs(transform.position.y)>40)
        {
            actor.velocity = new Vector3(0, 0); //reset speed
            transform.position = SpawnPoint.transform.position;
        }
    }
}
