using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Job
{
    public Worker _worker; //The worker occupying this job
    public ProductionBuilding _prodBuilding; //The building offering the job

    //Constructor. Call new Job(this) from the Building script to instanciate a job
    public Job(ProductionBuilding building)
    {
        _prodBuilding = building;
    }

    public void AssignWorker(Worker w)
    {
        w._employed = true;
        _worker = w;
        _prodBuilding.WorkerAssignedToBuilding(w);
    }

    public void RemoveWorker(Worker w)
    {
        _worker = null;
        _prodBuilding.WorkerRemovedFromBuilding(w);
    }
}
