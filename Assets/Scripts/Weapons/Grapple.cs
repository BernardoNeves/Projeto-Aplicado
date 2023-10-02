using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    //public GameObject player;
    public GameObject grappleOriginLnR;
    public GameObject grappleOriginUp;
    public Rigidbody2D PlayerRB;

    private GameObject target;
    private GameObject grappleOrigin;
    private Vector2 grappleDirection;
    private Vector2 targetPosition;
    private Vector2 targetWidthOrHeight;
    private float grappleDirectionCheck;
    private bool canGrapple = true;
    private bool doGrapple = false;
    //private Vector2 grapplePoint;
    //private bool isGrappling = false;

    public float grappleRange = 10f;
    public float grappleSpeed = 15f;
    public float grappleCoolDown = 1f;
    public float grappleTimer = 1f;
    public float grappleHoldTimer = 1f;
    public float grappleSideForce = 1000f;
    public float grappleSideForceMultiplier = 150f;

    private void Update()
    {
        // Find the direction the player is facing to throw the grapple

        grappleSideForce = grappleRange * grappleSideForceMultiplier;

        GrappleTimeCounter();

        Debug.Log(grappleTimer);

        if (Input.GetKey(KeyCode.W))
        {
            grappleOrigin = grappleOriginUp;
            grappleDirection = new Vector2(0f, grappleOrigin.transform.position.y - transform.position.y);

            // Perform the raycast and store the result in a RaycastHit2D object
            RaycastHit2D hitUp = Physics2D.Raycast(grappleOrigin.transform.position, grappleDirection, grappleRange);

            if (hitUp == false)
            {
                target = null;
            }
            else
            {
                target = hitUp.collider.gameObject;

                if (target.CompareTag("Wall") || target.CompareTag("Enemy"))
                {
                    if (Input.GetKey(KeyCode.Q) && canGrapple)
                    {
                        PlayerRB.gravityScale = -grappleSpeed;
                        targetWidthOrHeight = GetTargetWidthOrHeight(hitUp.collider.gameObject, 0);
                        targetPosition = GetTargetPosition(transform.position.x, hitUp.collider.gameObject.transform.position.y, targetWidthOrHeight);
                        GrapplePull(transform.position, targetPosition);
                        grappleTimer = 0f;
                    }
                    if (Input.GetKeyUp(KeyCode.Q))
                    {
                        PlayerRB.gravityScale = 5f;
                    }

                }
            }
        }
        else 
        {
            //PlayerRB.gravityScale = 5f;
            grappleOrigin = grappleOriginLnR;
            grappleDirection = new Vector2(grappleOrigin.transform.position.x - transform.position.x, 0f);

            // Perform the raycast and store the result in a RaycastHit2D object
            RaycastHit2D hit = Physics2D.Raycast(grappleOrigin.transform.position, grappleDirection, grappleRange);

            if (hit == false) 
            {
                target = null;
            }
            else
            {
                target = hit.collider.gameObject;

                if (target.CompareTag("Wall") || target.CompareTag("Enemy"))
                {
                    if (Input.GetKey(KeyCode.Q) && canGrapple)
                    {
                        PlayerRB.velocity = Vector2.zero;
                        grappleDirectionCheck = GetGrappleDirectionCheck(targetPosition, transform.position);
                        targetWidthOrHeight = GetTargetWidthOrHeight(target, grappleDirectionCheck);
                        targetPosition = GetTargetPosition(target.transform.position.x, transform.position.y, targetWidthOrHeight);
                        GrapplePull(transform.position, targetPosition);

                        PlayerRB.AddForce(new Vector2(targetPosition.x - transform.position.x, targetPosition.y - transform.position.y) * grappleSideForce);

                        grappleTimer = 0f;
                    }
                }
            }
        }
    }

    public void GrappleTimeCounter() 
    {
        if (grappleTimer < grappleCoolDown)
        {
            grappleTimer += Time.deltaTime;
            canGrapple = false;
        }
        if (grappleTimer >= grappleCoolDown)
        {
            canGrapple = true;
        }
    }
    
    public void GetGrappleTarget(RaycastHit2D hit) 
    {
        if ((hit.collider.gameObject.tag == "Wall" || hit.collider.gameObject.tag == "Enemy")
            && Input.GetKey(KeyCode.Q))
        {
            grappleDirectionCheck = GetGrappleDirectionCheck(targetPosition, transform.position);
            targetWidthOrHeight = GetTargetWidthOrHeight(hit.collider.gameObject, grappleDirectionCheck);
            targetPosition = GetTargetPosition(hit.collider.gameObject.transform.position.x, transform.position.y, targetWidthOrHeight);
            GrapplePull(transform.position, targetPosition);
        }
    }

    public Vector2 GetTargetPosition(float xTarget, float yTarget,Vector2 widthOrHeight)
    {
        Vector2 position = new Vector2(xTarget + widthOrHeight.x, yTarget + widthOrHeight.y);
        return position;
    }

    public Vector2 GetTargetWidthOrHeight(GameObject target, float playerDirection) 
    {
        Renderer objectRenderer = target.GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            if (playerDirection !=0)
            {
                // Get the width (x-axis size) of the object
                float objectWidth = objectRenderer.bounds.size.x / 2;

                if (grappleDirectionCheck > 0)
                {
                    objectWidth = -objectWidth;
                }

                return new Vector2(objectWidth, 0);
            }
            else
            {
                float objectHeight = objectRenderer.bounds.size.y / 2;
                objectHeight = -objectHeight;
                return new Vector2(0, objectHeight);
            }
        }
        else
        {
            Debug.LogWarning("Renderer component not found on the hit object.");
            return new Vector2(0,0);
        }
    }

    public float GetGrappleDirectionCheck(Vector2 target, Vector2 currentPosition) 
    {
        float directionCheck = target.x - currentPosition.x;
        return directionCheck;
    }

    public void GrapplePull(Vector2 cPosition, Vector2 tPosition) 
    {
        float distanceToTarget = Vector2.Distance(cPosition, tPosition);

        // Calculate the step based on the grappleSpeed
        float step = grappleSpeed * Time.deltaTime;

        // Make sure step is never greater than 1 to prevent overshooting
        step = Mathf.Clamp01(step / distanceToTarget);

        // Lerp only if the distance to the target is greater than a small threshold
        if (distanceToTarget > 0.1f)
        {
            transform.position = Vector2.Lerp(cPosition, tPosition, step);
        }
    }
}
