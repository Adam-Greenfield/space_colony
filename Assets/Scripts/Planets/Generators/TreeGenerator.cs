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
    public void InstantiateTrees(Vector3 vertex, float elevation, GameObject treeHolder, ShapeGenerator shapeGenerator, float waterElevation)
    {
        //TODO check elevation > waterElevation
        if( elevation < treeLineMax && elevation > waterElevation)
        {
            GameObject tree = GameObject.Instantiate(settings.treeModel, vertex, Quaternion.identity);
            tree.transform.parent = treeHolder.transform;
        }
        //randomly spread about individual tree assets
        //collect positions and rotations to instantiate trees in
    }

    //a forest is a blob of trees that can be farmed for wood
    public void InstantiateForests()
    {
        //create a series of forests out of the individual trees
    }

    public int GetNumberOfTrees(int verticies)
    {
        return verticies / ((11 - settings.intensity) * 50);
    }

    public void UpdateTreeLine(float? minLandElevation, float? maxLandELevation)
    {
        if(minLandElevation != null)
            treeLineMin = (float)minLandElevation;

        if(maxLandELevation != null)
            treeLineMax = (float)maxLandELevation * settings.treeLineElevationPercent;
    }
}
