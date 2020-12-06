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
        transform.localPosition = new Vector3(
            distance * Mathf.Sin(Time.time * speed), // x
            0.0f, // y
            distance * Mathf.Cos(Time.time * speed)); // z

        transform.LookAt(transform.parent);

        if (Input.GetKeyDown("d"))
        {

            Transform currentParent = transform.parent.GetComponent<Transform>();

            int index = targetTransforms.IndexOf(currentParent);


            if (index >= targetTransforms.Count - 1)
                index = -1;

            transform.SetParent(targetTransforms[index + 1]);

            Debug.Log(targetTransforms[index + 1]);
        }
    }
}
