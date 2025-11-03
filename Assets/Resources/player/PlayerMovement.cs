using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float jumpForce = 7f;
    public float groundCheckDistance = 1.1f;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private bool isGrounded;
    public bool facingRight = true;

    private bool canWallJump;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        CheckGrounded();

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

}