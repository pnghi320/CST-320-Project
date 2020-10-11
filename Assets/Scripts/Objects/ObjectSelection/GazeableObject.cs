//Attached to object of the screen that users can interact
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GazeableObject : MonoBehaviour
{
    //some obj we can't transform and some we can't
    public bool isTransformable = false;
    //make a variable that we can store object layer so that we're done transforming them, we can turn it back from ignoreRaycast to normal
    private int objectLayer;
    //We know that int for IGNORE_RAYCAST_LAYER is 2 based on the index that it was given in unity
    private const int IGNORE_RAYCAST_LAYER = 2;
    //the obj's initial rotation before we rotate it
    private Vector3 initialObjectRotation;
    //the player's initial rotation
    private Vector3 initialPlayerRotation;
    //keep track if the inition scale of the obj before the transformation
    private Vector3 initialObjectScale;





    //when we start to gaze/look at the object
    //include "virtual" so that the method can be overwritten by a child, later we will make other gazable obj that react differently when being gazed
    public virtual void OnGazeEnter(RaycastHit hitInfo)
    {
        //this will print the sentence "Gaze entered on <object>" in the debug section in unity when we interact in the 3d enviroment 
        Debug.Log("Gaze entered on " + gameObject.name);
    }

    //when we continue looking at the obj
    public virtual void OnGaze(RaycastHit hitInfo)
    {
        //same thing, print out sentence in the debug section
        Debug.Log("Gaze hold on " + gameObject.name);
    }

    public virtual void OnRaycastHold(RaycastHit hitInfo)
    {
        //same thing, print out sentence in the debug section
        Debug.Log("Raycast holds on " + gameObject.name);
    }
    public virtual void OnRaycastEnter(RaycastHit hitInfo)
    {
        //same thing, print out sentence in the debug section
        Debug.Log("Raycast enters on " + gameObject.name);
    }


    //when we are no longer looking at the obj
    public virtual void OnGazeExit()
    {
        Debug.Log("Gaze exited on " + gameObject.name);
    }

    public virtual void OnPress(RaycastHit hitInfo)
    {
        Debug.Log("Botton pressed on " + gameObject.name);
        //if we're in transform mode, this will make the movement look smoother
        if (isTransformable)
        {
            //for translate mode
            //save the original layer of the obj
            objectLayer = gameObject.layer;
            //assign the lay to ignoreRaycast
            gameObject.layer = IGNORE_RAYCAST_LAYER;

            //for rotate mode, set the initialObjectRotation and the initialPlayerRotation
            initialObjectRotation = transform.rotation.eulerAngles;
            initialPlayerRotation = Camera.main.transform.eulerAngles;
        }
        //if player is in CREATE mode then run the CreatePlanet function
        if (Player.instance.activeMode == InputMode.CREATE)
        {
            CreatePlanet(hitInfo);
        }

    }

    public virtual void OnHold(RaycastHit hitInfo)
    {
        Debug.Log("Button held on " + gameObject.name);
        if (isTransformable)
        {
            GazeTransform(hitInfo);
        }
    }

    public virtual void OnRelease(RaycastHit hitInfo)
    {
        Debug.Log("Botton released on " + gameObject.name);
        if (isTransformable)
        {
            gameObject.layer = objectLayer;
        }
    }

    //for moving the obj
    public virtual void GazeTranslate(RaycastHit hitInfo)
    {
        //make sure that we looking at a collider and it is the floor
        if (hitInfo.collider != null && hitInfo.collider.GetComponent<Floor>())
        {
            //transform.position is the postion of the obj
            //make it equal to the point of the Raycast
            var pos = transform.position;
            pos.z = hitInfo.point.z;
            pos.x = hitInfo.point.x;
            pos.y = hitInfo.point.y + 10;
            transform.position = pos;
        }
    }

    //for rotating the obj
    public virtual void GazeRotate(RaycastHit hitInfo)
    {
        //speed of the obj rotation
        float rotationSpeed = 2.0f;
        //access to the player current rotation of the moment
        Vector3 currentPlayerRotation = Camera.main.transform.rotation.eulerAngles;
        //access to the object current rotation of the moment
        Vector3 currentObjectRotation = transform.rotation.eulerAngles;
        //calculate the angle that we move our head
        Vector3 rotationDelta = currentPlayerRotation - initialPlayerRotation;
        //create the variable that hold the new position of the object being rotated
        Vector3 newRotation = new Vector3(currentObjectRotation.x, currentObjectRotation.y + (rotationDelta.y * rotationSpeed), currentObjectRotation.z);
        //assign the new rotation to the obj's rotation.
        transform.rotation = Quaternion.Euler(newRotation);

    }
    //for creating new planet
    public virtual void CreatePlanet(RaycastHit hitInfo)
    {
        GameObject placedPlanet = GameObject.Instantiate(Player.instance.activePlanetPrefab) as GameObject;
        var pos = transform.position;
        pos.z = hitInfo.point.z;
        pos.x = hitInfo.point.x;
        pos.y = hitInfo.point.y + 10;
        placedPlanet.transform.position = pos;
    }

    //for scalling the obj
    public virtual void GazeScale(RaycastHit hitInfo)
    {
        float scaleSpeed = 0.01f;
        //if the scale factor = 2, the obj grows twice as big. The obj scale starts at 1
        float scaleFactor = 1;
        //access to the player current rotation of the moment
        Vector3 currentPlayerRotation = Camera.main.transform.rotation.eulerAngles;
        //calculate the angle that we move our head
        Vector3 rotationDelta = currentPlayerRotation - initialPlayerRotation;
        //get the initial obj scale from unity transform component
        initialObjectScale = transform.localScale;
        //if we're looking up
        if (rotationDelta.x < 0 && rotationDelta.x > -180.0f || rotationDelta.x > 180.0f && rotationDelta.x < 360.0f)
        {
            //if greater than 180 map it between -180 and 180 
            if (rotationDelta.x > 180.0f)
            {
                rotationDelta.x = 360.0f - rotationDelta.x;
            }
            //map the rotationDelta from 0 to 180 by using the absolute function
            //calculate the how big we scale the obj
            scaleFactor = 1.0f + Mathf.Abs(rotationDelta.x) * scaleSpeed;

        }
        else
        {
            //map it from -180 to 180
            if (rotationDelta.x < -180.0f)
            {
                rotationDelta.x = 360.0f + rotationDelta.x;
            }
            //calculate the how small we scale the obj, the scale factor can't be smaller than 0.1 ortherwise the obj disapear
            scaleFactor = Mathf.Max(0.9f, 1.0f - (Mathf.Abs(rotationDelta.x) * (scaleSpeed * 10)) / 180.0f);
        }
        //set the object scale
        transform.localScale = scaleFactor * initialObjectScale;
    }


    //Calling the correct transform function
    public virtual void GazeTransform(RaycastHit hitInfo)
    {
        switch (Player.instance.activeMode)
        {
            case InputMode.TRANSLATE:
                GazeTranslate(hitInfo);
                break;
            case InputMode.ROTATE:
                GazeRotate(hitInfo);
                break;
            case InputMode.SCALE:
                GazeScale(hitInfo);
                break;
        }
    }
}

