using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


/// <summary>
/// Manages the lifecycle of all clusters in the scene
/// </summary>
public class ClusterController : MonoBehaviour {


    /// <summary>
    /// Anchor where all hosts will instantiate
    /// </summary>
    public Transform hostAnchor;

    /// <summary>
    /// Host prefab
    /// </summary>
    public Host hostPrefab;

    /// <summary>
    /// UI Canvas on which we display all the clusters
    /// </summary>
    public GameObject clusterCanvas;

    /// <summary>
    /// All cluster holder in UI
    /// </summary>
    public GameObject clusterUIHolder;

    /// <summary>
    /// Loading text when requsting for clusters
    /// </summary>
    public Text loadingText;

    /// <summary>
    /// Cluster prefab that have the info of each cluster and on 
    /// which when clicked will open the hosts 
    /// </summary>
    public GameObject clusterUIButton;

    /// <summary>
    /// List of all the clusters that are currently available to user in scene
    /// </summary>
    public List<Cluster> currentClustersInApp = new List<Cluster>();

    
    /// <summary>
    /// List of clusters received from connection manager
    /// </summary>
    //private List<vapitypes.Cluster> clusters;



    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable()
    {
        //ConnectionManager.instance.OnClusterRequested += OnClusterRequested;
    }

    void OnDisable()
    {
        //ConnectionManager.instance.OnClusterRequested -= OnClusterRequested;
    }

    /// <summary>
    /// Subscribed to ClusterRequestedAction() in 
    /// ConnectionManager as this is Async call,
    /// will show clusters when request is done
    /// </summary>
    public void OnClusterRequested(List<vapitypes.Cluster> vClusters)
    {
        var clusterCount = vClusters.Count;
        if (clusterCount == 0)
        {
            ShowLoadingText(true, "No Clusters found");
        }
        else
        {
            ShowLoadingText(true, "Your Clusters!");
            CreateClusterUI(vClusters);
        }
    }

    /// <summary>
    /// Initialize the clusters
    /// </summary>
    public void Init()
    {
        clusterCanvas.SetActive(true);
        ShowLoadingText(true, "Loading...");

        // Request clusters from connection manager
        ConnectionManager.instance.RequestClusters(this);
    }

    /// <summary>
    /// Create cluster button on UI
    /// </summary>
    private void CreateClusterUI(List<vapitypes.Cluster> clusters)
    {
        for (int i = 0; i < clusters.Count; i++)
        {
            GameObject obj = Instantiate(clusterUIButton);
            obj.name = clusters[i].name;
            obj.transform.GetChild(1).GetComponent<Text>().text = "Name :"+clusters[i].name + "\nCluster :" + clusters[i].cluster + "\nHA Enable: " + clusters[i].ha_enabled + "\nDRS Enable: " + clusters[i].drs_enabled;
            obj.transform.parent = clusterUIHolder.transform;
            obj.transform.localPosition = new Vector3(0, 0, 0);
            obj.transform.localScale = new Vector3(1, 1, 1);

            //ref sent to the button
            obj.GetComponent<ClusterUIButton>().ClusterRefInScene = CreateClusters(clusters[i]);
        }
    }

    /// <summary>
    /// Create Clusters i.e Hosts
    /// and returns ref of the cluster so that
    /// it can be sent to UI button to associate it with that button
    /// </summary>
    private Cluster CreateClusters(vapitypes.Cluster cluster)
    {
        GameObject clusterHolder = new GameObject(cluster.name, typeof(Cluster));

        var _cluster = clusterHolder.GetComponent<Cluster>();

        _cluster.Init(cluster);

        currentClustersInApp.Add(_cluster);
        
        clusterHolder.SetActive(false);
        return clusterHolder.GetComponent<Cluster>();
    }


    /// <summary>
    /// Hide clusters when UI button of cluster is clicked
    /// </summary>
    public void EnableCluster(Cluster ClusterRefInScene)
    {
        foreach (var item in currentClustersInApp)
        {
            if (item == ClusterRefInScene && ClusterRefInScene.gameObject.activeInHierarchy)
            {
                ClusterRefInScene.gameObject.SetActive(false);
            }
            else if (item == ClusterRefInScene && !ClusterRefInScene.gameObject.activeInHierarchy)
            {
                ClusterRefInScene.gameObject.SetActive(true);
                
                // Request info about this cluster from the Cloud
                //ClusterRefInScene.Init(ClusterRefInScene.AboutThisCluster);
            }
            else {
                item.gameObject.SetActive(false);
            }
        }
    }

    private void ShowLoadingText(bool show, String arg)
    {
        loadingText.text = "" + arg;
        loadingText.gameObject.SetActive(show);
    }
}
