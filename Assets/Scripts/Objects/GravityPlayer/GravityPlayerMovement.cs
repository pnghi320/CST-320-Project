using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPlayerMovement : MonoBehaviour
{
    public float distanceToGround = 0.1f;

    bool IsGrounded(Vector3 downDirection)
    {
        Debug.DrawRay(transform.position, downDirection * distanceToGround, Color.red);
        return Physics.Raycast(transform.position, downDirection, distanceToGround);
    }

    void Update()
    {
        Vector3 downDirection = FindDownDirection();
        Debug.Log(downDirection);
        if (IsGrounded(downDirection))
        {
            GetComponent<GravityObjectHandler>().moveByGravity = false;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        } else
        {
            GetComponent<GravityObjectHandler>().moveByGravity = true;
        }
        //Debug.Log(GetComponent<GravityObjectHandler>().moveByGravity);
        FixRotation(-downDirection);
    }

    Vector3 FindDownDirection()
    {
        GravityObjectHandler thisGravityHandler = GetComponent<GravityObjectHandler>();

        GravityObjectHandler[] gravityObjects = FindObjectsOfType<GravityObjectHandler>();
        if (gravityObjects.Length == 0) return Vector3.zero;
        float maxAttraction = 0;
        Vector3 maxAttractorLocation = Vector3.zero;
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
                maxAttractorLocation = gravityObject.transform.position;
            }
        }

        return (maxAttractorLocation - transform.position).normalized;
    }

    void FixRotation(Vector3 upDirection)
    {
        transform.rotation = Quaternion.LookRotation(Vector3.Cross(Vector3.forward, upDirection), upDirection);
    }
}
