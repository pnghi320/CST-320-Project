using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Step
{
    float duration;
    float delay;
    bool runNextAfterCompletion;

    GameObject gameObject;
    bool started = false;
    bool waiting = true;
    bool showing = false;
    bool beenShowed = false;

    public Step(float duration = -1, float delay = 0, bool runNextAfterCompletion = false)
    {
        this.duration = duration;
        this.delay = delay;
        this.runNextAfterCompletion = runNextAfterCompletion;
        Debug.Log("Step Constructed");
    }

    abstract protected void Show();

    public IEnumerator Start(GameObject gameObject)
    {
        started = true;
        Debug.Log("Step Start Function");
        Debug.Log("Delay: " + delay + " | Duration: " + duration);
        if (delay != -1)
        {
            DelayStart();
        }
        if (runNextAfterCompletion)
        {
            yield return new WaitUntil(() => waiting == false && showing == false);
        }
        else
        {
            yield return new WaitUntil(() => waiting == false && showing == true);
        }

    }

    protected void SetGameObject(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }

    protected GameObject GetGameObject()
    {
        return gameObject;
    }

    async void DelayStart()
    {
        Debug.Log("Running Delay Start");
        await Task.Delay(TimeSpan.FromSeconds(delay));
        Debug.Log("Finished Delay Start");

        Run();
    }

    async void DurationStop()
    {
        await Task.Delay(TimeSpan.FromSeconds(duration));
        Close();
        Debug.Log("Step ran duration close");
        showing = false;
    }

    public bool Stop()
    {
        if (!showing) return false;
        Close();
        showing = false;

        return true;
    }

    public bool Run()
    {
        if (!started || beenShowed) return false;
        waiting = false;
        showing = true;
        beenShowed = true;
        Show();
        Debug.Log("Step ran Show");

        if (duration == -1) return true;
        DurationStop();
        return true;
    }

    abstract protected void Close();
}
