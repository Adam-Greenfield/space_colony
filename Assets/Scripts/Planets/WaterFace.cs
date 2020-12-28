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
        Vector3[] verticies = new Vector3[resolution * resolution];
        //work out how many triangles will make up the face considering resolution
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];
        int triIndex = 0;

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                //same as incrememnting i in second loop
                int i = x + y * resolution;
                //how far through the face are we
                Vector2 percent = new Vector2(x, y) / (resolution - 1);
                //get point on face by working out values along axisA and axisB
                Vector3 pointOnUnitCube = localUp + (percent.x - .5f) * 2 * axisA + (percent.y - .5f) * 2 * axisB;
                //add to verticies
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                //pass in delegate to do this
                verticies[i] = generator.CalculatePointOnPlanet(pointOnUnitSphere);

                //don't do this for bottom and right face, as the triangles would extend out the mesh
                if (x != resolution -1 && y != resolution -1)
                {
                    //top left
                    triangles[triIndex] = i;
                    //bottom right
                    triangles[triIndex + 1] = i + resolution + 1;
                    //bottom left
                    triangles[triIndex + 2] = i + resolution;

                    //top left
                    triangles[triIndex + 3] = i;
                    //top right
                    triangles[triIndex + 4] = i + 1;
                    //bottom right
                    triangles[triIndex + 5] = i + resolution + 1;

                    triIndex += 6;
                }
            }
        }

        mesh.Clear();
        mesh.vertices = verticies;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

}