using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    //planet manager keeps track of the planet stats, works out the max trees from the amount first generated etc
    //only runs in play mode
    [HideInInspector]
    public int treeCount;

    [SerializeField, HideInInspector]
    int maxTreeCount;
    TerrainFace[] terrainFaces;
    Planet planet;
    int faceTracker = 0;

    public void Start()
    {
        InvokeRepeating("SpawnTree", 2.0f, 5.0f);
        InvokeRepeating("SpawnAnimal", 2.0f, 5.0f);
    }

    public void CalculateMaxTrees()
    {
        maxTreeCount = (int)(treeCount * 1.4f);
    }

    public void SpawnTree()
    {
        if (treeCount >= maxTreeCount)
            return;

        planet = gameObject.GetComponent<Planet>();
        terrainFaces = planet.terrainFaces;
        TerrainFace face = terrainFaces[faceTracker];
        int vertexIndex = Random.Range(0, face.verticies.Length);
        Vector3 vertex = face.verticies[vertexIndex];
        float elevation = face.elevationVerticies[vertexIndex];
        //find a point on the planet to spawn a tree
        planet.treeGenerator.InstantiateTree(vertex, elevation, face.treeHolder, face.waterGenerator.CalculateWaterElevation(), face.worldOrigin);

        if (faceTracker <= 4)
            faceTracker ++;
        else
            faceTracker = 0;
    }

    public void SpawnAnimal()
    {
        planet = gameObject.GetComponent<Planet>();
        terrainFaces = planet.terrainFaces;
        TerrainFace face = terrainFaces[faceTracker];
        int vertexIndex = Random.Range(0, face.verticies.Length);
        Vector3 vertex = face.verticies[vertexIndex];
        float elevation = face.elevationVerticies[vertexIndex];

        planet.animalGenerator.InstantiateAnimal(vertex, elevation, planet, face.waterGenerator.CalculateWaterElevation());
    }
}
