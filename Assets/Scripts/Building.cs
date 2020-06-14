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

    #region Methods   

    public void WorkerAssignedToBuilding(Worker w)
    {
        _workers.Add(w);
    }

    public void WorkerRemovedFromBuilding(Worker w)
    {
        _workers.Remove(w);
    }
    #endregion
}
