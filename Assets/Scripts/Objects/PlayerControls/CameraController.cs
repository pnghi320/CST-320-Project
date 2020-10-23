using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform[] views;
    public float transitionSpeed;
    Transform currentView;

    // Use this for initialization
    void Start()
    {
    }

    void Update()
    {
        if (Player.instance.activeMode == InputMode.NONE)
        {
            currentView = views[0];
        }
        if (Player.instance.activeMode == InputMode.CREATE || Player.instance.activeMode == InputMode.EDIT)
        {
            currentView = views[1];
        }
        if (Player.instance.activeMode == InputMode.WALK)
        {
            currentView = views[0];
        }
    }


    void LateUpdate()
    {

        //Lerp position
        transform.position = Vector3.Lerp(transform.position, currentView.position, Time.deltaTime * transitionSpeed);

        Vector3 currentAngle = new Vector3(
         Mathf.LerpAngle(transform.rotation.eulerAngles.x, currentView.transform.rotation.eulerAngles.x, Time.deltaTime * transitionSpeed),
         Mathf.LerpAngle(transform.rotation.eulerAngles.y, currentView.transform.rotation.eulerAngles.y, Time.deltaTime * transitionSpeed),
         Mathf.LerpAngle(transform.rotation.eulerAngles.z, currentView.transform.rotation.eulerAngles.z, Time.deltaTime * transitionSpeed));

        transform.eulerAngles = currentAngle;

    }
}