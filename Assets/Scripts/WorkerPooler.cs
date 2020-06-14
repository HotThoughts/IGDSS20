using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerPooler : MonoBehaviour
{
    public static WorkerPooler current;
    public GameObject worker;
    public int pooledAmount = 100;
    public bool willGrow = true;
    List<GameObject> workers;
    void Awake()
    {
        current = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        workers = new List<GameObject>();
        for(int i = 0; i < pooledAmount; i++)
        {
            GameObject obj = (GameObject) Instantiate(worker);
            obj.AddComponent<Worker>();
            obj.SetActive(false);
            workers.Add(obj);
        }
    }

    public GameObject GetPooledWorker()
    {
        foreach(GameObject w in workers)
            if(!w.activeInHierarchy)
                return w;
        if(willGrow)
        {
            GameObject w = (GameObject) Instantiate(worker); 
            workers.Add(w);
            return w;
        }

        return null;
    }
}
