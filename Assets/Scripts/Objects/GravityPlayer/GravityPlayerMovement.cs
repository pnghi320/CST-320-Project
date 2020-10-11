using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FixRotation(-FindDownDirection());
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
        transform.rotation = Quaternion.LookRotation(Vector3.forward, upDirection);
    }
}
