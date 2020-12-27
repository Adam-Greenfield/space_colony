using System.Collections;
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

    [HideInInspector]
    public bool shapeSettingsFoldout;

    [HideInInspector]
    public bool colorSettingsFoldout;

    private int _distanceFromCore;

    ColorGenerator colorGenerator = new ColorGenerator();
    ShapeGenerator shapeGenerator = new ShapeGenerator();

    [SerializeField, HideInInspector]

    TerrainFace[] terrainFaces;

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

        if (planetMeshFilters.terrain == null || planetMeshFilters.terrain.Length == 0)
        {
            planetMeshFilters.terrain = new MeshFilter[6];
        }

        if (planetMeshFilters.water == null || planetMeshFilters.water.Length == 0)
        {
            planetMeshFilters.water = new MeshFilter[6];
        }
        
        terrainFaces = new TerrainFace[6];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6; i++)
        {
            GenerateMeshBase(planetMeshFilters.terrain, i, directions);
        }
    }

    private void GenerateMeshBase(MeshFilter[] filters, int i, Vector3[] directions)
    {
            if(filters[i] == null)
            {
                GameObject terrainMeshObj = new GameObject("mesh");
                terrainMeshObj.transform.parent = transform;

                terrainMeshObj.AddComponent<MeshRenderer>();
                filters[i] = terrainMeshObj.AddComponent<MeshFilter>();
                filters[i].sharedMesh = new Mesh();
            }

            filters[i].GetComponent<MeshRenderer>().sharedMaterial = colorSettings.planetMaterial;

            terrainFaces[i] = new TerrainFace(shapeGenerator, filters[i].sharedMesh, resolution, directions[i]);
            bool renderFace = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask - 1 == i;
            filters[i].gameObject.SetActive(renderFace);
    }

    void GenerateMesh()
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

    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColor();
    }

    public void OnShapeSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateMesh();
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
