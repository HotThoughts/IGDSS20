using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Texture2D heightMap;
    public int dim;

    // Prefabs
    public GameObject waterTile;
    public GameObject sandTile;
    public GameObject grassTile;
    public GameObject forestTile;
    public GameObject stoneTile;
    public GameObject mountainTile;

    #region Map generation
    private Tile[,] _tileMap; //2D array of all spawned tiles
    #endregion

    #region Buildings
    public GameObject[] _buildingPrefabs; //References to the building prefabs
    public int _selectedBuildingPrefabIndex = 0; //The current index used for choosing a prefab to spawn from the _buildingPrefabs list
    #endregion


    #region Resources
    private Dictionary<ResourceTypes, float> _resourcesInWarehouse = new Dictionary<ResourceTypes, float>(); //Holds a number of stored resources for every ResourceType

    //A representation of _resourcesInWarehouse, broken into individual floats. Only for display in inspector, will be removed and replaced with UI later
    [SerializeField]
    private float _ResourcesInWarehouse_Fish;
    [SerializeField]
    private float _ResourcesInWarehouse_Wood;
    [SerializeField]
    private float _ResourcesInWarehouse_Planks;
    [SerializeField]
    private float _ResourcesInWarehouse_Wool;
    [SerializeField]
    private float _ResourcesInWarehouse_Clothes;
    [SerializeField]
    private float _ResourcesInWarehouse_Potato;
    [SerializeField]
    private float _ResourcesInWarehouse_Schnapps;
    #endregion

    #region Enumerations
    public enum ResourceTypes { None, Fish, Wood, Planks, Wool, Clothes, Potato, Schnapps }; //Enumeration of all available resource types. Can be addressed from other scripts by calling GameManager.ResourceTypes
    #endregion

    #region MonoBehaviour

    // awake is called before any Start functions
    void Awake()
    {
        dim = heightMap.width;

        for (int i = 0; i < dim; i++)
            for (int j = 0; j < dim; j++)
            {
                float height = heightMap.GetPixel(i, j).r;

                // We need bias to arrange hexagons over X axis
                int bias = i % 2 == 0 ? 0 : 5;
                // Spawn tiles (8.66 IS MAGIC NUM HERE)
                float magicNum = 8.66f;
                if (height == 0f)
                    Instantiate(waterTile, 
                        new Vector3(i * magicNum, height * 10, j * 10 + bias), 
                        new Quaternion(0f, 0f, 0f, 0f));
                else if (height > 0f && height <= 0.2f)
                    Instantiate(sandTile,
                        new Vector3(i * magicNum, height * 10, j * 10 + bias),
                        new Quaternion(0f, 0f, 0f, 0f));
                else if (height > 0.2f && height <= 0.4f)
                    Instantiate(grassTile,
                        new Vector3(i * magicNum, height * 10, j * 10 + bias),
                        new Quaternion(0f, 0f, 0f, 0f));
                else if (height > 0.4f && height <= 0.6f)
                    Instantiate(forestTile,
                        new Vector3(i * magicNum, height * 10, j * 10 + bias),
                        new Quaternion(0f, 0f, 0f, 0f));
                else if (height > 0.6f && height <= 0.8f)
                    Instantiate(stoneTile,
                        new Vector3(i * magicNum, height * 10, j * 10 + bias),
                        new Quaternion(0f, 0f, 0f, 0f));
                else
                    Instantiate(mountainTile,
                        new Vector3(i * magicNum, height * 10, j * 10 + bias),
                        new Quaternion(0f, 0f, 0f, 0f));
            }
    }

    // Start is called before the first frame update
    void Start()
    {
        PopulateResourceDictionary();
    }

    // Update is called once per frame
    void Update()
    {
        HandleKeyboardInput();
        UpdateInspectorNumbersForResources();
        
    }
    #endregion

    #region Methods
    //Makes the resource dictionary usable by populating the values and keys
    void PopulateResourceDictionary()
    {
        _resourcesInWarehouse.Add(ResourceTypes.None, 0);
        _resourcesInWarehouse.Add(ResourceTypes.Fish, 0);
        _resourcesInWarehouse.Add(ResourceTypes.Wood, 0);
        _resourcesInWarehouse.Add(ResourceTypes.Planks, 0);
        _resourcesInWarehouse.Add(ResourceTypes.Wool, 0);
        _resourcesInWarehouse.Add(ResourceTypes.Clothes, 0);
        _resourcesInWarehouse.Add(ResourceTypes.Potato, 0);
        _resourcesInWarehouse.Add(ResourceTypes.Schnapps, 0);
    }

    //Sets the index for the currently selected building prefab by checking key presses on the numbers 1 to 0
    void HandleKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _selectedBuildingPrefabIndex = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _selectedBuildingPrefabIndex = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _selectedBuildingPrefabIndex = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _selectedBuildingPrefabIndex = 3;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            _selectedBuildingPrefabIndex = 4;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            _selectedBuildingPrefabIndex = 5;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            _selectedBuildingPrefabIndex = 6;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            _selectedBuildingPrefabIndex = 7;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            _selectedBuildingPrefabIndex = 8;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            _selectedBuildingPrefabIndex = 9;
        }
    }

    //Updates the visual representation of the resource dictionary in the inspector. Only for debugging
    void UpdateInspectorNumbersForResources()
    {
        _ResourcesInWarehouse_Fish = _resourcesInWarehouse[ResourceTypes.Fish];
        _ResourcesInWarehouse_Wood = _resourcesInWarehouse[ResourceTypes.Wood];
        _ResourcesInWarehouse_Planks = _resourcesInWarehouse[ResourceTypes.Planks];
        _ResourcesInWarehouse_Wool = _resourcesInWarehouse[ResourceTypes.Wool];
        _ResourcesInWarehouse_Clothes = _resourcesInWarehouse[ResourceTypes.Clothes];
        _ResourcesInWarehouse_Potato = _resourcesInWarehouse[ResourceTypes.Potato];
        _ResourcesInWarehouse_Schnapps = _resourcesInWarehouse[ResourceTypes.Schnapps];
    }

    //Checks if there is at least one material for the queried resource type in the warehouse
    public bool HasResourceInWarehoues(ResourceTypes resource)
    {
        return _resourcesInWarehouse[resource] >= 1;
    }

    //Is called by MouseManager when a tile was clicked
    //Forwards the tile to the method for spawning buildings
    public void TileClicked(int height, int width)
    {
        Tile t = _tileMap[height, width];

        PlaceBuildingOnTile(t);
    }

    //Checks if the currently selected building type can be placed on the given tile and then instantiates an instance of the prefab
    private void PlaceBuildingOnTile(Tile t)
    {
        //if there is building prefab for the number input
        if (_selectedBuildingPrefabIndex < _buildingPrefabs.Length)
        {
            //TODO: check if building can be placed and then istantiate it

        }
    }

    //Returns a list of all neighbors of a given tile
    private List<Tile> FindNeighborsOfTile(Tile t)
    {
        List<Tile> result = new List<Tile>();

        //TODO: put all neighbors in the result list

        return result;
    }
    #endregion
}
