using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFace : PlanetFace
{
    public WaterFace(WaterGenerator waterGenerator, Mesh mesh, int resolution, Vector3 localUp)
        : base(waterGenerator, mesh, resolution, localUp){}
}