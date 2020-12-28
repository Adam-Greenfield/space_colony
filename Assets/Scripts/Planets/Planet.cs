﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour, ICameraTarget
{
    [Range(2,256)]
    public int resolution = 10;
    public bool autoUpdate = true;

    public enum FaceRenderMask { All, Top, Bottom, Left, Right, Front, Back }
    public FaceRenderMask faceRenderMask;

    public ShapeSettings shapeSettings;
    public ColorSettings colorSettings;
    public WaterSettings waterSettings;

    [HideInInspector]
    public bool shapeSettingsFoldout;

    [HideInInspector]
    public bool colorSettingsFoldout;

    [HideInInspector]
    public bool waterSettingsFoldout;

    private int _distanceFromCore;

    ColorGenerator colorGenerator = new ColorGenerator();
    ShapeGenerator shapeGenerator = new ShapeGenerator();
    WaterGenerator waterGenerator = new WaterGenerator();

    [SerializeField, HideInInspector]

    TerrainFace[] terrainFaces;
    WaterFace[] waterFaces;

    struct PlanetMeshFilters
    {
        public MeshFilter[] terrain;
        public MeshFilter[] water;
    }

    PlanetMeshFilters planetMeshFilters;

    private void OnValidate()
    {
        GeneratePlanet();
    }

    public int distanceFromCore { get { return _distanceFromCore; } set { _distanceFromCore = value; } }

    void Initialize()
    {
        colorGenerator.UpdateSettings(colorSettings);
        shapeGenerator.UpdateSettings(shapeSettings);
        waterGenerator.UpdateSettings(waterSettings);

        if (planetMeshFilters.terrain == null || planetMeshFilters.terrain.Length == 0)
        {
            planetMeshFilters.terrain = new MeshFilter[6];
        }

        if (planetMeshFilters.water == null || planetMeshFilters.water.Length == 0)
        {
            planetMeshFilters.water = new MeshFilter[6];
        }
        
        terrainFaces = new TerrainFace[6];
        waterFaces = new WaterFace[6];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6; i++)
        {
            GenerateMeshBase(planetMeshFilters.terrain, i, directions, colorSettings.planetMaterial, "terrain");

            terrainFaces[i] = new TerrainFace(shapeGenerator, planetMeshFilters.terrain[i].sharedMesh, resolution, directions[i]);
            bool renderTerrainFace = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask - 1 == i;
            planetMeshFilters.terrain[i].gameObject.SetActive(renderTerrainFace);

            GenerateMeshBase(planetMeshFilters.water, i, directions, waterSettings.waterMaterial, "water");

            waterFaces[i] = new WaterFace(waterGenerator, shapeGenerator, planetMeshFilters.water[i].sharedMesh, resolution, directions[i]);
            bool renderWaterFace = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask - 1 == i;
            planetMeshFilters.water[i].gameObject.SetActive(renderWaterFace);
        }
    }

    private void GenerateMeshBase(MeshFilter[] filters, int i, Vector3[] directions, Material material, string name)
    {
        if(filters[i] == null)
        {
            GameObject meshObj = new GameObject(name);
            meshObj.transform.parent = transform;

            meshObj.AddComponent<MeshRenderer>();
            filters[i] = meshObj.AddComponent<MeshFilter>();
            filters[i].sharedMesh = new Mesh();
        }

        filters[i].GetComponent<MeshRenderer>().sharedMaterial = material;
    }

    void GenerateTerrain()
    {
        for (int i = 0; i < 6; i++)
        {
            if(planetMeshFilters.terrain[i].gameObject.activeSelf)
                terrainFaces[i].ConstructMesh();
        }
        colorGenerator.UpdateElevation(shapeGenerator.elevationMinMax);
    }

    void GenerateColor()
    {
        colorGenerator.UpdateColors();
    }

    void GenerateWater()
    {
        for (int i = 0; i < 6; i++)
        {
            if(planetMeshFilters.water[i].gameObject.activeSelf)
            {
                waterFaces[i].ConstructMesh();
                waterFaces[i].UpdateUVs();
                //TODO also create water available mesh by comparing with terrain elevation
            }
        }
    }

    public void GeneratePlanet()
    {
        Initialize();
        GenerateTerrain();
        GenerateColor();
        GenerateWater();
    }

    public void OnShapeSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateTerrain();
        }
    }

    public void OnColorSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateColor();
        }
    }

    public void OnWaterSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateWater();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //TODO
    //Planet has resources: food, wood, water, ore. ore for technology wood for construction
    //Forest's exist in the plains, ore exists in the mountains

    //calculate a mesh for available water and a mesh for buildable land
    //for land, work out area with at least a few verticies without a harsh gradient
    //for water, create a mesh where the water elevation is higher than the ground elevation
}
