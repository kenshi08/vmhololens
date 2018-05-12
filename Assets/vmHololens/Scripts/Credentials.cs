using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credentials : MonoBehaviour {

    public string UserName;
    public string Password;
    public string IP;

    private LoginScreenController loginScreenController;

    void Awake()
    {
        loginScreenController = GetComponent<LoginScreenController>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        loginScreenController.inputfldUsername.text = UserName;
        loginScreenController.inputfldPassword.text = Password;
        loginScreenController.inputfldIP.text = IP;

    }
}
