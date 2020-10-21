using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StepBuilder
{
    List<Step> steps;
    public StepBuilder()
    {
        steps = new List<Step>();
    }

    public StepBuilder Add(Step step)
    {
        steps.Add(step);
        return this;
    }

    public StepBuilder AddList(List<Step> steps)
    {
        this.steps = this.steps.Concat(steps).ToList();
        return this;
    }

    public StepsRunner Build()
    {
        return new StepsRunner(steps);
    }
}
