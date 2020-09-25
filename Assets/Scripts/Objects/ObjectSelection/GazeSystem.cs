//attached to the main camera
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//the gazesystem class inherents from the MonoBehaviror class
public class GazeSystem : MonoBehaviour {
    
    //public GameObject reticle;

    //public Color inactiveReticleColor = Color.gray;
    //public Color activeReticleColor = Color.green;
    

    //get the type GazeableObject from the other file
    private GazeableObject currentGazeObject;
    //to keep track on what objct do we select (press down)
    private GazeableObject currentSelectedObject;
    //the last sucessful hit when we keep pressing on the button but moving our eye sight to a ungazeable object
    private RaycastHit lastHit;

    // Use this for initialization
    void Start () {
        //when we first start the game the reticle should has gray color since it is not in touch with any interable obj
        //SetReticleColor (inactiveReticleColor);
    }
        
    // Update is called once per frame
    void Update () {
        ProcessGaze ();
        CheckforInput(lastHit);
    }

    public void ProcessGaze()
    {

        Ray raycastRay = new Ray (transform.position, transform.forward);
        RaycastHit hitInfo;
        //raycastRay.direction * 100 because raycastRay.direction is just a very small unit
        Debug.DrawRay (raycastRay.origin, raycastRay.direction * 100);

        if(Physics.Raycast(raycastRay, out hitInfo))
        {
            // Do something to the object

            // Check if the object is interactable

            // Get the GameObject from the hitInfo
            GameObject hitObj = hitInfo.collider.gameObject;

            // Get the GazeableObject from the hit Object, if the obj is not gazable, gazeObj will be NULL
            GazeableObject gazeObj = hitObj.GetComponentInParent<GazeableObject>();

            // Check if object is gazeable
            if (gazeObj != null) 
            {

                // Check if object we're looking at is different from the one we've been looking
                if (gazeObj != currentGazeObject) 
                {
                    ClearCurrentObject ();
                    currentGazeObject = gazeObj;
                    currentGazeObject.OnGazeEnter(hitInfo);
                    //SetReticleColor(activeReticleColor);

                } 
                else 
                {
                    //if we keep looking at the same object, call the OnGaze function
                    currentGazeObject.OnGaze (hitInfo);
                }
            } 
            else 
            {
                //if we're not looking at a gazeable object
                ClearCurrentObject ();
            }
            //the last sucessful hit when we keep pressing on the button but moving our eye sight to a ungazeable object
            lastHit = hitInfo;
        }
        else 
        {
            //if we're not looking at anything
            ClearCurrentObject ();
        }
    }

    /*private void SetReticleColor(Color reticleColor)
    {
        // Set the color of the reticle
        reticle.GetComponent<Renderer>().material.SetColor("_Color", reticleColor);
    }
    */


    private void CheckforInput(RaycastHit hitInfo)
    {

        // Check for down press
        //with GetMouseButtonDown(0), 0 stand for the left mouse click or a quick push on the google cardboard.
        if (Input.GetMouseButtonDown(0) && currentGazeObject != null)
        {
            currentSelectedObject = currentGazeObject;
            currentSelectedObject.OnPress(hitInfo);
        }

        // Check for hold
        // the GetMouseButton(0) check for button position continuously and stop the loop when we release the button
        else if (Input.GetMouseButton(0) && currentSelectedObject != null)
        {
            currentSelectedObject.OnHold(hitInfo);
        }

        // Check for release
        else if (Input.GetMouseButtonUp(0) && currentSelectedObject != null)
        {
            currentSelectedObject.OnRelease(hitInfo);
            currentSelectedObject = null;
        }
    }

    private void ClearCurrentObject()
    {
        if(currentGazeObject != null){
            //finish looking at the current obj
            currentGazeObject.OnGazeExit();
            //change the reticle color to gray
            //SetReticleColor(inactiveReticleColor);
            //clear the current currentGazeObject
            currentGazeObject = null;
        }
    }
}
