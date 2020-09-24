using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class is for the teleport function of our game
//inherent from GazeableObject
public class Floor : GazeableObject
{
    public override void OnPress(RaycastHit hitInfo)
    {
        //The goal is when we press on the headset, we will teleport to the point we're looking at
        //print debug message 
        base.OnPress(hitInfo);
        //if the player hit the teleport button once, choosing the teleport mode
        if (Player.instance.activeMode == InputMode.TELEPORT)
        {
            //from the hitInfo, we can extract the location of point that the raycast is looking at.
            //we store that location in the vector3 variable called destLocation
            Vector3 destLocation = hitInfo.point;
            //so the height of the player is that same when he teleports to the new position
            destLocation.y =  Player.instance.transform.position.y;
            //access the player position in the game and change it to the destLocation
            Player.instance.transform.position = destLocation;

        }
        else if(Player.instance.activeMode == InputMode.CREATE){
            //create the piece of furniture
            //GameObject.Instantiate takes in a prefab and turn that into a GameObject by creating it in the scene (adding the obj to the hyrache)
            //since GameObject.Instantiate(Player.instance.activeFurniturePrefab) only returns an Object, we add "as GameObject" so that it will return GameObject instead
            GameObject placedFurniture =  GameObject.Instantiate(Player.instance.activeFurniturePrefab) as GameObject;
            //set the position of the furniture
            placedFurniture.transform.position = hitInfo.point;
        }
    }

}
