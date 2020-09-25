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
        {
            DrawPath();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!Application.isPlaying && showWhileEditing)
        {
            DrawPath();
        }
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
                if (!gravityObjectsData[i].frozen)
                {
                    Vector3 updatedPosition = gravityObjectsData[i].position + gravityObjectsData[i].velocity * timeStep;
                    gravityObjectsData[i].position = updatedPosition;
                    if (gravityObjectsData[i].showFuturePath)
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
            if (gravityObjectData == gravityObjectsData[j])
                continue;

            Vector3 direction = gravityObjectsData[j].position - gravityObjectData.position;
            float sqrDst = direction.sqrMagnitude;
            acceleration += direction.normalized * gravityObjectsData[j].mass / sqrDst;
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

        public GravityObjectData(GravityObjectHandler gravityObject)
        {
            position = gravityObject.transform.position;
            velocity = gravityObject.initalVelocity;
            showFuturePath = gravityObject.showFuturePath;
            if (gravityObject.useDensity)
            {
                Transform[] children = gravityObject.GetComponentsInChildren<Transform>();
                float totalVolume = 1;
                foreach (MeshFilter meshFilter in gravityObject.gameObject.GetComponentsInChildren<MeshFilter>())
                {
                    Vector3 objectSize = meshFilter.transform.localScale;
                    float volume = objectSize[0] * objectSize[1] * objectSize[2];
                    totalVolume *= Utils.VolumeOfMesh(meshFilter.mesh) * volume;
                }

                gravityObject.rb.mass = totalVolume * gravityObject.density;
            }
            mass = gravityObject.rb.mass;
            frozen = gravityObject.rb.isKinematic;
        }
    }
}
