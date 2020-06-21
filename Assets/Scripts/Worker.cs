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

    public HousingBuilding _house; // house building
    public Job _job; // reference to job, we can know where he or she works
    public List<GameManager.ResourceTypes> _resoucesToConsume 
    {
        get { return new List<GameManager.ResourceTypes> {
            GameManager.ResourceTypes.Fish,
            GameManager.ResourceTypes.Schnapps,
            GameManager.ResourceTypes.Clothes
        }; }
    } // resources that each worker consumes

    public Worker(HousingBuilding b)
    {
        this._house = b;
    }
    private void Awake()
    {
        _jobManager = JobManager.Instance;
        _gameManager = GameManager.Instance;
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
        EventualDeath();
        _age++;
        ConsumeResources();
    }
    private void Age()
    {
        //When becoming of age, the worker enters the job market, and leaves it when retiring.
        if (_age > 100)
        {
            Die();
            return;
        }
        if (_age > 64)
        {
            Retire();
            return;
        }
        if (_age > 14)
        {
            BecomeOfAge();
            return;
        }
    }

    private void EventualDeath()
    {
        //Eventually, the worker dies and leaves an empty space in his home. His Job occupation is also freed up.
        float prob = Random.Range(0f, 1f);
        // Calculation of death: 0.015 * Age - Happiness
        if (0.015f * _age - _happiness > prob)
            Die();
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
        Retire();
        // reset the worker
        this._age = 0;
        this._happiness = 1f;
        this._employed = false;
        // remove this worker from the house where he or she lives
        this._house.WorkerRemovedFromBuilding(this);
        // remove this worker from the house where he or she works
        if (this._job != null)
            this._job._prodBuilding.WorkerRemovedFromBuilding(this);

        this.gameObject.SetActive(false);
    } 
    float ComputeHappiness()
    {
        // Employment status and resources in warehouse give each 1/4 part of
        // the whole happiness
        float _hasJob = 0.25f;
        float _happinessGrade = 0f;
        // Does worker have a job?
        if (this._job != null)
            _happinessGrade += _hasJob;
        // Is worker supplied with resources?
        foreach (var res in _resoucesToConsume)
        {
            if (_gameManager.HasResourceInWarehouse(res))
                _happinessGrade += 0.25f;
        }

        return _happinessGrade;
    }
    private void UpdateHappiness()
    {
        this._happiness = ComputeHappiness();
    }
    private void ConsumeResources()
    {
        foreach(var res in _resoucesToConsume)
        {
            if (_gameManager.HasResourceInWarehouse(res))
                _gameManager._resourcesInWarehouse[res]--;
        }
    }
}
