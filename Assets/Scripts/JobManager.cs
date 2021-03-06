﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JobManager : MonoBehaviour
{

    public List<Job> _availableJobs = new List<Job>();
    public List<Worker> _unoccupiedWorkers = new List<Worker>();
    public List<Worker> _currentlyWorking = new List<Worker>();
    private int numWorkers;

    private static JobManager _instance;
    public static JobManager Instance
    {
        get
        {
            return _instance ? _instance : (_instance = (new GameObject("JobManager").AddComponent<JobManager>()));
        }
    }

    #region MonoBehaviour
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HandleUnoccupiedWorkers();
    }
    #endregion


    #region Methods

    private void HandleUnoccupiedWorkers()
    {
        if (_unoccupiedWorkers.Count > 0)
        {
            // InvalidOperationException fix
            List<Worker> _tempListOfWorkers = new List<Worker>(_unoccupiedWorkers);
            //TODO: What should be done with unoccupied workers?
            foreach(Worker w in _tempListOfWorkers)
            {
                if(_availableJobs == null || _availableJobs.Count == 0) break;
                else
                {   // Assign a job to a worker randomly
                   int index = Random.Range(0, _availableJobs.Count);
                   Job job = _availableJobs[index];
                   RemoveJob(index);
                   // Update properties of job
                   job.AssignWorker(w);
                   job._prodBuilding.WorkerAssignedToBuilding(w);
                   
                   RemoveWorker(w);
                   w._job = job;
                   w.InvokeCommuting();
                }

            }
        }
    }

    public void RegisterJobs(List<Job> j)
    {
        _availableJobs.AddRange(j);
    }
    public void RegisterJob(Job j)
    {
        _availableJobs.Add(j);
    }
    public void RemoveJob(Job j)
    {
        _availableJobs.Remove(j);
    }
    public void RemoveJob(int index)
    {
        _availableJobs.RemoveAt(index);
    }

    public void RegisterWorker(Worker w)
    {
        if (!_unoccupiedWorkers.Contains(w) && !_currentlyWorking.Contains(w))
        {
            _unoccupiedWorkers.Add(w);
            _currentlyWorking.Remove(w);
        }
    }
    public void RemoveWorker(Worker w)
    {
        _unoccupiedWorkers.Remove(w);
        _currentlyWorking.Add(w);
    }
    public void ReleaseJob(Worker w)
    {
        if (w._job != null)
        {
            w._job._worker = null;
            RegisterJob(w._job);
        }
    }
    // return the total number of active workers
    public int GetNumOfWorkers()
    {
        return this._unoccupiedWorkers.Count + this._currentlyWorking.Count;
    }

    #endregion
}
