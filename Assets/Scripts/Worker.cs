using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : MonoBehaviour
{
    #region Manager References
    public JobManager _jobManager; //Reference to the JobManager
    public GameManager _gameManager;//Reference to the GameManager
    #endregion

    public int _age = 0; // The age of this worker
    public float _happiness = 1; // The happiness of this worker, between 0 and 1
    public bool _employed = false; // the status of employment. We will set it to true in job class

    public Building _house; // house building
    public Job _job; // reference to job, we can know where he or she works

    public Worker(Building b)
    {
        this._house = b;
    }
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("IncrementAge", 15f, 15f);
    }

    // Update is called once per frame
    void Update()
    {
        Age();
        Debug.Log("Current Age: " + this._age);
    }
    // increment age by 1
    private void IncrementAge()
    {
        _age++;
    }
    private void Age()
    {
        //TODO: Implement a life cycle, where a Worker ages by 1 year every 15 real seconds.
        //When becoming of age, the worker enters the job market, and leaves it when retiring.
        //Eventually, the worker dies and leaves an empty space in his home. His Job occupation is also freed up.

        if (_age > 14)
        {
            BecomeOfAge();
        }

        if (_age > 64)
        {
            Retire();
        }

        if (_age > 100)
        {
            Die();
        }
    }


    public void BecomeOfAge()
    {
        _jobManager.RegisterWorker(this);
    }

    private void Retire()
    {
        _jobManager.ReleaseJob(this);
    }

    private void Die()
    {
       // reset the worker
        this._age = 0;
        this._happiness = 1f;
        this._employed = false;
        // remove this worker from the house where he or she lives
        this._job._building.WorkerRemovedFromBuilding(this);

        this.gameObject.SetActive(false);
    } 
    float ComputeHappiness()
    {   //TODO: compute happiness based on supplies and employment status 
        return 1f;
    }
    private void UpdateHappiness()
    {
        this._happiness = ComputeHappiness();
    }
    private void ConsumeResources()
    {
        //TODO: periodic consumption of resources (fish, clothes & schnapps)
    }
}
