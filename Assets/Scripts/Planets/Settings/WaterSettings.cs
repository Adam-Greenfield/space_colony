using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class WaterSettings : ScriptableObject
{
    public float elevation;
    public Color color;
    public Material waterMaterial;
}
