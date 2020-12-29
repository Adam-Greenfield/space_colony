using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGenerator
{

    WaterSettings settings;

    public void UpdateSettings(WaterSettings settings)
    {
        this.settings = settings;
    }

    public float CalculateElevation(Vector3 pointOnUnitSphere)
    {
        //might use this later for high tables of water
        throw new System.NotImplementedException();
    }

    public float CalculateWaterElevation()
    {
        float elevation = settings.elevation;

        elevation = settings.planetRadius * ( 1 + elevation / 10f);
        return elevation;
    }

    public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere, float elevation)
    {
        return pointOnUnitSphere * elevation;
    }
}
