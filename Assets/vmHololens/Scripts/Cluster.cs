using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class Cluster : MonoBehaviour
{

    private HostController hostController;
    private int hostsCount = 1;
    public List<Host> hosts = new List<Host>();
    
    public string info;

    /// <summary>
    /// Distance between two consecutive hosts
    /// </summary>
    public float distBtwHosts;

    private vapitypes.Cluster aboutThisCluster;

    public vapitypes.Cluster AboutThisCluster
    {
        get
        {
            return aboutThisCluster;
        }
        set
        {
            aboutThisCluster = value;
        }
    }

    void Awake()
    {
        hostController = GameObject.FindGameObjectWithTag("vSphereController").GetComponent<HostController>();
    }

    void Start()
    {
    }

    void Update()
    {

    }


    public void Init(vapitypes.Cluster cluster)
    {
        AboutThisCluster = cluster;
        //hosts = hostController.CreateHosts(this, hostsCount);
        hostController.Init(this);
    }
}
