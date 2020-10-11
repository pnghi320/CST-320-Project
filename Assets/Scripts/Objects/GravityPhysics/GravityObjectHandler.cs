using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[SelectionBase]
public class GravityObjectHandler : MonoBehaviour
{
    public static List<GravityObjectHandler> gravityObjects;

    public Vector3 initalVelocity;
    public Rigidbody rb;
    public bool useDensity = false;
    public float density = 10;
    public bool moveByGravity = true;
    public bool affectGravity = true;
    public bool showFuturePath = true;
    public bool showHistoricPath = false;

    float objectVolume = 0;

    const float GRAVITY = 0.0667428f; // 0.0000000000667428;
    void Start()
    {
        rb.velocity = initalVelocity;

        if (useDensity)
        {
            Transform[] children = GetComponentsInChildren<Transform>();
            float totalVolume = 1;
            foreach (MeshFilter meshFilter in gameObject.GetComponentsInChildren<MeshFilter>())
            {
                Vector3 objectSize = meshFilter.transform.localScale;
                float volume = objectSize[0] * objectSize[1] * objectSize[2];
                totalVolume *= Utils.VolumeOfMesh(meshFilter.mesh) * volume;
            }

            objectVolume = totalVolume;

            rb.mass = totalVolume * density;
        }
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
        if (rb.isKinematic || !moveByGravity) return;
        Vector3 summedAcceleration = Vector3.zero;
        foreach (GravityObjectHandler gravityObject in gravityObjects)
        {
            if (gravityObject != this && gravityObject.affectGravity)
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
        if (!Application.isPlaying)
            DrawArrow.ForGizmo(transform.position, initalVelocity, Color.black);
        else
            DrawArrow.ForGizmo(transform.position, rb.velocity, Color.black);
    }
}