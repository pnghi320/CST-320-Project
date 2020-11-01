using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsFreeze : MonoBehaviour
{
    class FreezeData
    {
        public Rigidbody rb;
        public Vector3 initalVelocity;

        public FreezeData(Rigidbody rb)
        {
            this.rb = rb;
            initalVelocity = rb.velocity;
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
            freezeDataObject.rb.isKinematic = false;
            freezeDataObject.rb.velocity = freezeDataObject.initalVelocity;
        }
    }

    public bool IsFrozen()
    {
        return isFrozen;
    }
}

