using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APIClientTest : MonoBehaviour {
    VCenterClient client;
    List<vapitypes.Cluster> clusters;
    List<vapitypes.Host> hosts;
    List<vapitypes.Summary> vms;

    // Use this for initialization
    void Start () {
        client = new VCenterClient("http://163.182.168.150",
                "administrator@neetcloud.local", "N33tcloud1!");
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {

            client.Authenticate((exception, authenticated) =>
            {
                if (authenticated)
                {
                    Debug.Log("Authenticated!");
                }
                else
                {
                    Debug.Log(exception.Message);
                }
            });
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            client.ListClusters((exception, clusterList) =>
            {
                if(clusterList != null)
                {
                    clusters = clusterList.value;
                    foreach(var cluster in clusters)
                    {
                        Debug.Log(cluster.name);
                    }
                } else
                {
                    Debug.Log(exception.Message);
                }
            });
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            client.ListHostsForCluster(clusters[0].cluster, (exception, hostList) =>
            {
                if (hostList != null)
                {
                    hosts = hostList.value;
                    foreach (var host in hosts)
                    {
                        Debug.Log(host.name);
                    }
                }
                else
                {
                    Debug.Log(exception.Message);
                }
            });
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            client.ListVmsForHost("host-1444", (exception, vmList) =>
            {
                if (vmList != null)
                {
                    vms = vmList.value;
                    foreach (var vm in vms)
                    {
                        Debug.Log(vm.name + " " + vm.power_state + " " + vm.memory_size_MiB + " " + vm.cpu_count);
                    }
                }
                else
                {
                    Debug.Log(exception.Message);
                }
            });
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            client.StartVM(vms[0].vm, (exception) =>
            {
                if (exception == null)
                {
                    Debug.Log(vms[0].vm + " started");
                }
                else
                {
                    Debug.Log(exception.Message);
                }
            });
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            client.StopVM(vms[0].vm, (exception) =>
            {
                if (exception == null)
                {
                    Debug.Log(vms[0].vm + " stopped");
                }
                else
                {
                    Debug.Log(exception.Message);
                }
            });
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            client.DeleteVM(vms[0].vm, (exception) =>
            {
                if (exception == null)
                {
                    Debug.Log(vms[0].vm + " Deleted");
                }
                else
                {
                    Debug.Log(exception.Message);
                }
            });
        }
    }
}
