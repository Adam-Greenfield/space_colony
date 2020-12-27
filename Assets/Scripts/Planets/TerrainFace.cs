using UnityEngine;
using System.Collections;

public class TerrainFace : PlanetFace
{
    public TerrainFace(ShapeGenerator shapeGenerator, Mesh mesh, int resolution, Vector3 localUp)
        : base(shapeGenerator, mesh, resolution, localUp){}
}
