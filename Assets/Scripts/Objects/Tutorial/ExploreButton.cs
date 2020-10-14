using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExploreButton : MonoBehaviour
{
    Camera mainCam;
    public Text theText;
    public Button theButton;
    Color32 activeColor = new Color32(161, 231, 106, 255);

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
		Button btn = theButton.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void TaskOnClick(){
        //enable the CameraPerspective attached to the main camera
        mainCam.GetComponent<CameraPerspective>().enabled = true;
        //set player mode to: Walk
        Player.instance.activeMode = InputMode.WALK;
        //change the button text color
        theText.color = activeColor;
        //
        

	}
}
