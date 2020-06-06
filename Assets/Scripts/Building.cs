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

        switch(this._type)
        {
            case BuildingType.Fishery:
                this._moneyCost = 100;
                this._planksCost = 2;
                this._upkeep = 40;
                this._outputCount = 1;
                this._efficiency = ComputeEfficiency();
                this._scalesWithNeighboringTiles = true;
                this._resourceGenerationInterval = 30f;
                this._canBeBuiltOn.Add(Tile.TileTypes.Sand);
                this._minNeighbors = 1;
                this._maxNeighbors = 3;
                this._inputResource = GameManager.ResourceTypes.None;
                this._outputResource = GameManager.ResourceTypes.Fish;
                break;
            case BuildingType.Lumberjack:
                this._moneyCost = 100;
                this._planksCost = 0;
                this._upkeep = 10;
                this._outputCount = 1;
                this._efficiency = ComputeEfficiency();
                this._scalesWithNeighboringTiles = true;
                this._resourceGenerationInterval = 15f;
                this._canBeBuiltOn.Add(Tile.TileTypes.Forest);
                this._minNeighbors = 1;
                this._maxNeighbors = 6;
                this._inputResource = GameManager.ResourceTypes.None;
                this._outputResource = GameManager.ResourceTypes.Wood;
                break;
            case BuildingType.Sawmill:
                this._moneyCost = 100;
                this._planksCost = 0;
                this._upkeep = 10;
                this._outputCount = 2;
                this._efficiency = 1f;
                this._scalesWithNeighboringTiles = false;
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
                this._efficiency = ComputeEfficiency();
                this._scalesWithNeighboringTiles = true;
                this._resourceGenerationInterval = 30f;
                this._canBeBuiltOn.Add(Tile.TileTypes.Grass);
                this._minNeighbors = 1;
                this._maxNeighbors = 4;
                this._inputResource = GameManager.ResourceTypes.None;
                this._outputResource = GameManager.ResourceTypes.Wood; 
                break;
            case BuildingType.FrameworkKnitters:
                this._moneyCost = 400;
                this._planksCost = 20;
                this._upkeep = 50;
                this._outputCount = 1;
                this._efficiency = 1f;
                this._scalesWithNeighboringTiles = false;
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
                this._efficiency = ComputeEfficiency();
                this._scalesWithNeighboringTiles = true;
                this._resourceGenerationInterval = 30f;
                this._canBeBuiltOn.Add(Tile.TileTypes.Grass);
                this._minNeighbors = 1;
                this._maxNeighbors = 4;
                this._inputResource = GameManager.ResourceTypes.None;
                this._outputResource = GameManager.ResourceTypes.Potato; 
                break;
            case BuildingType.SchnappsDistillery:
                this._moneyCost = 100;
                this._planksCost = 2;
                this._upkeep = 40;
                this._outputCount = 1;
                this._efficiency = 1f;
                this._scalesWithNeighboringTiles = false;
                this._resourceGenerationInterval = 30f;
                this._canBeBuiltOn.Add(Tile.TileTypes.Grass);
                this._canBeBuiltOn.Add(Tile.TileTypes.Forest);
                this._canBeBuiltOn.Add(Tile.TileTypes.Stone);
                this._inputResource = GameManager.ResourceTypes.Potato;
                this._outputResource = GameManager.ResourceTypes.Schnapps; 
                break;
        }
    }
    float ComputeEfficiency()
    {
        if (this._scalesWithNeighboringTiles)
        {
            Tile.TileTypes tt = Tile.TileTypes.Empty;
            switch(this._type)
            {
                case BuildingType.Fishery:
                    tt = Tile.TileTypes.Water;
                    break;
                case BuildingType.Lumberjack:
                    tt = Tile.TileTypes.Forest;
                    break;
                case BuildingType.SheepFarm:
                case BuildingType.PotatoFarm:
                    tt = Tile.TileTypes.Grass;
                    break;
            }
            int surroundingTiles = this._tile._neighborTiles.FindAll(t => t._type == tt).Count;; 
            if (this._maxNeighbors <= surroundingTiles) return 1f;
            if (this._minNeighbors > surroundingTiles) return 0f;
            return surroundingTiles / this._maxNeighbors; 
        }
        return 1f;
    }

    public void UpdateEfficiency()
    {
        this._efficiency = ComputeEfficiency();
    }
}