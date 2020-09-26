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
    //SerializeField makes private or protected variables visible in the inspector, so that we can still change it in unity 
    //Set the speed of the player while walking
    [SerializeField]
    private float playerSpeed = 5;
    //This runs before start to make sure there is only one player object in the game
    //can run even if the script component is unchecked, unlike Start(). 
    void Awake(){
        //this should not happen because there is only one player object in the game. But just in case that there are two
        //we will destroy the 2nd player.
        if (instance != null){
            GameObject.Destroy(instance.gameObject);
        }
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        TryWalk();
    }

    //check if the trigger is being held, if we're in the right mode to walk, or if we'll be within bound later
    public void TryWalk(){
        if (Input.GetMouseButton(0) && activeMode == InputMode.WALK)
        {
            //create a vector3 coming straight out of the raycast 
            //the vector3 variable returns 3 floats that represent the position x, y, z of the obj
            Vector3 forward = Camera.main.transform.forward;
            //transformPosition is our current position right now; forward is the the distance bw our postion and the destination; forward * Time.deltaTime 
            //is to make up for the delay if the computer is laggy (will arrive at the same place with the same time); Time.deltaTime * playerSpeed 
            //is so that we can control the speed more easily
            Vector3 newPosition = transform.position + forward * Time.deltaTime * playerSpeed;
            transform.position = newPosition;
        }
    }
}
