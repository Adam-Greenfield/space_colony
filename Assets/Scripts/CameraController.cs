using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float distance = 35.0f;
    float speed = 0.2f;
    float rotateSpeed = 8.0f;
    float inputTimer = 5f;
    bool noAutoRotate;
    public float nearClipPlane = 0.01f;


    bool transitioning = false;

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
            //TODO handle multiple presses and new targets
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

        if (!transitioning)
        {
            transform.RotateAround(transform.parent.transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
            transform.LookAt(transform.parent.transform.position);
        }
    }

    private void SetTotarget(Transform target)
    {
        float distanceFromPlanet = (distance - (target.transform.localScale.x / 10));

        transform.SetParent(target);

        transitioning = true;

        LookAt(target, distanceFromPlanet);
        MoveTo(target, distanceFromPlanet);
        //TODO do a final lookAt to compensate for target movement during transition
    }

    void LookAt(Transform target, float distanceFromPlanet)
    {
        StartCoroutine(LerpLookAt(transform.parent, distanceFromPlanet));
    }

    void MoveTo(Transform target, float distanceFromPlanet)
    {
        StartCoroutine(LerpMoveTo(transform.parent, distanceFromPlanet));
        transitioning = false;
    }


    IEnumerator LerpLookAt(Transform target, float distanceFromPlanet)
    {
        float timeElapsed = 0f;
        float lerpDuration = 0.5f;

        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);

        
        while (timeElapsed < lerpDuration)
        {
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        transform.rotation = targetRotation;
    }

    IEnumerator LerpMoveTo(Transform target, float distanceFromPlanet)
    {
        float timeElapsed = 0f;
        float lerpDuration = 2f;

        //divide distance by distance from planet to get a fraction we can use to calculate final vector3 world position to lerp to
        float difference = distanceFromPlanet / Vector3.Distance(target.position, transform.position);

        Vector3 targetPosition = Vector3.Lerp(target.position, transform.position, difference);

        //get the target position as a local position of the planet
        Vector3 localStartPosition = transform.localPosition;
        Vector3 localTargetPosition = target.InverseTransformPoint(targetPosition);

        //TODO change to use loacl positiion and constantly update target to compensate for movement

        while (timeElapsed < lerpDuration)
        {
            transform.localPosition = Vector3.Lerp(localStartPosition, localTargetPosition, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = localTargetPosition;
    }

}
