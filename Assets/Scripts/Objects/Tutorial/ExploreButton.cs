using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExploreButton : MonoBehaviour
{
    Text text;
    Button button;
    Camera mainCam;
    Color activeColor = new Color32(161, 231, 106, 255);
    Color inactiveColor;
    bool isExploring = false;

    void Start()
    {
        mainCam = Camera.main;
        text = GetComponentInChildren<Text>();
        inactiveColor = text.color;
        button = GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick(){
        //enable the CameraPerspective attached to the main camera
        //set player mode to: Walk
        if (isExploring)
        {
            mainCam.GetComponent<CameraPerspective>().enabled = false;
            Player.instance.activeMode = InputMode.NONE;
            //change the button text color
            text.color = inactiveColor;
        } else
        {
            mainCam.GetComponent<CameraPerspective>().enabled = true;
            Player.instance.activeMode = InputMode.WALK;
            //change the button text color
            text.color = activeColor;
        }
        isExploring = !isExploring;
	}
}
