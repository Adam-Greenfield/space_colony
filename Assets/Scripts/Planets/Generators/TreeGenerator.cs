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
    public void GenerateTrees()
    {
        //randomly spread about individual tree assets
    }

    //a forest is a blob of trees that can be farmed for wood
    public void GenerateForests()
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
