using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class TreeSettings : ScriptableObject
{
    [Range(0,10)]
    public int intensity;
    //TODO tree type enum

    [Range(0,1)]
    public float treeLineElevationPercent;

}
