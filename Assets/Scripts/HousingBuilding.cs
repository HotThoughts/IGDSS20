using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class HousingBuilding : Building {
    private int maxWorker = 10;
    private int initWorker = 2;

    public override void InitializeBuilding(int index, Tile t)
    {
        _workers = new List<Worker>();
    }
    private void Awake()
    {
        // Temporary placement (TODO: Find more appropriate place)
        _canBeBuiltOn = new List<Tile.TileTypes>();
        _canBeBuiltOn.Add(Tile.TileTypes.Forest);
        _canBeBuiltOn.Add(Tile.TileTypes.Grass);
        _canBeBuiltOn.Add(Tile.TileTypes.Mountain);
        _canBeBuiltOn.Add(Tile.TileTypes.Sand);
        _canBeBuiltOn.Add(Tile.TileTypes.Stone);
        _moneyCost = 0;
        _upkeep = 0;
        _planksCost = 0;
    }
    void Start()
    {
        PopulateWorkers();
        InvokeRepeating("RegisterChildWorker", 30f/this._efficiency, 30f/this._efficiency);
    }
    void Update() 
    {
        UpdateEfficiency();
    }
    // Register two grown workers 
    private void PopulateWorkers()
    {
        for (int i = 0; i < initWorker; i++)
            GetWorkerFromPooler(18);
    }
    // Register child worker
    private void RegisterChildWorker()
    {
        if(_workers.Count < maxWorker)
            GetWorkerFromPooler(0);
    }
    // Get Worker object from Pooled GameObject
    private void GetWorkerFromPooler(int age)
    {
        GameObject obj = (GameObject) WorkerPooler.Instance.GetPooledWorker();

        if (obj == null) return;

        Worker w = obj.GetComponent<Worker>() as Worker; 
        w._age = age;
        w._house = this;
        WorkerAssignedToBuilding(w);

        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
        obj.SetActive(true);
    }
    // The efficiency should depend on the average happiness of the workers living there. 
    float ComputeEfficiency()
    {   
       
        float sum = 0;
        foreach(Worker w in _workers)
            sum += w._happiness;
        // happiness is in the range (0, 1), 
        // so we can get a value in (0,1) by dividing the number of works
        return sum/_workers.Count;
    }
    public override void UpdateEfficiency()
    {
        this._efficiency = ComputeEfficiency();
    }
}