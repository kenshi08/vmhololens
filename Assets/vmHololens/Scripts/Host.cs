using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;


[Serializable]
public class Host : MonoBehaviour {
    public List<VM> vm = new List<VM>();
    
    public Text info;

    public Transform VMAnchor;

    private VMController vmController;

    private vapitypes.Host aboutThisHost;

    public vapitypes.Host AboutThisHost
    {
        get
        {
            return aboutThisHost;
        }
        set
        {
            aboutThisHost = value;
        }
    }


    void Awake()
    {
        vmController = GameObject.FindGameObjectWithTag("vSphereController").GetComponent<VMController>();
    }


    void Start()
    {
        
    }

    public void CreateVM()
    {
        if(vmController == null)
        {
            Debug.LogError("ref missing");
            return;
        }
        vmController.Init(this);
    }

    public GameObject GetVMAnchor()
    {
        return VMAnchor.gameObject;
    }

    public void Init(vapitypes.Host host)
    {
        AboutThisHost = host;

        var _info = info;
        
        if(_info!=null)
        {
            _info.text = "Host :" + host.host+"\nName :" + host.name + "\nConnection State :" + host.connection_state + "\nPower State :" + host.power_state;
        }
        else
        {
            Debug.LogError("Info text for host ref missing");
        }
    }
}
