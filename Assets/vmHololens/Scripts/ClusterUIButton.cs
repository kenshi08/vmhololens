using UnityEngine;

public class ClusterUIButton : MonoBehaviour {

    /// <summary>
    /// Ref of which cluster is associated with this button
    /// </summary>
    public Cluster ClusterRefInScene
    {
        get;
        set;
    }

    /// <summary>
    ///To store ClusterController reference
    /// </summary>
    private ClusterController clustercontroller;

    void Start()
    {
        clustercontroller = GameObject.FindGameObjectWithTag("vSphereController").GetComponent<ClusterController>();
    }

    /// <summary>
    /// Ask ClusterController to enable the cluster to which it is assocuated and hide else
    /// </summary>
    public void EnableCluster()
    {
        if(ClusterRefInScene != null)
        {
            clustercontroller.EnableCluster(ClusterRefInScene);
        }
    }
}
