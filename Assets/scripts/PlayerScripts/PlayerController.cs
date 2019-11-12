using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    private Rigidbody2D body;

    Vector2 direction;

    [SerializeField]
    private float speed = 2;
   
    [SerializeField]
    private float maxSpeed = 10;

    [Header("Jump")]
    [SerializeField]private float jumpForce = 5;
    [SerializeField] float raycastJumpLength = 0.6f;
    [SerializeField] float timeStopJump = 0.1f;
    float timerStopJump = 0f;
    bool canJump = false;
    int touchedWall = 0;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();

        if (body != null)
        {
            Debug.Log("Player's body founded!");
        }
        else
        {
            Debug.Log("No player body");
        }
    }

    void FixedUpdate()
    {
        body.velocity = direction;
        
    }

    void JumpCheck()
    {
        timerStopJump -= Time.deltaTime;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastJumpLength, 1 << LayerMask.NameToLayer("platform"));
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, raycastJumpLength, 1 << LayerMask.NameToLayer("wall"));
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, raycastJumpLength, 1 << LayerMask.NameToLayer("wall"));

        if (timerStopJump <= 0)
        {
            if (hit.rigidbody != null)
            {
                touchedWall = 0;
                canJump = true;
            }
            else if (hitRight.rigidbody != null && touchedWall == 0 || hitRight.rigidbody != null && touchedWall == 2)
            {
               
                body.gravityScale = 0.2f;
                canJump = true;
                touchedWall = 1;
                Debug.Log(body.gravityScale);
            }
            else if (hitLeft.rigidbody != null && touchedWall == 0 || hitLeft.rigidbody != null && touchedWall == 1)
            {
                
                body.gravityScale = 0.2f;
                canJump = true;
                touchedWall = 2;
                
               
            }
            //else
            //{
            //    body.gravityScale = 1;
            //    canJump = false;
            //    Debug.Log(canJump);
            //}
        }

        if (Input.GetAxis("Jump") > 0.1f && canJump)
        {
            Debug.Log("Jump");
            direction = new Vector2(body.velocity.x, jumpForce);

            //if( touchedWall== 1)
            //{
            //    direction = new Vector2(-40, jumpForce);
            //}
            //else if(touchedWall == 2)
            //{
            //    direction = new Vector2(40, jumpForce);
            //}
            //else if (touchedWall == 0)
            //{
            //    direction = new Vector2(body.velocity.x, jumpForce);
            //}

            canJump = false;
            body.gravityScale = 1;
            timerStopJump = timeStopJump;
        }
    }

    // Update is called once per frame
    void Update()
    {
        direction = new Vector2(Input.GetAxis("Horizontal") * speed, body.velocity.y);

        if (body.velocity.y < -0.1f)
        {
            direction = new Vector2(body.velocity.x, body.velocity.y * 1.1f);
        }
        JumpCheck();
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine((Vector2)transform.position, (Vector2)transform.position + Vector2.down * raycastJumpLength);
    }
        //void OnTriggerStay2D(Collider2D other)
        //{

        //    Debug.Log("Enter");
        //    canJump = true;
        //}

        //void OnTriggerExit(Collider2D other)
        //{

        //    Debug.Log("Leave");
        //    canJump = false;
        //}
    }