using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Step
{
    Vector2 from;
    Vector2 to;
    UnityEngine.Object arrowPrefab;
    GameObject arrow;


    public Arrow(Vector2 from, Vector2 to, int duration, float delay = 0, bool runNextAfterCompletion = false) : base(duration, delay, runNextAfterCompletion)
    {
        this.from = from;
        this.to = to;
    }

    public Arrow(Vector2 from, Vector2 to) : base()
    {
        this.from = from;
        this.to = to;
        arrowPrefab = Resources.Load("Assets/Prefabs/UI/Arrow");
        //GameObject pNewObject = (GameObject)GameObject.Instantiate(pPrefab, Vector3.zero, Quaternion.identity);
    }

    public Vector2 getTo()
    {
        return to;
    }

    public void setTo(Vector2 to)
    {
        this.to = to;
    }

    public Vector2 getFrom()
    {
        return from;
    }

    public void setFrom(Vector2 from)
    {
        this.from = from;
    }

    protected override void Close()
    {
        Debug.Log("Closing Arrow");
        GameObject.Destroy(arrow);
    }

    protected override void Show()
    {
        arrow = (GameObject)GameObject.Instantiate(arrowPrefab, Vector3.zero, Quaternion.identity);
        arrow.transform.localScale = Vector3.one * 3;
        arrow.transform.position = Random.insideUnitSphere * 4;
        Debug.Log("Showing Arrow");
    }
}
