using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour, ICameraTarget
{
    private int _distanceFromCore;

    public int distanceFromCore { get { return _distanceFromCore; } set { _distanceFromCore = value; } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
