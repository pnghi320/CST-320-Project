using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;

public class StepsRunner
{
    List<Step> steps;

    public StepsRunner(List<Step> steps)
    {
        this.steps = steps;
    }

    public IEnumerator RunSteps(GameObject gameObject)
    {
        foreach (Step step in steps)
        {
            Debug.Log("Calling Step from Builder");
            yield return step.Start(gameObject);
            Debug.Log("End of Step from Builder");
        }
    }
}