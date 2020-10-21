using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPlayerMovement : MonoBehaviour
{
    public float distanceToGround = 0.1f;
    public float speed = 5f;
    public float spaceForce = 50f; // I just wanted a variable named space force

    bool IsGrounded(Vector3 downDirection)
    {
        return Physics.Raycast(transform.position, downDirection, distanceToGround);
    }

    void Update()
    {
        Vector3 downDirection = FindDownDirection();

        FixRotation(-downDirection);
        if (IsGrounded(downDirection))
        {
            GetComponent<GravityObjectHandler>().moveByGravity = false;
            Rigidbody rb = GetComponent<Rigidbody>();
            Vector3 velocity = speed * (Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                velocity += transform.up * spaceForce / rb.mass;
            }
            rb.velocity = velocity;//+ GetMaxAttractor().lastVelocityChange;
        }
        else
        {
            GetComponent<GravityObjectHandler>().moveByGravity = true;
        }
    }

    GravityObjectHandler GetMaxAttractor()
    {
        GravityObjectHandler thisGravityHandler = GetComponent<GravityObjectHandler>();

        GravityObjectHandler[] gravityObjects = FindObjectsOfType<GravityObjectHandler>();
        float maxAttraction = 0;
        GravityObjectHandler maxAttractor = null;
        foreach (GravityObjectHandler gravityObject in gravityObjects)
        {
            if (gravityObject == thisGravityHandler) continue;
            Vector3 direction = gravityObject.rb.position - transform.position;
            float distance = Vector3.Magnitude(direction);
            distance = Mathf.Clamp(distance, 1f, Mathf.Infinity);
            float attraction = (gravityObject.rb.mass / (distance * distance));
            if (attraction > maxAttraction)
            {
                maxAttraction = attraction;
                maxAttractor = gravityObject;
            }
        }

        return maxAttractor;
    }

    Vector3 FindDownDirection()
    {
        Vector3 maxAttractorLocation = GetMaxAttractor().transform.position;

        return (maxAttractorLocation - transform.position).normalized;
    }

    void FixRotation(Vector3 upDirection)
    {
        transform.rotation = Quaternion.LookRotation(Vector3.Cross(Vector3.forward, upDirection), upDirection);
    }
}
