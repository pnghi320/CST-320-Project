using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsFreeze : MonoBehaviour
{
    class FreezeData
    {
        Rigidbody rigidbody;
        Vector3 initalVelocity;
        Vector3 initalAngularVelocity;

        public FreezeData(Rigidbody rigidbody)
        {
            this.rigidbody = rigidbody;
            initalVelocity = rigidbody.velocity;
            initalAngularVelocity = rigidbody.angularVelocity;
        }

        public Rigidbody GetRigidbody()
        {
            return rigidbody;
        }

        public Vector3 GetInitialVelocity()
        {
            return initalVelocity;
        }

        public Vector3 GetInitialAngularVelocity()
        {
            return initalAngularVelocity;
        }
    }

    public static PhysicsFreeze instance;
    bool isFrozen = false;

    List<FreezeData> freezeDataObjects;

    void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        instance = this;
    }

    private void Start()
    {
        freezeDataObjects = new List<FreezeData>();
    }

    public void Freeze()
    {
        if (isFrozen)
        {
            Debug.LogWarning("Tried to freeze when already frozen");
            return;
        }

        isFrozen = true;

        Rigidbody[] rigidbodies = FindObjectsOfType(typeof(Rigidbody)) as Rigidbody[];
        
        foreach (var rigidbody in rigidbodies)
        {
            if (rigidbody.isKinematic) continue;
            
            freezeDataObjects.Add(new FreezeData(rigidbody));
            rigidbody.isKinematic = true;
        }
    }

    public void ClearFreeze()
    {
        if (!isFrozen)
        {
            Debug.LogWarning("Tried to clear freeze when not frozen");
            return;
        }

        isFrozen = false;

        foreach (var freezeDataObject in freezeDataObjects)
        {
            Rigidbody freezeDataRigidbody = freezeDataObject.GetRigidbody();
            freezeDataRigidbody.isKinematic = false;
            freezeDataRigidbody.velocity = freezeDataObject.GetInitialVelocity();
            freezeDataRigidbody.angularVelocity = freezeDataObject.GetInitialAngularVelocity();
        }
    }

    public bool IsFrozen()
    {
        return isFrozen;
    }
}

