using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    //speed for things
    public float moveSpeed = 6f;
    public float jumpForce = 7f;
    //is object facing right variable
    public bool facingRight = true;

    //rigid body and ground detection
    private Rigidbody rb;
    private bool isGrounded;
    
    public float groundCheckDistance = 1.1f;
    public LayerMask groundLayer;

    //wall jump
    private bool canWallJump = false;
    public LayerMask wallLayer;
    public Transform wallCheckPoint;
    public float wallCheckDistance = 0.6f;
    public float wallPush = 5f;
    public float wallJumpForce = 10f;
    public float wallJumpHorizontalForce = 8f;
    public float wallJumpTime = 0.2f;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        CheckGrounded();
        CheckWall();

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        move = transform.TransformDirection(move) * moveSpeed;
        move.y = rb.velocity.y;

        if (move.x < 0 && facingRight)
        {
            Flip();
        }
        else if (move.x > 0 && !facingRight)
        {
            Flip();
        }


            rb.velocity = move;

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        if (Input.GetButtonDown("Jump") && !isGrounded && (canWallJump == true))
        {
            rb.velocity = Vector3.zero;

            // Direction away from wall (Mario-style "kick")
            float wallDir = facingRight ? -1f : 1f;

            // Stronger horizontal (3) and higher vertical (1.8) for that diagonal launch
            Vector3 jumpDirection = new Vector3(wallDir * 3f, 1.8f, 0f).normalized;

            // Use a high impulse force for strong acceleration
            float wallJumpStrength = jumpForce * 3.2f;

            rb.AddForce(jumpDirection * wallJumpStrength, ForceMode.Impulse);

            // Optional: slight upward boost after impulse for smoother arc
            rb.AddForce(Vector3.up * jumpForce * 0.5f, ForceMode.Impulse);

            // Optional: short input lock for crisp control
            StartCoroutine(WallJumpControlLock(3f));

        }
               
      
        // If we're against a wall and not grounded, reduce or stop horizontal input
        if (!isGrounded && canWallJump)
        {
            // Stop pushing into wall
            float moveDir = Input.GetAxisRaw("Horizontal");

            // If moving *into* the wall, cancel it
            if ((facingRight && moveDir > 0) || (!facingRight && moveDir < 0))
            {
                move.x = 0;
            }

            // Optional: add a slow slide effect instead of full stop
            rb.velocity = new Vector3(move.x, Mathf.Max(rb.velocity.y, -2f), 0);
        }
        else
        {
            rb.velocity = move;
        }

        IEnumerator WallJumpControlLock(float duration)
        {
            float timer = 0f;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                // Prevent horizontal input for a split second (feels more intentional)
                rb.velocity = new Vector3(rb.velocity.x * 0.9f, rb.velocity.y, 0);
                yield return null;
            }
        }
    }

    void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        facingRight = !facingRight;
    }

  
     
    void CheckGrounded()
    {
        float groundCheckRadius = 0.4f;
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.SphereCast(ray, groundCheckRadius, out hit, groundCheckDistance, groundLayer))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    void CheckWall()
    {
        Vector3 forwardDir = facingRight ? Vector3.right : Vector3.left;
        Vector3 wallOrigin = wallCheckPoint ? wallCheckPoint.position : transform.position;

        bool wallAhead = Physics.Raycast(wallOrigin, forwardDir, wallCheckDistance, wallLayer);
        if (!isGrounded && wallAhead)
        {
            canWallJump = true;

        } else
        {
            canWallJump = false;
        }

    }

}