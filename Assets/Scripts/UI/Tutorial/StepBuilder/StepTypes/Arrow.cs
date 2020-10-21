using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Step
{
    Vector2 from;
    Vector2 to;
    GameObject gameObject;

    public Arrow(Vector2 from, Vector2 to, int duration, float delay = 0, bool runNextAfterCompletion = false) : base(duration, delay, runNextAfterCompletion)
    {
        this.from = from;
        this.to = to;
    }

    public Arrow(Vector2 from, Vector2 to) : base()
    {
        this.from = from;
        this.to = to;
    }

    protected override void Close()
    {
        Debug.Log("Closing Arrow");
        GameObject.Destroy(gameObject);
    }

    protected override void Show()
    {
        gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        gameObject.transform.localScale = Vector3.one * 3;
        gameObject.transform.position = Random.insideUnitSphere * 4;
        Debug.Log("Showing Arrow");
    }
}
