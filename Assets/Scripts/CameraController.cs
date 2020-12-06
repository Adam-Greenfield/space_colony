using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float distance = 5.0f;
    float speed = 0.2f;
    float rotateSpeed = 8.0f;

    bool noAutoRotate;
    float inputTimer = 5f;

    List<Transform> targetTransforms = new List<Transform>();

    void Start()
    {
        
        var targets = FindObjectsOfType<MonoBehaviour>().OfType<ICameraTarget>();
        foreach (ICameraTarget target in targets)
        {
            targetTransforms.Add(target.transform);
        }

        Debug.Log(targetTransforms[0]);

        SetTotarget(targetTransforms[0]);
    }

    void Update()
    {
        inputTimer += Time.deltaTime;

        //modify the distance by the planets radius
        float sizeFactor = transform.parent.transform.localScale.x / 10;

        float rotationSpeed = Mathf.Clamp((speed - sizeFactor / 25) * 50, 0.1f, float.MaxValue);


        if (Input.GetKeyDown("d"))
        {
            Transform currentParent = transform.parent.GetComponent<Transform>();

            int index = targetTransforms.IndexOf(currentParent);

            if (index >= targetTransforms.Count - 1)
                index = -1;

            SetTotarget(targetTransforms[index + 1]);
        }

        if (Input.GetMouseButton(2))
        {
            inputTimer = 0;

            transform.RotateAround(transform.parent.transform.position, transform.up, -Input.GetAxis("Mouse X") * rotateSpeed);
            transform.RotateAround(transform.parent.transform.position, transform.right, -Input.GetAxis("Mouse Y") * rotateSpeed);
        }

        if (inputTimer <= 5f)
            return;

        transform.RotateAround(transform.parent.transform.position, Vector3.up, rotationSpeed * Time.deltaTime);

        transform.LookAt(transform.parent);
    }

    private void SetTotarget(Transform target)
    {
        float distanceFromPlanet = (distance - (target.transform.localScale.x / 10));

        transform.SetParent(target);
        transform.localPosition = new Vector3(distanceFromPlanet, 0.0f, distanceFromPlanet);
        transform.LookAt(transform.parent);
    }
}
