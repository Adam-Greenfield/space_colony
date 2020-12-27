using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class WaterSettings : ScriptableObject, IFaceSetting
{
    public float planetRadius { get; set; } = 1;
    public float elevation;
    public Color color;
    public Material waterMaterial;
}
