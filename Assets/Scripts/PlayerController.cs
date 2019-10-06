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

    //holds info about the 4 quads around the player. They're used for determining LOS for our chaser enemies.
    LOSquad los;

    //sprite stuff
    public SpriteRenderer sprite;

    void Start()
    {
        controller = GetComponent<Controller>();
        actor = GetComponent<Actor>();
        sprite = GetComponent<SpriteRenderer>();

        //initialize LOS quads
        los.TopLeft =         transform.Find("TopLeftLOSQuad").gameObject.GetComponent<BoxCollider2D>();
        los.TopRight =       transform.Find("TopRightLOSQuad").gameObject.GetComponent<BoxCollider2D>();
        los.BottomLeft =   transform.Find("BottomLeftLOSQuad").gameObject.GetComponent<BoxCollider2D>();
        los.BottomRight = transform.Find("BottomRightLOSQuad").gameObject.GetComponent<BoxCollider2D>();
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

        los.UpdateFacing();

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
        if (Input.GetButtonDown("Interact"))
        {
            CheckIfWon();
        }
    }

    //checks if the player is standing by the exit
    void CheckIfWon()
    {
        //cast a ray from player's position toward door's position, with a length of half the player's size
        Vector3 pos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos, (Door.transform.position - pos).normalized, 0.5f, DoorMask);
        Debug.DrawRay(pos, (Door.transform.position - pos).normalized * 0.5f, Color.red);

        if (hit)
        {
            WinLevel();
        }
        //if it hits, win
    }

    //checks if the player is still in the level
    void CheckInBounds()
    {
        //if character is far out of bounds
        if (Mathf.Abs(transform.position.x) > 40 || Mathf.Abs(transform.position.y) > 40)
        {
            actor.velocity = new Vector3(0, 0); //reset speed
            transform.position = SpawnPoint.transform.position;
        }
    }

    void WinLevel()
    {
        Debug.Log("Yay, you win!");
        SceneManager.LoadScene("Level" + ++GameState.CurrentLevel);
    }

    //the definitely-not-boos shoot a ray at the player, and if it hits they can be seen and are not allowed to move.
    struct LOSquad
    {
        public BoxCollider2D TopLeft, TopRight, BottomLeft, BottomRight;


        //disable LOS barriers on the left, enable on the right
        private void TurnRight()
        {
            Debug.Log("Right");
            TopLeft.enabled = true;
            BottomLeft.enabled = true;
            TopRight.enabled = false;
            BottomRight.enabled = false;
        }

        //disable LOS barriers on the right, enable on the left
        private void TurnLeft()
        {
            Debug.Log("Left");
            TopLeft.enabled = false;
            BottomLeft.enabled = false;
            TopRight.enabled = true;
            BottomRight.enabled = true;
        }

        public void UpdateFacing()
        {
            if (Input.GetButtonDown("Left"))
            {
                TurnLeft();
            }
            else if (Input.GetButtonDown("Right"))
            {
                TurnRight();
            }
        }
    }
}
