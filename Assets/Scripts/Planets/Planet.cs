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
    public WaterSettings waterSettings;
    public TreeSettings treeSettings;

    [HideInInspector]
    public bool shapeSettingsFoldout;

    [HideInInspector]
    public bool colorSettingsFoldout;

    [HideInInspector]
    public bool waterSettingsFoldout;

    [HideInInspector]
    public bool treeSettingsFoldout;

    [HideInInspector]
    public GameObject treeHolder;

    [HideInInspector]
    public GameObject landForms;

    private int _distanceFromCore;

    ColorGenerator colorGenerator = new ColorGenerator();
    ShapeGenerator shapeGenerator = new ShapeGenerator();
    WaterGenerator waterGenerator = new WaterGenerator();
    TreeGenerator treeGenerator = new TreeGenerator();

    [SerializeField, HideInInspector]

    TerrainFace[] terrainFaces;
    WaterVisibleFace[] waterVisibleFaces;
    WaterAccessableFace[] waterAccessableFaces;



    struct PlanetMeshFilters
    {
        public MeshFilter[] terrain;
        public MeshFilter[] waterVisible;
        public MeshFilter[] waterAccessable;
    }

    PlanetMeshFilters planetMeshFilters;

    Vector3 worldOrigin;

    private void OnValidate()
    {
        GeneratePlanet();
    }

    protected virtual void Awake() {}

    public int distanceFromCore { get { return _distanceFromCore; } set { _distanceFromCore = value; } }

    void Initialize()
    {
        colorGenerator.UpdateSettings(colorSettings);
        shapeGenerator.UpdateSettings(shapeSettings);
        waterGenerator.UpdateSettings(waterSettings);
        treeGenerator.UpdateSettings(treeSettings);

        if (planetMeshFilters.terrain == null || planetMeshFilters.terrain.Length == 0)
        {
            planetMeshFilters.terrain = new MeshFilter[6];
        }

        if (planetMeshFilters.waterVisible == null || planetMeshFilters.waterVisible.Length == 0)
        {
            planetMeshFilters.waterVisible = new MeshFilter[6];
        }

        if (planetMeshFilters.waterAccessable == null || planetMeshFilters.waterAccessable.Length == 0)
        {
            planetMeshFilters.waterAccessable = new MeshFilter[6];
        }

        if(transform.Find("LandForms"))
            DestroyImmediate(transform.Find("LandForms").gameObject);

        landForms = new GameObject("LandForms");
        landForms.transform.position = transform.position;
        landForms.transform.parent = transform;

        treeHolder = new GameObject("Trees");
        treeHolder.transform.position = transform.position;
        treeHolder.transform.parent = landForms.transform;
        
        terrainFaces = new TerrainFace[6];
        waterAccessableFaces = new WaterAccessableFace[6];
        waterVisibleFaces = new WaterVisibleFace[6];

        worldOrigin = transform.position;

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6; i++)
        {
            GenerateMeshRendererBase(planetMeshFilters.terrain, i, "terrain", colorSettings.planetMaterial);

            terrainFaces[i] = new TerrainFace(waterGenerator, shapeGenerator, treeGenerator, planetMeshFilters.terrain[i].sharedMesh, 
                resolution, directions[i], transform, treeHolder.transform, worldOrigin);
            bool renderTerrainFace = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask - 1 == i;
            planetMeshFilters.terrain[i].gameObject.SetActive(renderTerrainFace);

            GenerateMeshColliderBase(planetMeshFilters.waterAccessable, i, "water_accessable");

            waterAccessableFaces[i] = new WaterAccessableFace(waterGenerator, shapeGenerator, planetMeshFilters.waterAccessable[i].sharedMesh, resolution, directions[i]);
            bool renderWaterAccessableFace = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask - 1 == i;
            planetMeshFilters.waterAccessable[i].gameObject.SetActive(renderWaterAccessableFace);
            
            GenerateMeshRendererBase(planetMeshFilters.waterVisible, i, "water_visible", waterSettings.waterVisibleMaterial);

            waterVisibleFaces[i] = new WaterVisibleFace(waterGenerator, planetMeshFilters.waterVisible[i].sharedMesh, resolution, directions[i]);
            bool renderWaterVisibleFace = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask - 1 == i;
            planetMeshFilters.waterVisible[i].gameObject.SetActive(renderWaterVisibleFace);
        }
    }

    private void GenerateMeshRendererBase(MeshFilter[] filters, int i, string name, Material material)
    {
        if(filters[i] == null)
        {
            GameObject meshObj = new GameObject(name);
            meshObj.transform.parent = landForms.transform;
            meshObj.transform.position = transform.position;

            meshObj.AddComponent<MeshRenderer>();
            filters[i] = meshObj.AddComponent<MeshFilter>();
            filters[i].sharedMesh = new Mesh();
        }

        filters[i].GetComponent<MeshRenderer>().sharedMaterial = material;
    }

    private void GenerateMeshColliderBase(MeshFilter[] filters, int i, string name)
    {
        if(filters[i] == null)
        {
            GameObject meshObj = new GameObject(name);
            meshObj.transform.parent = landForms.transform;
            meshObj.transform.position = transform.position;

            meshObj.AddComponent<MeshCollider>();
            filters[i] = meshObj.AddComponent<MeshFilter>();

            filters[i].sharedMesh = new Mesh();
        }
    }

    void GenerateTerrain()
    {
        for (int i = 0; i < 6; i++)
        {
            if(planetMeshFilters.terrain[i].gameObject.activeSelf)
                terrainFaces[i].ConstructMesh();
        }
        colorGenerator.UpdateElevation(shapeGenerator.elevationMinMax);
        treeGenerator.UpdateTreeLine(null, shapeGenerator.elevationMinMax.Max);
    }

    void GenerateColor()
    {
        colorGenerator.UpdateColors();
    }

    void GenerateWater()
    {
        for (int i = 0; i < 6; i++)
        {
            if(planetMeshFilters.waterVisible[i].gameObject.activeSelf)
            {
                waterVisibleFaces[i].ConstructMesh();
                waterVisibleFaces[i].UpdateUVs();
            }
            
            if(planetMeshFilters.waterAccessable[i].gameObject.activeSelf)
            {
                waterAccessableFaces[i].ConstructMesh();
                waterAccessableFaces[i].UpdateUVs();

                planetMeshFilters.waterAccessable[i].gameObject.GetComponent<MeshCollider>().sharedMesh = null;
                planetMeshFilters.waterAccessable[i].gameObject.GetComponent<MeshCollider>().sharedMesh =
                    planetMeshFilters.waterAccessable[i].gameObject.GetComponent<MeshFilter>().sharedMesh;

            }
        }
        float waterElevation = waterGenerator.CalculateWaterElevation();
        treeGenerator.UpdateTreeLine(waterElevation, null);
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
            GenerateTerrain();
            GenerateWater();
        }
    }

    public void OnTreeSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateTerrain();
        }
    }


    //TODO
    //Planet has resources: food, wood, water, ore. ore for technology wood for construction

    //create stats for planet: health, resource amounts, that can be altered at both compile and run time
    //split the water mesh into water visual and available water
    //expand on the water and terrain shaders
    //implement building and feature placement by mouse
    //build the sun
    //implement orbits
    //implement skybox camera
    //let trees grow over time, and appear around the planet
    //create life (people walking about and chopping down trees, going in and out of buildings)

    //DONE fix bug where planet duplicates terrain and water faces
    //DONE create clusters of trees
    //DONE skybox

}
