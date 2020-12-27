using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFaceGenerator
{
    void UpdateSettings(IFaceSetting settings);

    Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere);
}
