using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostController : MonoBehaviour {

    /// <summary>
    /// host prefab
    /// </summary>
    public Host hostPrefab;

    /// <summary>
    /// Anchor where all hosts will instantiate
    /// </summary>
    public Transform hostAnchor;
    /// <summary>
    /// Distance between two consecutive hosts
    /// </summary>
    public float distBtwHosts;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Create n number of hosts that represents a cluster
    /// </summary>
    /// <param name="parentCluster"></param>
    private List<Host> CreateHosts(List<vapitypes.Host> vHosts, Cluster parentCluster)
    {
        var left = hostAnchor.transform.position;
        var right = hostAnchor.transform.position;
        var worldAnchor = hostAnchor.transform.position;
        var dtBtwHosts = distBtwHosts;

        List<Host> hosts = new List<Host>();

        for (int i = 0; i < vHosts.Count; i++)
        {
            Host obj = Instantiate(hostPrefab);
            if (i == 0)
            {
                obj.transform.position = worldAnchor;
            }
            else if (i % 2 != 0)
            {
                left = new Vector3(-dtBtwHosts, 0, 0) + left;
                obj.transform.position = left;
            }
            else
            {
                right = new Vector3(dtBtwHosts, 0, 0) + right;
                obj.transform.position = right;
            }
            obj.Init(vHosts[i]);
            hosts.Add(obj);
            obj.CreateVM();
            obj.transform.parent = parentCluster.transform;
        }

        return hosts;
    }


    public void Init(Cluster parentCluster)
    {
        // Request clusters from connection manager
        ConnectionManager.instance.ResquestHosts(this , parentCluster);
    }

    public void OnHostsRequested(List<vapitypes.Host> hosts, Cluster parentCluster)
    {
        var hostsCount = hosts.Count;
        if (hostsCount == 0)
        {
            Debug.Log("No Hosts found");
        }
        else
        {
            Debug.Log("Loading Hosts");
            parentCluster.hosts = CreateHosts(hosts, parentCluster);
        }
    }
}
