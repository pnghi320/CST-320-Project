using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class GravityObjectHandler : MonoBehaviour
{
    public static List<GravityObjectHandler> gravityObjects;
    public Vector3 initalVelocity;
    public Rigidbody rb;

    const float GRAVITY = 0.0667428f; // 0.0000000000667428;

    void Start()
    {
        rb.velocity = initalVelocity;
    }


    private void OnEnable()
    {
        if (gravityObjects == null)
            gravityObjects = new List<GravityObjectHandler>();
        gravityObjects.Add(this);
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        Vector3 summedAcceleration = Vector3.zero;
        foreach (GravityObjectHandler gravityObject in gravityObjects)
        {
            if (gravityObject != this)
            {
                // F = G * ((m1 * m2) / r^2)
                Vector3 direction = gravityObject.rb.position - rb.position;
                float distance = Vector3.Magnitude(direction);
                distance = Mathf.Clamp(distance, 1f, Mathf.Infinity);
                Vector3 acceleration = (gravityObject.rb.mass / (distance * distance)) * direction.normalized;
                summedAcceleration += acceleration;
            }
        }

        rb.velocity += GRAVITY * summedAcceleration * Time.fixedDeltaTime;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 255, 0.25f);
        Gizmos.DrawSphere(transform.position, 0.75f);
    }
}