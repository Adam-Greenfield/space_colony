using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class TreeGenerator
{

    public float treeLineMin;
    public float treeLineMax;

    TreeSettings settings;

    System.Random random = new System.Random();

    public readonly int[] clusterSequence = new int[] { 1,2,3,5,8,13,21,34,55 };

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
            GameObject tree = GameObject.Instantiate(settings.treeModel, vertex + worldOrigin, Quaternion.identity) as GameObject;
            tree.transform.LookAt(worldOrigin);
            tree.transform.rotation *= Quaternion.Euler(270, 0, 0);
            tree.transform.parent = treeHolderTransform;
        }
        //randomly spread about individual tree assets
        //collect positions and rotations to instantiate trees in
    }

    //a forest is a blob of trees that can be farmed for wood

    public int GetInverseNumberOfTrees(int verticies)
    {
        if (settings.intensity == 0)
            return 0;

        return verticies / (settings.intensity * 20);
    }

    public void UpdateTreeLine(float? minLandElevation, float? maxLandELevation)
    {
        if(minLandElevation != null)
            treeLineMin = (float)minLandElevation;

        if(maxLandELevation != null)
            treeLineMax = (float)maxLandELevation * settings.treeLineElevationPercent;
    }

    public int GetClusterSequenceLength()
    {
        return settings.maxClusterSize * (settings.maxClusterSize + 1 ) / 2;
    }

    public int[] MapClusterBySize()
    {
        int clusterSizeLength = GetClusterSequenceLength();
        int[] array = new int[clusterSizeLength];
        int size = settings.maxClusterSize;

        int clusterTracker = 0;

        int groupProgress = 0;

        for (int i = 0; i < clusterSizeLength; i++)
        {
            int clusterIndex = size - (size - clusterTracker);
            array[i] = clusterSequence[clusterIndex];

            groupProgress ++;

            if ( groupProgress == size - (clusterTracker))
            {
                groupProgress = 0;
                clusterTracker ++;
            }

        }

        return array;
    }

    public int VertextIncrementor(int resolution, int size)
    {
        //bigger forests have less spacing
        
        //int c = (clusterSequence[clusterSequence.Length - 1] + 5 - size) / 6;
        int c = 2;
        int r = resolution * 2;

        int magnitudeX = random.Next(2,10);
        int magnitudeY = random.Next(2,10);

        int resultX = c * magnitudeX;
        int resultY = r * magnitudeY;

        return resultX + resultY;

    }
}

