using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class WaterSettings : ScriptableObject
{
    public float planetRadius = 1;
    public float elevation;
    public Material waterVisibleMaterial;
}
