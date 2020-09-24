using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbiters : MonoBehaviour
{
    public int number_of_planets;
    public int max_radius;
    public GameObject[] planets;
    public Material[] materials;
    public Material trailMaterial;

    void Awake()
    {
        planets = new GameObject[number_of_planets];
    }

    // Start is called before the first frame update
    void Start()
    {
        planets = CreatePlanets(number_of_planets, max_radius);
    }

    public GameObject[] CreatePlanets(int count, int radius)
    {
        var planets = new GameObject[count];
        var planet_to_copy = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Rigidbody rb = planet_to_copy.AddComponent<Rigidbody>();
        rb.useGravity = false;

        for (int i=0; i < count; i++)
        {
            var pl = GameObject.Instantiate(planet_to_copy);
            pl.transform.position = this.transform.position + new Vector3(UnityEngine.Random.Range(-max_radius, max_radius), UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-max_radius, max_radius));
            pl.transform.localScale *= UnityEngine.Random.Range(0.5f, 1);
            pl.GetComponent<Renderer>().material = materials[UnityEngine.Random.Range(0, materials.Length)];
            /*
            TrailRenderer tr = pl.AddComponent<TrailRenderer>();
            tr.time = 1.0f;
            tr.startWidth = 0.1f;
            tr.endWidth = 0;
            tr.material = trailMaterial;
            tr.startColor = new Color(1, 1, 0, 0.1f);
            tr.endColor = new Color(0, 0, 0, 0);
            */
            planets[i] = pl;
        }

        GameObject.Destroy(planet_to_copy);

        return planets;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject p in planets)
        {
            Vector3 difference = this.transform.position - p.transform.position;
            float dist = difference.magnitude;
            Vector3 gravityDirection = difference.normalized;
            float gravity = 6.7f * (this.transform.localScale.x * p.transform.localScale.x * 80) / (dist * dist);
            Vector3 gravityVector = (gravityDirection * gravity);
            p.transform.GetComponent<Rigidbody>().AddForce(p.transform.forward, ForceMode.Acceleration);
            p.transform.GetComponent<Rigidbody>().AddForce(gravityVector, ForceMode.Acceleration);
        }
    }
}
