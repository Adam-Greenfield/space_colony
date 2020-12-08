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


    bool transitioningMove = false;
    bool transitioningLook = false;


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
    }

    void LateUpdate()
    {

        float sizeFactor = transform.parent.transform.localScale.x / 10;

        float rotationSpeed = Mathf.Clamp((speed - sizeFactor / 25) * 50, 0.1f, float.MaxValue);

        if (!transitioningLook)
            transform.LookAt(transform.parent.transform.position);

        if (inputTimer <= 5f)
            return;

        if (!transitioningMove)
            transform.RotateAround(transform.parent.transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
        
    }


    private void SetTotarget(Transform target)
    {
        float distanceFromPlanet = (distance - (target.transform.localScale.x / 10));

        transform.SetParent(target);

        transitioningMove = true;
        transitioningLook = true;

        LookAt(distanceFromPlanet);
        MoveTo(distanceFromPlanet);
        //TODO do a final lookAt to compensate for target movement during transition
    }

    void LookAt(float distanceFromPlanet)
    {
        StartCoroutine(LerpLookAt(transform.parent, distanceFromPlanet));
    }

    void MoveTo(float distanceFromPlanet)
    {
        StartCoroutine(LerpMoveTo(transform.parent, distanceFromPlanet));
    }


    IEnumerator LerpLookAt(Transform target, float distanceFromPlanet)
    {
        float timeElapsed = 0f;
        float lerpDuration = 0.5f;

        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);

        
        while (timeElapsed < lerpDuration)
        {
            float t = timeElapsed / lerpDuration;
            t = t * t * (3f - 2f * t);

            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        transform.rotation = targetRotation;
        transitioningLook = false;
    }

    IEnumerator LerpMoveTo(Transform target, float distanceFromPlanet)
    {
        float timeElapsed = 0f;
        float lerpDuration = 2f;

        //divide distance by distance from planet to get a fraction we can use to calculate final vector3 world position to lerp to
        float difference = distanceFromPlanet / Vector3.Distance(target.position, transform.position);

        float sizeFactor = transform.parent.transform.localScale.x / 10;
        float rotationSpeed = Mathf.Clamp((speed - sizeFactor / 25) * 50, 0.1f, float.MaxValue);

        Vector3 targetPosition = Vector3.Lerp(target.position, transform.position, difference);

        //get the target position as a local position of the planet
        Vector3 localStartPosition = transform.localPosition;
        Vector3 localTargetPosition = target.InverseTransformPoint(targetPosition);

        while (timeElapsed < lerpDuration)
        {
            float t = timeElapsed / lerpDuration;
            t = t * t * (3f - 2f * t);

            Vector3 zoomTarget = Vector3.Lerp(localStartPosition, localTargetPosition, t);

    
            if (timeElapsed / lerpDuration >= 0.7f)
            {
                //increase the local position by the new rotation
                //get the local position, then the new position by the rotation, then the new position by the move and calc the difference
                Vector3 rotationalTarget = GetRotationalLocalPosition(transform.parent.transform.position, Vector3.up, (rotationSpeed * 2.5f) * Time.deltaTime);

                Vector3 rotationDiff = rotationalTarget - transform.localPosition;
                
                transform.localPosition = zoomTarget + rotationDiff;
                localStartPosition += rotationDiff;
                localTargetPosition += rotationDiff;
            } else
            {
                transform.localPosition = zoomTarget;
            }


            timeElapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = localTargetPosition;
        transitioningMove = false;
    }

    private Vector3 GetRotationalLocalPosition(Vector3 point, Vector3 axis, float angle)
    {
        Vector3 vector = transform.position;
        Quaternion rotation = Quaternion.AngleAxis(angle, axis);
        Vector3 vector2 = vector - point;
        vector2 = rotation * vector2;
        vector = point + vector2;
        return transform.parent.transform.InverseTransformPoint(vector);
    }

}
