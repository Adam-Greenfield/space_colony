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
        //get the tree line
    }

    //trees appear randomly about the planet for decoration
    public void InstantiateTrees(Vector3 vertex, float elevation, GameObject treeHolder, ShapeGenerator shapeGenerator, int i)
    {
        if( elevation < treeLineMax && i % (11000 - settings.intensity * 1000) == 0)
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

    public void UpdateTreeLine(float? minLandElevation, float? maxLandELevation)
    {
        if(minLandElevation != null)
            treeLineMin = (float)minLandElevation;

        if(maxLandELevation != null)
            treeLineMax = (float)maxLandELevation * settings.treeLineElevationPercent;
    }
}
