using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFace : PlanetFace
{
    public WaterFace(WaterGenerator waterGenerator, Mesh mesh, int resolution, Vector3 localUp)
        : base(waterGenerator, mesh, resolution, localUp){}

    public void UpdateUVs()
    {
        Vector3[] vertices = mesh.vertices;
        Vector2[] uvs = new Vector2[vertices.Length];

        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }
        mesh.uv = uvs;
    }

    public override void ConstructMesh()
    {
        base.ConstructMesh();
        UpdateUVs();
    }

}