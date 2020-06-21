using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ProductionBuilding: Building
{
    #region Attributes

    public float _resourceGenerationInterval; // If operating at 100% efficiency, this is the time in seconds it takes for one production cycle to finish
    public float _outputCount; // The number of output resources per generation cycle(for example the Sawmill produces 2 planks at a time)

    public bool _scalesWithNeighboringTiles; // A choice if its efficiency scales with a specific type of surrounding tile
    public int _minNeighbors; // The minimum number of surrounding tiles its efficiency scales with(0-6)
    public int _maxNeighbors; // The maximum number of surrounding tiles its efficiency scales with(0-6)
    public GameManager.ResourceTypes _inputResource; // A choice for input resource types(0, 1 or 2 types)
    public GameManager.ResourceTypes _outputResource; // A choice for output resource type
    #endregion

    #region Job
    public List<Job> _jobs = new List<Job>(); // all jobs belongs to the building
    public int _jobsCapacity;
    #endregion

    public override void InitializeBuilding(int index, Tile t)
    {
        _workers = new List<Worker>();
        this._tile = t;
        this._type = (BuildingType) index + 1; // increment by 1 since the first item in BuildingType is Empty 
        this._canBeBuiltOn = new List<Tile.TileTypes>();
        switch(this._type)
        {
            case BuildingType.Fishery:
                this._moneyCost = 100;
                this._planksCost = 2;
                this._upkeep = 40;
                this._outputCount = 1;
                this._scalesWithNeighboringTiles = true;
                this._resourceGenerationInterval = 30f;
                this._canBeBuiltOn.Add(Tile.TileTypes.Sand);
                this._minNeighbors = 1;
                this._maxNeighbors = 3;
                this._inputResource = GameManager.ResourceTypes.None;
                this._outputResource = GameManager.ResourceTypes.Fish;
                this._jobsCapacity = 25;
                this._efficiency = ComputeEfficiency();
                break;
            case BuildingType.Lumberjack:
                this._moneyCost = 100;
                this._planksCost = 0;
                this._upkeep = 10;
                this._outputCount = 1;
                this._scalesWithNeighboringTiles = true;
                this._resourceGenerationInterval = 15f;
                this._canBeBuiltOn.Add(Tile.TileTypes.Forest);
                this._minNeighbors = 1;
                this._maxNeighbors = 6;
                this._inputResource = GameManager.ResourceTypes.None;
                this._outputResource = GameManager.ResourceTypes.Wood;
                this._jobsCapacity = 5;
                this._efficiency = ComputeEfficiency();
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
                this._jobsCapacity = 10;
                break;
            case BuildingType.SheepFarm:
                this._moneyCost = 100;
                this._planksCost = 2;
                this._upkeep = 20;
                this._outputCount = 1;
                this._scalesWithNeighboringTiles = true;
                this._resourceGenerationInterval = 30f;
                this._canBeBuiltOn.Add(Tile.TileTypes.Grass);
                this._minNeighbors = 1;
                this._maxNeighbors = 4;
                this._inputResource = GameManager.ResourceTypes.None;
                this._outputResource = GameManager.ResourceTypes.Wood;
                this._jobsCapacity = 10;
                this._efficiency = ComputeEfficiency();
                break;
            case BuildingType.FrameworkKnitters:
                this._moneyCost = 400;
                this._planksCost = 2;
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
                this._jobsCapacity = 50;
                break;
            case BuildingType.PotatoFarm:
                this._moneyCost = 100;
                this._planksCost = 2;
                this._upkeep = 20;
                this._outputCount = 1;
                this._scalesWithNeighboringTiles = true;
                this._resourceGenerationInterval = 30f;
                this._canBeBuiltOn.Add(Tile.TileTypes.Grass);
                this._minNeighbors = 1;
                this._maxNeighbors = 4;
                this._inputResource = GameManager.ResourceTypes.None;
                this._outputResource = GameManager.ResourceTypes.Potato;
                this._jobsCapacity = 20;
                this._efficiency = ComputeEfficiency();
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
                this._jobsCapacity = 50;
                break;
        }
    }
    // TODO: I am not sure how to compute efficiency. Here is my understanding:
    // Efficiency is the average of
    // - surrounding tiles
    // - number of employees
    // - happiness of employees
    float ComputeEfficiency()
    {
        float result = 1f;
        // compute efficiency based on surrounding tiles 
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
            int surroundingTiles = this._tile._neighborTiles.FindAll(t => t._type == tt).Count;
            if (this._maxNeighbors <= surroundingTiles) result = 1f;
            if (this._minNeighbors > surroundingTiles) result = 0f;
            result = (float) surroundingTiles / this._maxNeighbors; 
        }

        // add effiency based on the precentage of employment
        result += (float) _workers.Count / this._jobsCapacity;
        // add effiency based on happiness of workers 
        if (_workers.Count > 0)
            result += (float)  _workers.Sum(w => w._happiness) / _workers.Count;

        Debug.Log("Efficiency: " + result);
        return (float) result * 1/3;
    }
    public override void UpdateEfficiency()
    {
        this._efficiency = ComputeEfficiency();
    }
    // register job instances in Job Manager when a building is built
    public void PopulateJobs()
    {
        for(int i = 0; i < this._jobsCapacity; i++)
            this._jobs.Add(new Job(this));

        this._jobManager.RegisterJobs(this._jobs);
    }

}