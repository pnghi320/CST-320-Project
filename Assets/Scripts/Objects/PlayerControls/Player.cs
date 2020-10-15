using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//keep track of what mood we are in
//enum : set of name constant
public enum InputMode{
    NONE,
    WALK,
    CREATE,
    TRANSLATE,
    ROTATE,
    SCALE,
}


//use singleton design pattern: only one instance of the class appears
public class Player : MonoBehaviour
{
    //you can access this instance anywhere in the code by using Player.instance
    public static Player instance;
    public InputMode activeMode = InputMode.NONE;
    //keep track of what kind of planet is being created during the game
    public Object activePlanetPrefab;
    
    //Set the speed of the player while walking
    public float playerSpeed = 5;

    public bool mouseMovementMode = true;

    //This runs before start to make sure there is only one player object in the game
    //can run even if the script component is unchecked, unlike Start(). 
    void Awake(){
        //this should not happen because there is only one player object in the game. But just in case that there are two
        //we will destroy the 2nd player.
        if (instance != null){
            Destroy(instance.gameObject);
        }
        instance = this;
    }


    // Update is called once per frame
    void Update()
    {
        TryWalk();
    }

    //check if the trigger is being held, if we're in the right mode to walk, or if we'll be within bound later
    public void TryWalk(){
        if (activeMode == InputMode.WALK)
        {
            if (mouseMovementMode)
            {
                Vector3 forward = Camera.main.transform.forward;
                Vector3 newPosition = transform.position + forward * Time.deltaTime * playerSpeed;
                transform.position = newPosition;
            }
            else
            {
                Transform camTransform = Camera.main.transform;
                Vector3 direction = Input.GetAxis("Horizontal") * camTransform.right + Input.GetAxis("Vertical") * camTransform.forward + Input.GetAxis("UpDown") * camTransform.up;

                Vector3 newPosition = transform.position + (direction * Time.deltaTime * playerSpeed);
                transform.position = newPosition;
            }
        }
    }
}
