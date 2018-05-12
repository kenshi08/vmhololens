using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VMController : MonoBehaviour {
    public VM vmPrefab;
   
    public float distBtwVM = 0.5f;

    public int row = 5;
    public int column = 5;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}



    int currentVMCount = 0;
    private List<VM> CreateVM(List<vapitypes.Summary> vSphere_vm, Host parentHost)
    {
        currentVMCount = 0;
        var VMAnchor = parentHost.GetVMAnchor();
        List<VM> vmList = new List<VM>();
        var tempPos = VMAnchor.transform.position;
        for (int i = 0; i < row; i++)
        {
            tempPos = tempPos + new Vector3(0, i * distBtwVM, i*distBtwVM);
            for (int j = 0;  j < column;  j++)
            {
                VM _vm = Instantiate(vmPrefab);
                _vm.transform.position = tempPos;
                tempPos = tempPos + new Vector3(distBtwVM, 0, 0);
                _vm.Init(vSphere_vm[currentVMCount],parentHost);
                vmList.Add(_vm);
                _vm.transform.parent = parentHost.gameObject.transform;
                currentVMCount++;
                if (currentVMCount > vSphere_vm.Count - 1)
                {
                    break;
                }
            }
            tempPos = VMAnchor.transform.position;
            if(currentVMCount > vSphere_vm.Count-1)
            {
                break;
            }
        }
        return vmList;
    }


    public void Init(Host parentHost)
    {
        // Request clusters from connection manager
        ConnectionManager.instance.ResquestForVMs(this, parentHost);
    }

    public void OnVMRquested(List<vapitypes.Summary> vSphere_vm, Host parentHost)
    {
        var vmCount = vSphere_vm.Count;
        if (vmCount == 0)
        {
            Debug.Log("No VM found");
        }
        else
        {
            parentHost.vm = CreateVM(vSphere_vm, parentHost);
        }
    }

    public void DeleteVM(VM vm)
    {
        ConnectionManager.instance.DeleteVM(vm);
    }
}
