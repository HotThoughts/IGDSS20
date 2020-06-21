using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    #region Manager References
    public JobManager _jobManager; //Reference to the JobManager
    #endregion
    
    #region Workers
    public List<Worker> _workers; //List of all workers associated with this building, either for work or living
    #endregion

    #region Attributes
    public List<Tile.TileTypes> _canBeBuiltOn; // A restriction on which types of tiles it can be placed on
    public BuildingType _type; // The name of the building
    public int _upkeep; // The money cost per minute
    public int _moneyCost; // Placement money cost
    public int _planksCost; // Placement planks cost
    public Tile _tile; // Reference to the tile it is built on 
    public float _efficiency = 0f;
    #endregion

    #region Enumerations
    public enum BuildingType { Empty, Fishery, Lumberjack, Sawmill, SheepFarm, FrameworkKnitters, PotatoFarm, SchnappsDistillery, FarmersResidence };
    #endregion

    #region Methods   
    public void WorkerAssignedToBuilding(Worker w)
    {
        _workers.Add(w);
    }

    public void WorkerRemovedFromBuilding(Worker w)
    {
        _workers.Remove(w);
    }
    public abstract void UpdateEfficiency();
    public abstract void InitializeBuilding(int index, Tile t);

    #endregion
}
