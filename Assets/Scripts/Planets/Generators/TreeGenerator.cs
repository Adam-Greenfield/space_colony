using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class TreeGenerator
{
    public float treeLineMin;
    public float treeLineMax;

    public PlanetManager planetManager;

    TreeSettings settings;

    int treeVariety;

    System.Random random = new System.Random();

    public readonly int[] clusterSequence = new int[] { 1,2,3,5,8,13,21,34,55 };

    public void UpdateSettings(TreeSettings settings)
    {
        this.settings = settings;
        this.treeVariety = settings.treeModels.Length;
    }

    public void InstantiateTree(Vector3 vertex, float elevation, Transform treeHolderTransform, float waterElevation, Vector3 worldOrigin)
    {
        // if (settings.treeModels.Length == 0)
        //     throw new Exception("No tree models have been set");

        int varietyIndex = random.Next(treeVariety);

        if( elevation < treeLineMax && elevation > waterElevation)
        {
            GameObject treeGO = GameObject.Instantiate(settings.treeModels[varietyIndex], vertex + worldOrigin, Quaternion.identity) as GameObject;
            treeGO.transform.LookAt(worldOrigin);
            treeGO.transform.rotation *= Quaternion.Euler(270, 0, 0);
            treeGO.transform.parent = treeHolderTransform;

            _Tree tree = treeGO.GetComponent<_Tree>();
            tree.defaultScale = treeGO.transform.localScale;
            tree.Age = random.Next(1,5);
            tree.SetStartingScale();

            planetManager.treeCount += 1;
        }
    }

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

    public int VertextIncrementor(int resolution)
    {
        int c = 4;
        int r = resolution * 4;

        int magnitudeX = random.Next(2,10);
        int magnitudeY = random.Next(2,10);

        int resultX = c * magnitudeX;
        int resultY = r * magnitudeY;

        return resultX + resultY;

    }
}

