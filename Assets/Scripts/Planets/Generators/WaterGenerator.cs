using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGenerator : IFaceGenerator
{

    WaterSettings settings;

    public void UpdateSettings(IFaceSetting Isettings)
    {
        this.settings = (WaterSettings)Isettings;
    }

    public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere)
    {
        return new Vector3();
    }
}
