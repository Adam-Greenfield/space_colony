using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class OrbitSettings : ScriptableObject
{    public Range range;
}

public enum Range
{
    Near = 200,
    Nearish = 400,
    Middling = 600,
    Farish = 800,
    Far = 1000
}