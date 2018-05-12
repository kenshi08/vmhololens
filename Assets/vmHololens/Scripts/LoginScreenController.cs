using UnityEngine;
using UnityEngine.UI;

public class LoginScreenController : MonoBehaviour {

    public InputField inputfldIP;
    public InputField inputfldUsername;
    public InputField inputfldPassword;

    public Text displayMsg;

    public vSphereController vSphereController;
    public ConnectionManager connectionManager;
    public GameObject loginUICanvas;

    private bool isLoginPressed;

    void Awake()
    {
    }

    // Use this for initialization
    void Start () {
        DisplayLoginStatus(Constants.INITIAL_LOGIN_MESSAGE, true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Login the vSphere
    /// </summary>
    public void Login()
    {
        if(isLoginPressed)
        {
            return;
        }

        isLoginPressed = true;

        if (inputfldPassword == null || inputfldUsername == null || inputfldIP == null || inputfldPassword.text == string.Empty || inputfldUsername.text == string.Empty || inputfldIP.text == string.Empty)
        {
            DisplayLoginStatus(Constants.FIELDS_ARE_EMPTY, true);
        }else
        {
            ConnectionManager.instance.CreateClientAndLogin(inputfldIP.text, inputfldUsername.text, inputfldPassword.text);
        }
    }

    /// <summary>
    /// Exits App
    /// </summary>
    public void Exit()
    {
        Application.Quit();
    }

    /// <summary>
    /// Display msg if the user entered wrong or empty text field
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="flag"></param>
    private void DisplayLoginStatus(string msg, bool flag)
    {
        DisplayLoginStatus(flag);
        displayMsg.text = msg;
    }

    /// <summary>
    /// To toggle the message 
    /// </summary>
    /// <param name="flag"></param>
    private void DisplayLoginStatus(bool flag)
    {
        displayMsg.gameObject.SetActive(flag);
    }


    void OnEnable()
    {
        ConnectionManager.instance.OnLogin += OnLogin;
    }


    void OnDisable()
    {
        ConnectionManager.instance.OnLogin -= OnLogin;
    }

    /// <summary>
    /// Subscibe to LoginAction(bool isLoginSucess) in
    /// ConnectionManager as this is Async call
    /// </summary>
    /// <param name="isLoginSucess"></param>
    void OnLogin(bool isLoginSucess)
    {
        if (isLoginSucess)
        {
            DisplayLoginStatus(false);
            loginUICanvas.SetActive(false);
            vSphereController.Init();
        }
        else
        {
            DisplayLoginStatus(Constants.INVALID_CREDENTIALS, true);
        }
    }
}
