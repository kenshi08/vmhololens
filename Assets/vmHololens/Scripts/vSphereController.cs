using UnityEngine;

/// <summary>
/// Manages the lifecycle of Clusters, Hosts, and VMs
/// </summary>
public class vSphereController : MonoBehaviour {

    /// <summary>
    /// Cluster controller that manages all clusters
    /// </summary>
    private ClusterController clusterController;


    void Awake()
    {
        clusterController = GetComponent<ClusterController>();    
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>
    /// Initializes the CLusters 
    /// and then subsequent components if login is successfull
    /// </summary>
    public void Init()
    {
        clusterController.Init();
    }
}
