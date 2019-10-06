using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    private Controller _controller;
    private GameObject _player;
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    public float moveSpeed;

    private LayerMask _layerMask;

    void Start()
    {
        _controller = GetComponent<Controller>();
        _player = GameObject.Find("Player");
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        //get our position
        Vector3 pos = transform.position;
        Vector3 dir = (_player.transform.position - pos).normalized;
        //raycast to player
        RaycastHit2D hit = Physics2D.Raycast(pos, dir , Mathf.Infinity, _layerMask);
        Debug.DrawRay(pos, dir * 10, Color.red);

        //if player isn't looking at us, become green and move towards him
        if (hit && !hit.collider.CompareTag("Player"))
        { 
            _spriteRenderer.color = Color.green;
            _controller.Move(dir * moveSpeed * Time.deltaTime);
        } else if (hit && hit.collider.CompareTag("Player")) //if he is looking at us
        {
            //turn red and don't move
            _spriteRenderer.color = Color.red;
        }
        else
        {
            Debug.Log("Wtf no hit");
        }
        //If we hit anything in our LayerMask other than Player, we can't see him
        
        //move speed towards him


        
    }
}
