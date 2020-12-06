using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float distance = 40.0f;
    public float speed = 0.2f;

    List<Transform> targetTransforms = new List<Transform>();

    void Start()
    {
        
        var targets = FindObjectsOfType<MonoBehaviour>().OfType<ICameraTarget>();
        foreach (ICameraTarget target in targets)
        {
            targetTransforms.Add(target.transform);
            Debug.Log(target.transform);
        }
        Debug.Log(targetTransforms.Count);
    }

    void Update()
    {
        //modify the distance by the planets radius
        float sizeFactor = transform.parent.transform.localScale.x / 10;

        Debug.Log(sizeFactor);

        float distanceFromPlanet = distance - sizeFactor;
        float rotationSpeed = Mathf.Clamp(speed - sizeFactor / 15, 0.1f, float.MaxValue);

        Debug.Log(rotationSpeed);

        transform.localPosition = new Vector3(
            distanceFromPlanet * Mathf.Sin(Time.time * rotationSpeed), // x
            0.0f, // y
            distanceFromPlanet * Mathf.Cos(Time.time * rotationSpeed)); // z

        transform.LookAt(transform.parent);

        if (Input.GetKeyDown("d"))
        {

            Transform currentParent = transform.parent.GetComponent<Transform>();

            int index = targetTransforms.IndexOf(currentParent);


            if (index >= targetTransforms.Count - 1)
                index = -1;

            transform.SetParent(targetTransforms[index + 1]);
        }
    }
}
