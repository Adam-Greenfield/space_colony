using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFaceGenerator
{
    void UpdateSettings(IFaceSettings settings);
    Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere);
}
