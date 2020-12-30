using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class TreeSettings : ScriptableObject
{
    [Range(0,10)]
    public int intensity;

    [Range(1,9)]
    public int maxClusterSize;
    //TODO tree type enum

    [Range(0,1)]
    public float treeLineElevationPercent;

    public GameObject treeModel;

}
