using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Animal : MonoBehaviour
{
    public IPrimary primary;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate() {
        //apply spherical gravity to selected objects (set the objects in editor)
        Rigidbody rigidBody = GetComponent<Rigidbody>();
        Debug.Log(rigidBody);
        if(rigidBody != null)
            rigidBody.AddForce((primary.transform.position - transform.position).normalized * primary.gravitationalPull);
    
    }

    // Update is called once per frame
    void Update()
    {
        //attach the animal to the nearest primary
    }
}
