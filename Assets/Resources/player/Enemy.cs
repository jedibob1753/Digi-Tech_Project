using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SideToSidePatrol3D : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3f;
    public bool facingRight = true;

    [Header("Detection")]
    public float groundCheckDistance = 1.2f;  // make sure it's long enough
    public float wallCheckDistance = 0.6f;
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public Transform groundCheckPoint;
    public Transform wallCheckPoint;

    [Header("Timing")]
    public float flipCooldownTime = 0.3f;

    private Rigidbody rb;
    private bool canFlip = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        Patrol();

    }

    void Patrol()
    {
        // move in one direction (X-axis)
        rb.velocity = new Vector3((facingRight ? 1 : -1) * moveSpeed, rb.velocity.y, 0f);

        // pick the correct forward direction
        Vector3 forwardDir = facingRight ? Vector3.right : Vector3.left;

        // choose where rays start from
        Vector3 groundOrigin = groundCheckPoint ? groundCheckPoint.position : transform.position;
        Vector3 wallOrigin = wallCheckPoint ? wallCheckPoint.position : transform.position;

        // ground check: cast diagonally down-forward
        bool groundAhead = Physics.Raycast(groundOrigin + forwardDir * 0.4f, Vector3.down, groundCheckDistance, groundLayer);

        // wall check: cast straight forward
        bool wallAhead = Physics.Raycast(wallOrigin, forwardDir, wallCheckDistance, wallLayer);

        // flip if needed (only if cooldown allows)
        if (canFlip && (!groundAhead || wallAhead))
        {
            StartCoroutine(FlipCooldown());
        }
    }

    IEnumerator FlipCooldown()
    {
        canFlip = false;
        Flip();
        yield return new WaitForSeconds(flipCooldownTime);
        canFlip = true;
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector3 forwardDir = facingRight ? Vector3.right : Vector3.left;
        Vector3 groundOrigin = groundCheckPoint ? groundCheckPoint.position : transform.position;
        Vector3 wallOrigin = wallCheckPoint ? wallCheckPoint.position : transform.position;

        // Ground ray: forward + down
        Gizmos.DrawLine(
            groundOrigin + forwardDir * 0.4f,
            groundOrigin + forwardDir * 0.4f + Vector3.down * groundCheckDistance
        );

        // Wall ray: straight forward
        Gizmos.DrawLine(
            wallOrigin,
            wallOrigin + forwardDir * wallCheckDistance
        );
    }
}