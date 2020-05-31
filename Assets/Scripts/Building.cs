using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building: MonoBehaviour
{
    #region Attributes
    public BuildingType _type; // The name of the building
    public int _upkeep; // The money cost per minute
    public int _moneyCost; // placement money cost
    public int _planksCost; // placement planks cost
    public Tile _tile; // R eference to the tile it is built on 

    public float _efficiency; // Calculated based on the surrounding tile types
    public float _resourceGenerationInterval; // If operating at 100% efficiency, this is the time in seconds it takes for one production cycle to finish
    public float _outputCount; // The number of output resources per generation cycle(for example the Sawmill produces 2 planks at a time)

    public List<Tile.TileTypes> _canBeBuiltOn; // A restriction on which types of tiles it can be placed on
    public bool _scalesWithNeighboringTiles; // A choice if its efficiency scales with a specific type of surrounding tile
    public int _minNeighbors; // The minimum number of surrounding tiles its efficiency scales with(0-6)
    public int _maxNeighbors; // The maximum number of surrounding tiles its efficiency scales with(0-6)
    public List<GameManager.ResourceTypes> _inputResources; // A choice for input resource types(0, 1 or 2 types)
    public GameManager.ResourceTypes _outputResource; // A choice for output resource type
    #endregion

    #region Enumerations
    public enum BuildingType { Empty, Fishery, Lumberjack, Sawmill, SheepFarm, FrameworkKnitters, PotatoFarm, SchnappsDistillery };
    #endregion
}