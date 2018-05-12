using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dustbin : MonoBehaviour {

    public GameObject ConfirmationUI;

    private VMController vmController;

    private VM refOfVMToDelete;

    void Awake()
    {
        
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "vm")
        {
            if(other.GetComponent<VM>().IsVMDropped)
            {
                ShowConfirmationsUI(true);
                refOfVMToDelete = other.GetComponent<VM>();

                // Cancel delete if user does not interact with confirmation box within 5 sec
                Invoke("CancelDelete", 10);
            }
        }
    }
    public void DeleteVM()
    {
        ShowConfirmationsUI(false);
        ConnectionManager.instance.DeleteVM(refOfVMToDelete);
    }

    public void CancelDelete()
    {
        ShowConfirmationsUI(false);
        refOfVMToDelete = null;
    }

    private void ShowConfirmationsUI(bool arg)
    {
        ConfirmationUI.SetActive(arg);
    }
}
