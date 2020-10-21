using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepTest : MonoBehaviour
{
    public Step waitedStep;
    // Start is called before the first frame update
    void Start()
    {
        waitedStep = new Arrow(Vector2.zero, Vector2.one * 10, -1, -1, true);

        StepBuilder stepBuilder = new StepBuilder()
            .Add(new Arrow(Vector2.zero, Vector2.one * 10, 5, 3, true))
            .Add(new Arrow(Vector2.zero, Vector2.one * 10, 1, 3, false))
            .Add(new Arrow(Vector2.zero, Vector2.one * 10, 1, 0, true))
            .Add(waitedStep);
        StartCoroutine(stepBuilder.Build().RunSteps(gameObject));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (waitedStep.Run())
            {
                Debug.Log("Ran nicely");
            } else
            {
                Debug.Log("Can't run");
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (waitedStep.Stop())
            {
                Debug.Log("Ran nicely");
            }
            else
            {
                Debug.Log("Can't run");
            }
        }
    }
}
