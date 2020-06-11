using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Job
{
    public Worker _worker; //The worker occupying this job
    public Building _building; //The building offering the job

    //Constructor. Call new Job(this) from the Building script to instanciate a job
    public Job(Building building)
    {
        _building = building;
    }

    public void AssignWorker(Worker w)
    {
        _worker = w;
        _building.WorkerAssignedToBuilding(w);
    }

    public void RemoveWorker(Worker w)
    {
        _worker = null;
        _building.WorkerRemovedFromBuilding(w);
    }
}
