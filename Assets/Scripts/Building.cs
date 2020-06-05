using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building: MonoBehaviour
{
    #region Attributes
    public BuildingType _type; // The name of the building
    public int _upkeep; // The money cost per minute
    public int _moneyCost; // Placement money cost
    public int _planksCost; // Placement planks cost
    public Tile _tile; // Reference to the tile it is built on 

    public float _efficiency; // Calculated based on the surrounding tile types
    public float _resourceGenerationInterval; // If operating at 100% efficiency, this is the time in seconds it takes for one production cycle to finish
    public float _outputCount; // The number of output resources per generation cycle(for example the Sawmill produces 2 planks at a time)

    public List<Tile.TileTypes> _canBeBuiltOn; // A restriction on which types of tiles it can be placed on
    public bool _scalesWithNeighboringTiles; // A choice if its efficiency scales with a specific type of surrounding tile
    public int _minNeighbors; // The minimum number of surrounding tiles its efficiency scales with(0-6)
    public int _maxNeighbors; // The maximum number of surrounding tiles its efficiency scales with(0-6)
    public GameManager.ResourceTypes _inputResource; // A choice for input resource types(0, 1 or 2 types)
    public GameManager.ResourceTypes _outputResource; // A choice for output resource type
    #endregion

    #region Enumerations
    public enum BuildingType { Empty, Fishery, Lumberjack, Sawmill, SheepFarm, FrameworkKnitters, PotatoFarm, SchnappsDistillery };
    #endregion


    public void InitializeBuilding(int index, Tile t)
    {
        this._tile = t;
        this._type = (BuildingType) index + 1; // increment by 1 since the first item in BuildingType is Empty 
        this._efficiency = ComputeEfficiency(t);
        this._scalesWithNeighboringTiles = this._efficiency > 0;
        
        switch(this._type)
        {
            case BuildingType.Fishery:
                this._moneyCost = 100;
                this._planksCost = 2;
                this._upkeep = 40;
                this._outputCount = 1;
                this._resourceGenerationInterval = 30f;
                this._canBeBuiltOn.Add(Tile.TileTypes.Sand);
                this._minNeighbors = 1;
                this._maxNeighbors = 3;
                this._outputResource = GameManager.ResourceTypes.Fish;
                break;
            case BuildingType.Lumberjack:
                this._moneyCost = 100;
                this._planksCost = 0;
                this._upkeep = 10;
                this._outputCount = 1;
                this._resourceGenerationInterval = 15f;
                this._canBeBuiltOn.Add(Tile.TileTypes.Forest);
                this._minNeighbors = 1;
                this._maxNeighbors = 6;
                this._outputResource = GameManager.ResourceTypes.Wood;
                break;
            case BuildingType.Sawmill:
                this._moneyCost = 100;
                this._planksCost = 0;
                this._upkeep = 10;
                this._outputCount = 2;
                this._resourceGenerationInterval = 15f;
                this._canBeBuiltOn.Add(Tile.TileTypes.Grass);
                this._canBeBuiltOn.Add(Tile.TileTypes.Forest);
                this._canBeBuiltOn.Add(Tile.TileTypes.Stone);
                this._inputResource = GameManager.ResourceTypes.Wood;
                this._outputResource = GameManager.ResourceTypes.Planks; 
                break;
            case BuildingType.SheepFarm:
                this._moneyCost = 100;
                this._planksCost = 2;
                this._upkeep = 20;
                this._outputCount = 1;
                this._resourceGenerationInterval = 30f;
                this._canBeBuiltOn.Add(Tile.TileTypes.Grass);
                this._minNeighbors = 1;
                this._maxNeighbors = 4;
                this._outputResource = GameManager.ResourceTypes.Wood; 
                break;
            case BuildingType.FrameworkKnitters:
                this._moneyCost = 400;
                this._planksCost = 20;
                this._upkeep = 50;
                this._outputCount = 1;
                this._resourceGenerationInterval = 30f;
                this._canBeBuiltOn.Add(Tile.TileTypes.Grass);
                this._canBeBuiltOn.Add(Tile.TileTypes.Forest);
                this._canBeBuiltOn.Add(Tile.TileTypes.Stone);
                this._inputResource = GameManager.ResourceTypes.Wood;
                this._outputResource = GameManager.ResourceTypes.Clothes;  
                break;
            case BuildingType.PotatoFarm:
                this._moneyCost = 100;
                this._planksCost = 2;
                this._upkeep = 20;
                this._outputCount = 1;
                this._resourceGenerationInterval = 30f;
                this._canBeBuiltOn.Add(Tile.TileTypes.Grass);
                this._minNeighbors = 1;
                this._maxNeighbors = 4;
                this._outputResource = GameManager.ResourceTypes.Potato; 
                break;
            case BuildingType.SchnappsDistillery:
                this._moneyCost = 100;
                this._planksCost = 2;
                this._upkeep = 40;
                this._outputCount = 1;
                this._resourceGenerationInterval = 30f;
                this._canBeBuiltOn.Add(Tile.TileTypes.Grass);
                this._canBeBuiltOn.Add(Tile.TileTypes.Forest);
                this._canBeBuiltOn.Add(Tile.TileTypes.Stone);
                this._inputResource = GameManager.ResourceTypes.Potato;
                this._outputResource = GameManager.ResourceTypes.Schnapps; 
                break;
        }
    }
    float ComputeEfficiency(Tile t)
    {
        switch(this._type)
        {
            case BuildingType.Fishery:
                return ComputeSurroundingTiles(t, Tile.TileTypes.Water) / this._maxNeighbors; 
            case BuildingType.Lumberjack:
                return ComputeSurroundingTiles(t, Tile.TileTypes.Forest) / this._maxNeighbors;
            case BuildingType.SheepFarm:
            case BuildingType.PotatoFarm:
                return ComputeSurroundingTiles(t, Tile.TileTypes.Grass) / this._maxNeighbors; 
        }
        // Otherwise
        return 0f;
    }

    int ComputeSurroundingTiles(Tile tile, Tile.TileTypes type)
    {
        return tile._neighborTiles.FindAll(t => t._type == type).Count;
    }
}