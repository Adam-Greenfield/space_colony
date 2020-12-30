using UnityEngine;
using System.Collections;

public class TerrainFace
{
    WaterGenerator waterGenerator;
    ShapeGenerator shapeGenerator;
    TreeGenerator treeGenerator;
    public Mesh mesh { get; private set; }
    int resolution;
    Vector3 localUp;
    Vector3 axisA;
    Vector3 axisB;
    Transform parentTransform;
    Transform treeHolder;
    System.Random random;


    public TerrainFace(WaterGenerator waterGenerator, ShapeGenerator shapeGenerator, TreeGenerator treeGenerator,
        Mesh mesh, int resolution, Vector3 localUp, Transform parentTransform, Transform treeHolder)
    {
        this.waterGenerator = waterGenerator;
        this.shapeGenerator = shapeGenerator;
        this.treeGenerator = treeGenerator;
        this.mesh = mesh;
        this.resolution = resolution;
        this.localUp = localUp;
        this.parentTransform = parentTransform;
        this.treeHolder = treeHolder;
        this.random = new System.Random();

        //get a perpendicular axis from local
        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        //get cross product of local and a, results in normal to local and a
        axisB = Vector3.Cross(localUp, axisA);
    }

    //TODO create a mesh of useable land, where forests and structures can go
    //as we generate terrain, create this useable mesh

    public void ConstructMesh()
    {
        float waterElevation = waterGenerator.CalculateWaterElevation();
        Vector3[] verticies = new Vector3[resolution * resolution];
        int numOfTrees = treeGenerator.GetInverseNumberOfTrees(resolution * resolution);
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
                float elevation = shapeGenerator.CalculateElevation(pointOnUnitSphere);
                Vector3 vertexPoint = shapeGenerator.CalculatePointOnPlanet(pointOnUnitSphere, elevation);
                verticies[i] = vertexPoint;

                int randomIndex = this.random.Next(i - (resolution * 200), i + (resolution * 200));

                //add trees here
                if (randomIndex % numOfTrees == 0)
                    treeGenerator.InstantiateTrees(vertexPoint, elevation, treeHolder, shapeGenerator, waterElevation);
                                    

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
