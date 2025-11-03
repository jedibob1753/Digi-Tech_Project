using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeOrbit : MonoBehaviour
{
    public Transform player;     // Reference to player
    public PlayerMovement playerMovement;

    public float orbitRadius = 3f; // Fixed radius from player
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        // Get mouse position in world space
        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(
        new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane + 10f));

        // Lock Z, x&y only 
        mouseWorldPos.z = player.position.z;

        // Direction from player to mouse
        Vector3 dir = (mouseWorldPos - player.position).normalized;
        
         


       

        // Always place cube exactly orbitRadius away, will move on circle outline instead of in a circle area 
        Vector3 finalPos = player.position + dir * orbitRadius;

        // Fix Z position so no movement on z axis
        finalPos.z = player.position.z;

        // Move cube
        transform.position = finalPos;
        

        // Optional: make cube face outward or toward player
        //
    }
   
}
