using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    //public GameObject player;
    public GameObject grappleOriginLnR;
    public GameObject grappleOriginUp;

    private bool isGrappling = false;
    private Vector2 grappleDirection;
    private GameObject grappleOrigin;
    private Vector2 grapplePoint;
    private float objectWidth;

    public float grappleRange = 10f;
    public float grappleSpeed = 5f;

    private void Update()
    {
        // Find the direction the player is facing to throw the grapple

        if (Input.GetKey(KeyCode.W))
        {
            grappleOrigin = grappleOriginUp;
            grappleDirection = new Vector2(0f, grappleOrigin.transform.position.y - transform.position.y);

            // Perform the raycast and store the result in a RaycastHit2D object
            RaycastHit2D hitUp = Physics2D.Raycast(grappleOrigin.transform.position, grappleDirection, grappleRange);

            // Draw the ray as a debug line
            Debug.DrawRay(grappleOrigin.transform.position, grappleDirection * grappleRange, Color.green);

            GrappelPull(hitUp);
        }
        else 
        {
            grappleOrigin = grappleOriginLnR;
            grappleDirection = new Vector2(grappleOrigin.transform.position.x - transform.position.x, 0f);

            // Perform the raycast and store the result in a RaycastHit2D object
            RaycastHit2D hit = Physics2D.Raycast(grappleOrigin.transform.position, grappleDirection, grappleRange);

            // Draw the ray as a debug line
            Debug.DrawRay(grappleOrigin.transform.position, grappleDirection * grappleRange, Color.green);

            GrappelPull(hit);
        }
    }

    public void GrappelPull(RaycastHit2D hit) 
    {
        if ((hit.collider.gameObject.tag == "Wall" || hit.collider.gameObject.tag == "Enemy") && Input.GetKey(KeyCode.Q))
        {
            Renderer objectRenderer = hit.collider.gameObject.GetComponent<Renderer>();
            if (objectRenderer != null)
            {
                // Get the width (x-axis size) of the object
                objectWidth = objectRenderer.bounds.size.x;
                Debug.Log("Width of the hit object: " + objectWidth);
            }
            else
            {
                Debug.LogWarning("Renderer component not found on the hit object.");
            }

            objectWidth = objectWidth * grappleDirection.x;

            Vector2 targetPosition = new Vector2 (hit.collider.gameObject.transform.position.x + objectWidth, hit.collider.gameObject.transform.position.y);

            Vector2 currentPosition = transform.position;

            float distanceToTarget = Vector2.Distance(currentPosition, targetPosition);
            float step = grappleSpeed * Time.deltaTime;

            transform.position = Vector2.Lerp(currentPosition, targetPosition, Mathf.Clamp01(step / distanceToTarget));

            isGrappling = true;
            grapplePoint = hit.point;


        }

        if (isGrappling)
        {
            Vector2 targetPosition = grapplePoint;
            Vector2 currentPosition = transform.position;
            float distanceToTarget = Vector2.Distance(currentPosition, targetPosition);

            if (Input.GetKey(KeyCode.Q))
            {
                // Continue to stick to the wall
                float step = grappleSpeed * Time.deltaTime;
                transform.position = Vector2.Lerp(currentPosition, targetPosition, Mathf.Clamp01(step / distanceToTarget));
            }
            else
            {
                // Release the grapple
                isGrappling = false;
            }
        }
    }
}
