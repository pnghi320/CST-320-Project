using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FutureOrbitPath : MonoBehaviour
{
    public List<GravityObjectHandler> gravityObjects;
    public int iterations = 1000;
    public float timeStep = 0.02f;
    public float playingPersistTime = 10;
    public bool showWhileEditing = true;
    public bool showWhenPlaying = false;

    void Start()
    {
        gravityObjects = new List<GravityObjectHandler>();
        if (Application.isPlaying && showWhenPlaying)
            DrawPath();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Application.isPlaying && showWhileEditing)
            DrawPath();
    }

    void DrawPath()
    {
        GravityObjectHandler[] gravityObjects = FindObjectsOfType<GravityObjectHandler>();
        GravityObjectData[] gravityObjectsData = new GravityObjectData[gravityObjects.Length];
        Vector3[][] linePoints = new Vector3[gravityObjects.Length][];

        for (int i = 0; i < gravityObjects.Length; i++)
        {
            gravityObjectsData[i] = new GravityObjectData(gravityObjects[i]);
            linePoints[i] = new Vector3[iterations];
        }

        for (int iteration = 0; iteration < iterations; iteration++)
        {
            foreach (GravityObjectData gravityObjectData in gravityObjectsData)
                if (!gravityObjectData.frozen)
                    gravityObjectData.velocity += GravityAcceleration(gravityObjectData, gravityObjectsData) * timeStep;

            for (int i = 0; i < gravityObjectsData.Length; i++)
            {
                GravityObjectData gravityObjectData = gravityObjectsData[i]; 
                if (!gravityObjectData.frozen)
                {
                    Vector3 updatedPosition = gravityObjectData.position + gravityObjectData.velocity * timeStep;
                    gravityObjectData.position = updatedPosition;
                    if (gravityObjectData.showFuturePath)
                        linePoints[i][iteration] = updatedPosition;
                }
            }
        }

        for (int j = 0; j < gravityObjectsData.Length; j++)
        {
            if (gravityObjectsData[j].showFuturePath && !gravityObjectsData[j].frozen)
            {
                Color lineColor = gravityObjects[j].gameObject.GetComponentInChildren<MeshRenderer>().sharedMaterial.color; //

                for (int i = 0; i < linePoints[j].Length - 1; i++)
                    if (Application.isPlaying)
                        Debug.DrawLine(linePoints[j][i], linePoints[j][i + 1], lineColor, playingPersistTime);
                    else
                        Debug.DrawLine(linePoints[j][i], linePoints[j][i + 1], lineColor);
            }
        }
    }

    Vector3 GravityAcceleration(GravityObjectData gravityObjectData, GravityObjectData[] gravityObjectsData)
    {
        Vector3 acceleration = Vector3.zero;
        for (int j = 0; j < gravityObjectsData.Length; j++)
        {
            GravityObjectData otherGravityObjectData = gravityObjectsData[j];
            if (gravityObjectData == otherGravityObjectData || !otherGravityObjectData.affectGravity)
                continue;

            Vector3 direction = otherGravityObjectData.position - gravityObjectData.position;
            float sqrDst = direction.sqrMagnitude;
            acceleration += direction.normalized * otherGravityObjectData.mass / sqrDst;
        }
        return 0.0667428f * acceleration;
    }

    void OnDrawGizmos()
    {
        //DrawPath();
    }

    class GravityObjectData
    {
        public Vector3 position;
        public Vector3 velocity;
        public bool showFuturePath;
        public float mass;
        public bool frozen;
        public bool affectGravity;

        public GravityObjectData(GravityObjectHandler gravityObject)
        {
            position = gravityObject.transform.position;
            velocity = gravityObject.initalVelocity;
            showFuturePath = gravityObject.showFuturePath;
            if (gravityObject.useDensity)
            {
                float totalVolume = Utils.VolumeOfGameObject(gravityObject.gameObject);

                gravityObject.rb.mass = totalVolume * gravityObject.density;
            }
            mass = gravityObject.rb.mass;
            frozen = gravityObject.rb.isKinematic || !gravityObject.moveByGravity;
            affectGravity = gravityObject.affectGravity;
        }
    }
}
