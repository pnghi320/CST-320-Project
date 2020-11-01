using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExploreButton : MonoBehaviour
{
    Text text;
    Button button;
    Color activeColor = new Color32(161, 231, 106, 255);
    Color inactiveColor;
    public bool isExploring = false;
    public bool changedMode = false;
    public GameObject player;
    void Start()
    {
        text = GetComponentInChildren<Text>();
        inactiveColor = text.color;
        button = GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {


        //enable the CameraPerspective attached to the main camera
        //set player mode to: Walk
        if (isExploring)
        {
            player.GetComponent<CameraController>().enabled = true;
            player.GetComponent<CameraPerspective>().enabled = false;
            Player.instance.activeMode = InputMode.NONE;
            //change the button text color
            text.color = inactiveColor;
            isExploring = false;

            PhysicsFreeze.instance.ClearFreeze();
        }
        else
        {
            changedMode = true;
            text.color = activeColor;
            PhysicsFreeze.instance.Freeze();
            Invoke("DelayedFunction", 1.0f);
        }
    }
    void DelayedFunction()
    {
        Player.instance.activeMode = InputMode.WALK;
        player.GetComponent<CameraController>().enabled = false;
        player.GetComponent<CameraPerspective>().enabled = true;
        isExploring = true;
    }
}
