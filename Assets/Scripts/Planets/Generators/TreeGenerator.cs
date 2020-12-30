using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGenerator
{
    TreeSettings settings;

    public float treeLineMin;
    public float treeLineMax;

    public void UpdateSettings(TreeSettings settings)
    {
        this.settings = settings;
    }

    //trees appear randomly about the planet for decoration
    public void InstantiateTrees(Vector3 vertex, float elevation, Transform treeHolderTransform, ShapeGenerator shapeGenerator, float waterElevation, Vector3 worldOrigin)
    {
        //TODO check elevation > waterElevation
        if( elevation < treeLineMax && elevation > waterElevation)
        {
            GameObject tree = GameObject.Instantiate(settings.treeModel, vertex, Quaternion.identity) as GameObject;
            tree.transform.LookAt(worldOrigin);
            tree.transform.rotation *= Quaternion.Euler(270, 0, 0);
            tree.transform.parent = treeHolderTransform;
        }
        //randomly spread about individual tree assets
        //collect positions and rotations to instantiate trees in
    }

    //a forest is a blob of trees that can be farmed for wood
    public void InstantiateForests()
    {
        //create a series of forests out of the individual trees
    }

    public int GetInverseNumberOfTrees(int verticies)
    {
        return verticies / (settings.intensity * 20);
    }

    public void UpdateTreeLine(float? minLandElevation, float? maxLandELevation)
    {
        if(minLandElevation != null)
            treeLineMin = (float)minLandElevation;

        if(maxLandELevation != null)
            treeLineMax = (float)maxLandELevation * settings.treeLineElevationPercent;
    }
}
