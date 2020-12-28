﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator : IFaceGenerator
{
    ShapeSettings settings;
    INoiseFilter[] noiseFilters;
    public MinMax elevationMinMax;

    public void UpdateSettings(IFaceSettings Isettings)
    {
        this.settings = (ShapeSettings)Isettings;
        noiseFilters = new INoiseFilter[settings.noiseLayers.Length];
        for (int i = 0; i < noiseFilters.Length; i++)
        {
            noiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(settings.noiseLayers[i].noiseSettings);
        }
        elevationMinMax = new MinMax();
    }

    public float CalculateElevation(Vector3 pointOnUnitSphere)
    {
        float firstLayerValue = 0;
        float elevation = 0;

        if (noiseFilters.Length > 0)
        {
            firstLayerValue = noiseFilters[0].Evaluate(pointOnUnitSphere);
            if (settings.noiseLayers[0].enabled)
                elevation = firstLayerValue;
        }

        for (int i = 1; i < noiseFilters.Length; i++)
        {
            if(settings.noiseLayers[i].enabled)
            {
                float mask = settings.noiseLayers[i].useFirstLayerAsMask ? firstLayerValue : 1;
                elevation += noiseFilters[i].Evaluate(pointOnUnitSphere) * mask;
            }
        } 
        elevation = settings.planetRadius * ( 1 + elevation);
        return elevation;
    }

    public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere, float elevation)
    {
        elevationMinMax.AddValue(elevation);
        return pointOnUnitSphere * elevation;
    }
}
