using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionManager : MonoBehaviour {

    public Text log;

    VCenterClient client;
    List<vapitypes.Host> hosts;
    List<vapitypes.Summary> vms;

    /// <summary>
    /// Call back to handle login sucess or failed
    /// </summary>
    /// <param name="loginSucceded"></param>
    public delegate void LoginAction(bool isLoginSucess);
    public event LoginAction OnLogin;



    /// <summary>
    /// Call back to handle cluster request
    /// </summary>
    /// <param name="loginSucceded"></param>
    //public delegate void ClusterRequestedAction(List<vapitypes.Cluster>());
    //public event ClusterRequestedAction OnClusterRequested;


    public static ConnectionManager instance = null;

    //Awake is always called before any Start functions
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }


    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update () {
    }

    public void CreateClientAndLogin(string ip, string uName, string password)
    {
        client = new VCenterClient("https://"+ip, uName, password);
        client.Authenticate((exception, authenticated) =>
        {
            if (authenticated)
            {
                Debug.Log("Authentication Sucessful, now request for clusters");
                if (OnLogin != null)
                {
                    OnLogin(true);
                }
            }
            else
            {
                Debug.LogError(exception.Message);
                if (OnLogin != null)
                {
                    OnLogin(false);
                }
            }
        });
    }

    /// <summary>
    /// Return list of clusters
    /// </summary>
    /// <returns></returns>
    public void RequestClusters (ClusterController clusterController)
    {
        List<vapitypes.Cluster> clusters = new List<vapitypes.Cluster>();
        client.ListClusters((exception, clusterList) =>
        {
            if (clusterList != null)
            {
                clusters = clusterList.value;
                clusterController.OnClusterRequested(clusters);
            }
            else
            {
                Debug.Log(exception.Message);
            }
        });
    }

    /// <summary>
    /// Request hosts under a cluster
    /// </summary>
    public void ResquestHosts(HostController hostController, Cluster _cluster)
    {
        client.ListHostsForCluster(_cluster.AboutThisCluster.cluster, (exception, hostList) =>
        {
            if (hostList != null)
            {
                hosts = hostList.value;
                hostController.OnHostsRequested(hosts, _cluster);
            }
            else
            {
                Debug.Log(exception.Message);
            }
        });
    }


    /// <summary>
    /// Request VMs under a Host
    /// </summary>
    public void ResquestForVMs(VMController hostController, Host _host)
    {
        client.ListVmsForHost(_host.AboutThisHost.host, (exception, vmList) =>
        {
            if (vmList != null)
            {
                vms = vmList.value;
                hostController.OnVMRquested(vms, _host);
            }
            else
            {
                Debug.Log(exception.Message);
            }
        });
    }


    public void TurnOnVM(VM vm)
    {
        client.StartVM(vm.AboutThisVM.vm, (exception) =>
        {
            if (exception == null)
            {
                Debug.Log(vm.AboutThisVM.vm + " Started");
                vm.PowerStateChanged(true);
            }
            else
            {
                Debug.Log(exception.Message);
                log.text = "" + exception.Message;
            }
        });
    }

    public void TurnOffVM(VM vm)
    {
        client.StopVM(vm.AboutThisVM.vm, (exception) =>
        {
            if (exception == null)
            {
                Debug.Log(vm.AboutThisVM.vm + " stopped");
                vm.PowerStateChanged(false);
            }
            else
            {
                Debug.Log(exception.Message);
                log.text = "" + exception.Message;
            }
        });
    }

    public void DeleteVM(VM vm)
    {
        client.DeleteVM(vm.AboutThisVM.vm, (exception) =>
        {
            if (exception == null)
            {
                Debug.Log(vm.AboutThisVM.vm + " Deleted");
                vm.DeleteThisVM();
            }
            else
            {
                Debug.Log(exception.Message);
                log.text = "" + exception.Message;
            }
        });
    }
}
